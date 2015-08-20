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
using Declarations.Enums;
using Declarations.Players;
using Implementation.Exceptions;
using LibVlcWrapper;
using System;

namespace Implementation.Players
{
    internal class AudioPlayer : BasicPlayer, IAudioPlayer
    {
        private AudioRenderer _mRender = null;

        public AudioPlayer(IntPtr hMediaLib)
            : base(hMediaLib)
        {

        }

        #region IAudioPlayer Members

        public int Volume
        {
            get
            {
                return LibVlcMethods.libvlc_audio_get_volume(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_set_volume(MHMediaPlayer, value);
            }
        }

        public bool Mute
        {
            get
            {
                return LibVlcMethods.libvlc_audio_get_mute(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_set_mute(MHMediaPlayer, value);
            }
        }

        public long Delay
        {
            get
            {
                return LibVlcMethods.libvlc_audio_get_delay(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_set_delay(MHMediaPlayer, value);
            }
        }

        public AudioChannelType Channel
        {
            get
            {
                return (AudioChannelType)LibVlcMethods.libvlc_audio_get_channel(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_set_channel(MHMediaPlayer, (LibvlcAudioOutputChannelT)value);
            }
        }

        public void ToggleMute()
        {
            LibVlcMethods.libvlc_audio_toggle_mute(MHMediaPlayer);
        }

        public IAudioRenderer CustomAudioRenderer
        {
            get 
            {
                if (_mRender == null)
                {
                    _mRender = new AudioRenderer(MHMediaPlayer);
                }
                return _mRender; 
            }
        }

        public AudioOutputDeviceType DeviceType
        {
            get
            {
                return (AudioOutputDeviceType)LibVlcMethods.libvlc_audio_output_get_device_type(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_output_set_device_type(MHMediaPlayer, (LibvlcAudioOutputDeviceTypesT)value);
            }
        }

        public void SetAudioOutputModuleAndDevice(AudioOutputModuleInfo module, AudioOutputDeviceInfo device)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            if (device != null)
            {
                LibVlcMethods.libvlc_audio_output_device_set(MHMediaPlayer, module.Name.ToUtf8(), device.Id.ToUtf8());
            }

            var res = LibVlcMethods.libvlc_audio_output_set(MHMediaPlayer, module.Name.ToUtf8());
            if (res < 0)
            {
                throw new LibVlcException();
            }
        }

        #endregion

        public override void Play()
        {
            base.Play();
            if (_mRender != null)
            {
                _mRender.StartTimer();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_mRender != null)
            {
                _mRender.Dispose();
                _mRender = null;
            }

            base.Dispose(disposing);
        }

        public void SetEqualizer(Equalizer equalizer)
        {
            if (equalizer == null)
            {
                LibVlcMethods.libvlc_media_player_set_equalizer(MHMediaPlayer, IntPtr.Zero);
                return;
            }

            LibVlcMethods.libvlc_media_player_set_equalizer(MHMediaPlayer, equalizer.Handle);
        }
    }
}
