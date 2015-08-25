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
using System.Windows.Forms;
using Declarations;
using Declarations.Enums;
using Declarations.Media;
using Declarations.Players;
using Implementation;

namespace nVLC_Demo_MemoryInputOutput
{
    public partial class Form1 : Form
    {
        IMediaPlayerFactory _mFactory;
        IVideoPlayer _mSourcePlayer;
        IVideoPlayer _mRenderPlayer;
        IMemoryInputMedia _mInputMedia;
        const long MicroSecondsInSecomd = 1000 * 1000;
        long _microSecondsBetweenFrame;
        long _frameCounter;
        FrameData _data = new FrameData() { Dts = -1 };
        const int DefaultFps = 24;
        Timer _timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = 1000;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Text = _mInputMedia.PendingFramesCount.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _mFactory = new MediaPlayerFactory(true);
            _mSourcePlayer = _mFactory.CreatePlayer<IVideoPlayer>();
            _mSourcePlayer.Events.PlayerPlaying += new EventHandler(Events_PlayerPlaying);
            _mSourcePlayer.Mute = true;
            _mRenderPlayer = _mFactory.CreatePlayer<IVideoPlayer>();
            _mRenderPlayer.WindowHandle = panel1.Handle;
            _mInputMedia = _mFactory.CreateMedia<IMemoryInputMedia>(MediaStrings.Imem);
            SetupOutput(_mSourcePlayer.CustomRendererEx);
        }

        void Events_PlayerPlaying(object sender, EventArgs e)
        {
            _microSecondsBetweenFrame = (long)(MicroSecondsInSecomd / (_mSourcePlayer.Fps != 0 ? _mSourcePlayer.Fps : DefaultFps));
        }

        private void SetupOutput(IMemoryRendererEx iMemoryRenderer)
        {
            iMemoryRenderer.SetFormatSetupCallback(OnSetupCallback);
            iMemoryRenderer.SetExceptionHandler(OnErrorCallback);
            iMemoryRenderer.SetCallback(OnNewFrameCallback);
        }

        private BitmapFormat OnSetupCallback(BitmapFormat format)
        {
            SetupInput(format);
            return new BitmapFormat(format.Width, format.Height, ChromaType.Rv24);
        }

        private void OnErrorCallback(Exception error)
        {
            MessageBox.Show(error.Message);
        }

        private void OnNewFrameCallback(PlanarFrame frame)
        {          
            _data.Data = frame.Planes[0];
            _data.DataSize = frame.Lenghts[0];
            _data.Pts = _frameCounter++ * _microSecondsBetweenFrame;
            _mInputMedia.AddFrame(_data);

            if (/*m_inputMedia.PendingFramesCount == 10 && */!_mRenderPlayer.IsPlaying)
            {
                _mRenderPlayer.Play();
            }
        }

        private void SetupInput(BitmapFormat format)
        {
            var streamInfo = new StreamInfo();
            streamInfo.Category = StreamCategory.Video;
            streamInfo.Codec = VideoCodecs.Bgr24;
            streamInfo.Width = format.Width;
            streamInfo.Height = format.Height;
            streamInfo.Size = format.ImageSize;

            _mInputMedia.Initialize(streamInfo);
            _mInputMedia.SetExceptionHandler(OnErrorCallback);
            _mRenderPlayer.Open(_mInputMedia);           
        }

        private void OpenSourceMedia(string path)
        {
            var media = _mFactory.CreateMedia<IMediaFromFile>(path);
            _mSourcePlayer.Open(media);                    
            _mSourcePlayer.Play();
            _timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenSourceMedia(ofd.FileName);
            }
        }
    }
}
