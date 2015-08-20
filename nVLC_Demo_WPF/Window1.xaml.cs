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
using System.Windows;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace nVLC_Demo_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        IMediaPlayerFactory _mFactory;
        IVideoPlayer _mPlayer;
        IMediaFromFile _mMedia;
        private volatile bool _mIsDrag;

        public Window1()
        {
            InitializeComponent();

            var p = new System.Windows.Forms.Panel();
            p.BackColor = System.Drawing.Color.Black;
            WindowsFormsHost1.Child = p;

            _mFactory = new MediaPlayerFactory(true);
            _mPlayer = _mFactory.CreatePlayer<IVideoPlayer>();

            this.DataContext = _mPlayer;

            _mPlayer.Events.PlayerPositionChanged += new EventHandler<MediaPlayerPositionChanged>(Events_PlayerPositionChanged);
            _mPlayer.Events.TimeChanged += new EventHandler<MediaPlayerTimeChanged>(Events_TimeChanged);
            _mPlayer.Events.MediaEnded += new EventHandler(Events_MediaEnded);
            _mPlayer.Events.PlayerStopped += new EventHandler(Events_PlayerStopped);

            _mPlayer.WindowHandle = p.Handle;
            Slider2.Value = _mPlayer.Volume;
        }

        void Events_PlayerStopped(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                InitControls();
            }));
        }

        void Events_MediaEnded(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                InitControls();
            }));
        }

        private void InitControls()
        {
            Slider1.Value = 0;
            Label1.Content = "00:00:00";
            Label3.Content = "00:00:00";
        }

        void Events_TimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                Label1.Content = TimeSpan.FromMilliseconds(e.NewTime).ToString().Substring(0, 8);
            }));
        }

        void Events_PlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                if (!_mIsDrag)
                {
                    Slider1.Value = (double)e.NewPosition;
                }
            }));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBlock1.Text = ofd.FileName;
                _mMedia = _mFactory.CreateMedia<IMediaFromFile>(ofd.FileName);
                _mMedia.Events.DurationChanged += new EventHandler<MediaDurationChange>(Events_DurationChanged);
                _mMedia.Events.StateChanged += new EventHandler<MediaStateChange>(Events_StateChanged);

                _mPlayer.Open(_mMedia);
                _mMedia.Parse(true);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            _mPlayer.Play();
        }

        void Events_StateChanged(object sender, MediaStateChange e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {

            }));
        }

        void Events_DurationChanged(object sender, MediaDurationChange e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                Label3.Content = TimeSpan.FromMilliseconds(e.NewDuration).ToString().Substring(0, 8);
            }));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            _mPlayer.Pause();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            _mPlayer.Stop();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            _mPlayer.ToggleMute();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mPlayer != null)
            {
                _mPlayer.Volume = (int)e.NewValue;
            }
        }

        private void slider1_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _mPlayer.Position = (float)Slider1.Value;
            _mIsDrag = false;
        }

        private void slider1_DragStarted(object sender, DragStartedEventArgs e)
        {
            _mIsDrag = true;
        }
    }
}
