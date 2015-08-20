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
using System.ComponentModel;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using System.Linq;

namespace nVLC_Demo_WinForms
{
    public partial class Form1 : Form
    {
        IMediaPlayerFactory _mFactory;
        IDiskPlayer _mPlayer;
        IMediaFromFile _mMedia;

        public Form1()
        {
            InitializeComponent();

            _mFactory = new MediaPlayerFactory(true);
            _mPlayer = _mFactory.CreatePlayer<IDiskPlayer>();

            _mPlayer.Events.PlayerPositionChanged += new EventHandler<MediaPlayerPositionChanged>(Events_PlayerPositionChanged);
            _mPlayer.Events.TimeChanged += new EventHandler<MediaPlayerTimeChanged>(Events_TimeChanged);
            _mPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
            _mPlayer.Events.PlayerStopped += new EventHandler(Events_PlayerStopped);
            _mPlayer.AspectRatio = AspectRatioMode.Default;
            _mPlayer.WindowHandle = panel1.Handle;
            trackBar2.Value = _mPlayer.Volume > 0 ? _mPlayer.Volume : 0;

            UiSync.Init(this);
        }

        void Events_PlayerStopped(object sender, EventArgs e)
        {
            UiSync.Execute(() => InitControls());
        }

        void Events_MediaEnded(object sender, EventArgs e)
        {
            UiSync.Execute(() => InitControls());
        }

        private void InitControls()
        {
            trackBar1.Value = 0;
            lblTime.Text = "00:00:00.00";
            lblDuration.Text = "00:00:00";
        }

        void Events_TimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            string tmpstr;
            tmpstr = TimeSpan.FromMilliseconds(e.NewTime).ToString() + ".00";
            UiSync.Execute(() => lblTime.Text = tmpstr.Substring(0, 11));
        }

        void Events_PlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            UiSync.Execute(() => trackBar1.Value = (int)(e.NewPosition * 100));
        }

        private void LoadMedia()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        void Events_StateChanged(object sender, MediaStateChange e)
        {
            //UISync.Execute(() => label1.Text = e.NewState.ToString());
            UiSync.Execute(() => this.Text = e.NewState.ToString());
            
        }

        void Events_DurationChanged(object sender, MediaDurationChange e)
        {
            UiSync.Execute(() => lblDuration.Text = TimeSpan.FromMilliseconds(e.NewDuration).ToString().Substring(0, 8));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadMedia();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                _mMedia = _mFactory.CreateMedia<IMediaFromFile>(textBox1.Text);
                _mMedia.Events.DurationChanged += new EventHandler<MediaDurationChange>(Events_DurationChanged);
                _mMedia.Events.StateChanged += new EventHandler<MediaStateChange>(Events_StateChanged);
                _mMedia.Events.ParsedChanged += new EventHandler<MediaParseChange>(Events_ParsedChanged);

                _mPlayer.Open(_mMedia);
                _mMedia.Parse(true);

                _mPlayer.Play();
            }
            else
            {
                errorProvider1.SetError(textBox1, "Please select media path first !");
            }
        }

        void Events_ParsedChanged(object sender, MediaParseChange e)
        {
            Console.WriteLine(e.Parsed);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _mPlayer.Volume = trackBar2.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _mPlayer.Position = (float)trackBar1.Value / 100;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _mPlayer.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mPlayer.Pause();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _mPlayer.ToggleMute();

            button1.Text = _mPlayer.Mute ? "Unmute" : "Mute";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private class UiSync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                _sync.BeginInvoke(action, null);
            }
        }
    }
}
