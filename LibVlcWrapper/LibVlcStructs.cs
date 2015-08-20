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

using System;
using System.Runtime.InteropServices;

namespace LibVlcWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    
    public struct LibvlcLogMessageT
    {
        public UInt32 sizeof_msg;   
        public Int32 i_severity;    
        public IntPtr psz_type;     
        public IntPtr psz_name;     
        public IntPtr psz_header;   
        public IntPtr psz_message;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcMediaStatsT
    {
        /* Input */
        public int i_read_bytes;
        public float f_input_bitrate;

        /* Demux */
        public int i_demux_read_bytes;
        public float f_demux_bitrate;
        public int i_demux_corrupted;
        public int i_demux_discontinuity;

        /* Decoders */
        public int i_decoded_video;
        public int i_decoded_audio;

        /* Video Output */
        public int i_displayed_pictures;
        public int i_lost_pictures;

        /* Audio output */
        public int i_played_abuffers;
        public int i_lost_abuffers;

        /* Stream output */
        public int i_sent_packets;
        public int i_sent_bytes;
        public float f_send_bitrate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcMediaTrackInfoT
    {
        public UInt32 i_codec;
        public int i_id;
        public LibvlcTrackTypeT i_type;
        public int i_profile;
        public int i_level;

        public LibvlcMediaTrackInfoType audio_video;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct LibvlcMediaTrackInfoType
    {
        [FieldOffset(0)]
        public Audio audio;

        [FieldOffset(0)]
        public Video video;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Audio
    {
        public int i_channels;
        public int i_rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Video
    {
        public int i_height;
        public int i_width;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcEventT
    {
        public LibvlcEventE type;
        public IntPtr p_obj;
        public MediaDescriptorUnion MediaDescriptor;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MediaDescriptorUnion
    {
        [FieldOffset(0)]
        public MediaMetaChanged media_meta_changed;

        [FieldOffset(0)]
        public MediaSubitemAdded media_subitem_added;

        [FieldOffset(0)]
        public MediaDurationChanged media_duration_changed;

        [FieldOffset(0)]
        public MediaParsedChanged media_parsed_changed;

        [FieldOffset(0)]
        public MediaFreed media_freed;

        [FieldOffset(0)]
        public MediaStateChanged media_state_changed;

        [FieldOffset(0)]
        public MediaPlayerPositionChanged media_player_position_changed;

        [FieldOffset(0)]
        public MediaPlayerTimeChanged media_player_time_changed;

        [FieldOffset(0)]
        public MediaPlayerTitleChanged media_player_title_changed;

        [FieldOffset(0)]
        public MediaPlayerSeekableChanged media_player_seekable_changed;

        [FieldOffset(0)]
        public MediaPlayerPausableChanged media_player_pausable_changed;

        [FieldOffset(0)]
        public MediaListItemAdded media_list_item_added;

        [FieldOffset(0)]
        public MediaListWillAddItem media_list_will_add_item;

        [FieldOffset(0)]
        public MediaListItemDeleted media_list_item_deleted;

        [FieldOffset(0)]
        public MediaListWillDeleteItem media_list_will_delete_item;

        [FieldOffset(0)]
        public MediaListPlayerNextItemSet media_list_player_next_item_set;

        [FieldOffset(0)]
        public MediaPlayerSnapshotTaken media_player_snapshot_taken;

        [FieldOffset(0)]
        public MediaPlayerLengthChanged media_player_length_changed;

        [FieldOffset(0)]
        public VlmMediaEvent vlm_media_event;

        [FieldOffset(0)]
        public MediaPlayerMediaChanged media_player_media_changed;
    }

    /* media descriptor */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaMetaChanged
    {
        public LibvlcMetaT meta_type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaSubitemAdded
    {
        public IntPtr new_child;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaDurationChanged
    {
        public long new_duration;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaParsedChanged
    {
        public int new_status;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaFreed
    {
        public IntPtr md;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaStateChanged
    {
        public LibvlcStateT new_state;
    }

    /* media instance */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerPositionChanged
    {
        public float new_position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerTimeChanged
    {
        public long new_time;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerTitleChanged
    {
        public int new_title;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerSeekableChanged
    {
        public int new_seekable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerPausableChanged
    {
        public int new_pausable;
    }

    /* media list */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaListItemAdded
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaListWillAddItem
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaListItemDeleted
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaListWillDeleteItem
    {
        public IntPtr item;
        public int index;
    }

    /* media list player */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaListPlayerNextItemSet
    {
        public IntPtr item;
    }

    /* snapshot taken */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerSnapshotTaken
    {
        public IntPtr psz_filename;
    }

    /* Length changed */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerLengthChanged
    {
        public long new_length;
    }

    /* VLM media */
    [StructLayout(LayoutKind.Sequential)]
    public struct VlmMediaEvent
    {
        public IntPtr psz_media_name;
        public IntPtr psz_instance_name;
    }

    /* Extra MediaPlayer */
    [StructLayout(LayoutKind.Sequential)]
    public struct MediaPlayerMediaChanged
    {
        public IntPtr new_media;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcModuleDescriptionT
    {
        public IntPtr psz_name;
        public IntPtr psz_shortname;
        public IntPtr psz_longname;
        public IntPtr psz_help;
        public IntPtr p_next;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcAudioOutputT
    {
        public IntPtr psz_name;
        public IntPtr psz_description;
        public IntPtr p_next;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcTrackDescriptionT
    {
        public int i_id;
        public IntPtr psz_name;
        public IntPtr p_next;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcAudioTrackT
    {
        public uint i_channels;
        public uint i_rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcVideoTrackT
    {
        public uint i_height;
        public uint i_width;
        public uint i_sar_num;
        public uint i_sar_den;
        public uint i_frame_rate_num;
        public uint i_frame_rate_den;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcSubtitleTrackT
    {
        public IntPtr psz_encoding;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LibvlcMediaTrackT
    {
        public uint i_codec;
        public uint i_original_fourcc;
        public int i_id;
        public LibvlcTrackTypeT i_type;
        public int i_profile;
        public int i_level;
        public IntPtr media;
        public uint i_bitrate;
        public IntPtr psz_language;
        public IntPtr psz_description;
    }
}