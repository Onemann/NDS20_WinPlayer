//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Structures;
using Implementation.Events;
using Implementation.Exceptions;
using Implementation.Utils;
using LibVlcWrapper;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Implementation.Media
{
    internal class BasicMedia : DisposableBase, IMedia, INativePointer, IReferenceCount, IEventProvider
    {
        protected readonly IntPtr MHMediaLib;
        protected IntPtr MHMedia;
        protected string MPath;
        IntPtr _mHEventManager = IntPtr.Zero;
        IMediaEvents _mEvents;
        private SlaveMedia _slaveMedia;

        public BasicMedia(IntPtr hMediaLib)
        {
            MHMediaLib = hMediaLib;
        }

        public BasicMedia(IntPtr hMedia, ReferenceCountAction refCountAction)
        {
            MHMedia = hMedia;
            var pData = LibVlcMethods.libvlc_media_get_mrl(MHMedia);
            MPath = Marshal.PtrToStringAnsi(pData);
            switch (refCountAction)
            {
                case ReferenceCountAction.AddRef:
                    AddRef();
                    break;

                case ReferenceCountAction.Release:
                    Release();
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        #region IMedia Members

        public virtual string Input
        {
            get
            {
                return MPath;
            }
            set
            {
                MPath = value;
                MHMedia = LibVlcMethods.libvlc_media_new_location(MHMediaLib, MPath.ToUtf8());
            }
        }

        public MediaState State
        {
            get
            {
                return (MediaState)LibVlcMethods.libvlc_media_get_state(MHMedia);
            }
        }

        public void AddOptions(IEnumerable<string> options)
        {
            foreach (var item in options)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    LibVlcMethods.libvlc_media_add_option(MHMedia, item.ToUtf8());
                }
            }
        }

        public void AddOptionFlag(string option, int flag)
        {
            LibVlcMethods.libvlc_media_add_option_flag(MHMedia, option.ToUtf8(), flag);
        }

        public IMedia Duplicate()
        {
            var clone = LibVlcMethods.libvlc_media_duplicate(MHMedia);
            return new BasicMedia(clone, ReferenceCountAction.None);
        }

        public void Parse(bool aSync)
        {
            if (aSync)
            {
                LibVlcMethods.libvlc_media_parse_async(MHMedia);
            }
            else
            {
                LibVlcMethods.libvlc_media_parse(MHMedia);
            }
        }

        public bool IsParsed
        {
            get
            {
                return LibVlcMethods.libvlc_media_is_parsed(MHMedia);
            }
        }

        public IntPtr Tag
        {
            get
            {
                return LibVlcMethods.libvlc_media_get_user_data(MHMedia);
            }
            set
            {
                LibVlcMethods.libvlc_media_set_user_data(MHMedia, value);
            }
        }

        public IMediaEvents Events
        {
            get
            {
                if (_mEvents == null)
                {
                    _mEvents = new MediaEventManager(this);
                }

                return _mEvents;
            }
        }

        public MediaStatistics Statistics
        {
            get
            {
                LibvlcMediaStatsT t;

                var num = LibVlcMethods.libvlc_media_get_stats(MHMedia, out t);

                return t.ToMediaStatistics();
            }
        }

        public IMediaList SubItems
        {
            get
            {
                var hMediaList = LibVlcMethods.libvlc_media_subitems(MHMedia);
                if (hMediaList == IntPtr.Zero)
                {
                    return null;
                }

                return new MediaList(hMediaList, ReferenceCountAction.None);
            }
        }

        public MediaTrack[] TracksInfoEx
        {
            get
            {
                unsafe
                {
                    LibvlcMediaTrackT** ppTracks;
                    var num = LibVlcMethods.libvlc_media_tracks_get(MHMedia, &ppTracks);
                    if (num == 0 || ppTracks == null)
                    {
                        throw new LibVlcException();
                    }

                    var list = new List<MediaTrack>(num);
                    for (var i = 0; i < num; i++)
                    {
                        MediaTrack track = null;
                        var pTrackInfo = ppTracks[i];
                        switch (pTrackInfo->i_type)
                        {
                            case LibvlcTrackTypeT.LibvlcTrackAudio:
                                var audio = new AudioTrack();
                                var audioTrack = (LibvlcAudioTrackT*)pTrackInfo->media.ToPointer();
                                audio.Channels = audioTrack->i_channels;
                                audio.Rate = audioTrack->i_rate;
                                track = audio;
                                break;

                            case LibvlcTrackTypeT.LibvlcTrackVideo:
                                var video = new VideoTrack();
                                var videoTrack = (LibvlcVideoTrackT*)pTrackInfo->media.ToPointer();
                                video.Width = videoTrack->i_width;
                                video.Height = videoTrack->i_height;
                                video.SarDen = videoTrack->i_sar_den;
                                video.SarNum = videoTrack->i_sar_num;
                                video.FrameRateDen = videoTrack->i_frame_rate_den;
                                video.FrameRateNum = videoTrack->i_frame_rate_num;
                                track = video;
                                break;

                            case LibvlcTrackTypeT.LibvlcTrackText:
                                var sub = new SubtitlesTrack();
                                var subtitleTrack = (LibvlcSubtitleTrackT*)pTrackInfo->media.ToPointer();
                                sub.Encoding = subtitleTrack->psz_encoding == null ? null : Marshal.PtrToStringAnsi(subtitleTrack->psz_encoding);
                                track = sub;
                                break;

                            case LibvlcTrackTypeT.LibvlcTrackUnknown:
                            default:
                                track = new MediaTrack();
                                break;
                        }

                        track.Bitrate = pTrackInfo->i_bitrate;
                        track.Codec = MiscUtils.DwordToFourCc(pTrackInfo->i_codec);
                        track.Id = pTrackInfo->i_id;
                        track.OriginalFourCc = MiscUtils.DwordToFourCc(pTrackInfo->i_original_fourcc);
                        track.Language = pTrackInfo->psz_language == null ? null : Marshal.PtrToStringAnsi(pTrackInfo->psz_language);
                        track.Description = pTrackInfo->psz_description == null ? null : Marshal.PtrToStringAnsi(pTrackInfo->psz_description);
                        list.Add(track);
                    }

                    LibVlcMethods.libvlc_media_tracks_release(ppTracks, num);
                    return list.ToArray();
                }
            }
        }

        #endregion

        #region INativePointer Members

        public IntPtr Pointer
        {
            get
            {
                return MHMedia;
            }
        }

        #endregion

        #region IReferenceCount Members

        public void AddRef()
        {
            LibVlcMethods.libvlc_media_retain(MHMedia);
        }

        public void Release()
        {
            try
            {
                LibVlcMethods.libvlc_media_release(MHMedia);
            }
            catch (Exception)
            { }
        }

        #endregion

        #region IEventProvider Members

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_mHEventManager == IntPtr.Zero)
                {
                    _mHEventManager = LibVlcMethods.libvlc_media_event_manager(MHMedia);
                }

                return _mHEventManager;
            }
        }

        #endregion

        #region IEqualityComparer<IMedia> Members

        public bool Equals(IMedia x, IMedia y)
        {
            var x1 = (INativePointer)x;
            var y1 = (INativePointer)y;

            return x1.Pointer == y1.Pointer;
        }

        public int GetHashCode(IMedia obj)
        {
            return ((INativePointer)obj).Pointer.GetHashCode();
        }

        #endregion

        public override bool Equals(object obj)
        {
            return this.Equals((IMedia)obj, this);
        }

        public override int GetHashCode()
        {
            return MHMedia.GetHashCode();
        }

        public SlaveMedia SlaveMedia
        {
            get
            {
                return _slaveMedia;
            }
            set
            {
                AddOptions(new[] { value.ToString() });
                _slaveMedia = value;
            }
        }
    }
}
