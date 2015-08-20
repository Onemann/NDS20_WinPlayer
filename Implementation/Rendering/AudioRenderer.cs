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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using Declarations;
using Declarations.Enums;
using LibVlcWrapper;

namespace Implementation
{
    internal unsafe sealed class AudioRenderer : DisposableBase, IAudioRenderer
    {
        private IntPtr _mHMediaPlayer;
        private AudioCallbacks _mCallbacks = new AudioCallbacks();
        private Func<SoundFormat, SoundFormat> _mFormatSetupCb;
        private SoundFormat _mFormat;
        private List<Delegate> _mCallbacksDelegates = new List<Delegate>();
        private Action<Exception> _mExcHandler;
        private IntPtr _mHSetup;
        private IntPtr _mHVolume;
        private IntPtr _mHSound;
        private IntPtr _mHPause;
        private IntPtr _mHResume;
        private IntPtr _mHFlush;
        private IntPtr _mHDrain;
        private Timer _mTimer = new Timer();
        volatile int _mFrameRate = 0;
        int _mLatestFps;

        public AudioRenderer(IntPtr hMediaPlayer)
        {
            _mHMediaPlayer = hMediaPlayer;

            PlayCallbackEventHandler pceh = PlayCallback;
            VolumeCallbackEventHandler vceh = VolumeCallback;
            SetupCallbackEventHandler sceh = SetupCallback;
            AudioCallbackEventHandler pause = PauseCallback;
            AudioCallbackEventHandler resume = ResumeCallback;
            AudioCallbackEventHandler flush = FlushCallback;
            AudioDrainCallbackEventHandler drain = DrainCallback;

            _mHSound = Marshal.GetFunctionPointerForDelegate(pceh);
            _mHVolume = Marshal.GetFunctionPointerForDelegate(vceh);
            _mHSetup = Marshal.GetFunctionPointerForDelegate(sceh);
            _mHPause = Marshal.GetFunctionPointerForDelegate(pause);
            _mHResume = Marshal.GetFunctionPointerForDelegate(resume);
            _mHFlush = Marshal.GetFunctionPointerForDelegate(flush);
            _mHDrain = Marshal.GetFunctionPointerForDelegate(drain);

            _mCallbacksDelegates.Add(pceh);
            _mCallbacksDelegates.Add(vceh);
            _mCallbacksDelegates.Add(sceh);
            _mCallbacksDelegates.Add(pause);
            _mCallbacksDelegates.Add(resume);
            _mCallbacksDelegates.Add(flush);
            _mCallbacksDelegates.Add(drain);

            _mTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _mTimer.Interval = 1000;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mLatestFps = _mFrameRate;
            _mFrameRate = 0;
        }

        public void SetCallbacks(VolumeChangedEventHandler volume, NewSoundEventHandler sound)
        {
            _mCallbacks.VolumeCallback = volume;
            _mCallbacks.SoundCallback = sound;
            LibVlcMethods.libvlc_audio_set_callbacks(_mHMediaPlayer, _mHSound, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            LibVlcMethods.libvlc_audio_set_volume_callback(_mHMediaPlayer, _mHVolume);
        }

        public void SetCallbacks(AudioCallbacks callbacks)
        {
            if (callbacks.SoundCallback == null)
            {
                throw new ArgumentNullException("Sound playback callback must be set");
            }

            _mCallbacks = callbacks;
            LibVlcMethods.libvlc_audio_set_callbacks(_mHMediaPlayer, _mHSound, _mHPause, _mHResume, _mHFlush, _mHDrain, IntPtr.Zero);
            LibVlcMethods.libvlc_audio_set_volume_callback(_mHMediaPlayer, _mHVolume);
        }

        public void SetFormat(SoundFormat format)
        {
            _mFormat = format;
            LibVlcMethods.libvlc_audio_set_format(_mHMediaPlayer, _mFormat.Format.ToUtf8(), _mFormat.Rate, _mFormat.Channels);
        }

        public void SetFormatCallback(Func<SoundFormat, SoundFormat> formatSetup)
        {
            _mFormatSetupCb = formatSetup;
            LibVlcMethods.libvlc_audio_set_format_callbacks(_mHMediaPlayer, _mHSetup, IntPtr.Zero);
        }

        internal void StartTimer()
        {
            _mTimer.Start();
        }

        private void PlayCallback(void* data, void* samples, uint count, long pts)
        {
            var s = new Sound();
            s.SamplesData = new IntPtr(samples);
            s.SamplesSize = (uint)(count * _mFormat.BlockSize);
            s.Pts = pts;

            if (_mCallbacks.SoundCallback != null)
            {
                _mCallbacks.SoundCallback(s);
            }
        }

        private void PauseCallback(void* data, long pts)
        {
            if (_mCallbacks.PauseCallback != null)
            {
                _mCallbacks.PauseCallback(pts);
            }
        }

        private void ResumeCallback(void* data, long pts)
        {
            if (_mCallbacks.ResumeCallback != null)
            {
                _mCallbacks.ResumeCallback(pts);
            }
        }

        private void FlushCallback(void* data, long pts)
        {
            if (_mCallbacks.FlushCallback != null)
            {
                _mCallbacks.FlushCallback(pts);
            }
        }

        private void DrainCallback(void* data)
        {
            if (_mCallbacks.DrainCallback != null)
            {
                _mCallbacks.DrainCallback();
            }
        }

        private void VolumeCallback(void* data, float volume, bool mute)
        {
            if (_mCallbacks.VolumeCallback != null)
            {
                _mCallbacks.VolumeCallback(volume, mute);
            }
        }

        private int SetupCallback(void** data, char* format, int* rate, int* channels)
        {
            var pFormat = new IntPtr(format);
            var formatStr = Marshal.PtrToStringAnsi(pFormat);

            SoundType sType;
            if (!Enum.TryParse<SoundType>(formatStr, out sType))
            {
                var exc = new ArgumentException("Unsupported sound type " + formatStr);
                if (_mExcHandler != null)
                {
                    _mExcHandler(exc);
                    return 1;
                }
                else
                {
                    throw exc;
                }
            }

            _mFormat = new SoundFormat(sType, *rate, *channels);
            if (_mFormatSetupCb != null)
            {
                _mFormat = _mFormatSetupCb(_mFormat);
            }
            
            Marshal.Copy(_mFormat.Format.ToUtf8(), 0, pFormat, 4);
            *rate = _mFormat.Rate;
            *channels = _mFormat.Channels;

            return _mFormat.UseCustomAudioRendering == true ? 0 : 1;
        }

        protected override void Dispose(bool disposing)
        {
            LibVlcMethods.libvlc_audio_set_format_callbacks(_mHMediaPlayer, IntPtr.Zero, IntPtr.Zero);
            LibVlcMethods.libvlc_audio_set_callbacks(_mHMediaPlayer, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            if (disposing)
            {
                _mFormatSetupCb = null;
                _mExcHandler = null;
                _mCallbacks = null;
                _mCallbacksDelegates.Clear();
            }          
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _mExcHandler = handler;
        }

        public int ActualFrameRate
        {
            get
            {
                return _mLatestFps;
            }
        }
    }
}
