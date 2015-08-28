using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Json;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;




namespace NDS20WinPlayer
{
    public partial class Subframe : Form
    {
        IMediaPlayerFactory m_factory;
        IDiskPlayer m_player;
        IMediaFromFile m_media;

        public Subframe(JsonObject paramSchedule)
        {
            InitializeComponent();

            JsonObjectCollection col = (JsonObjectCollection)paramSchedule;
            frameInfoStrc frameInfo = new frameInfoStrc();

            frameInfo.xPos = int.Parse(col["xPos"].GetValue().ToString());
            frameInfo.yPos = int.Parse(col["yPos"].GetValue().ToString());
            frameInfo.width = int.Parse(col["width"].GetValue().ToString());
            frameInfo.height = int.Parse(col["height"].GetValue().ToString());
            frameInfo.contentsFileName = (string)col["fileName"].GetValue();
            frameInfo.mute = bool.Parse(col["mute"].GetValue().ToString());

            if (frameInfo.width == 0)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Width = frameInfo.width;
                this.Height = frameInfo.height;
                
            }
            this.Location = new System.Drawing.Point(frameInfo.xPos, frameInfo.yPos);

            # region ==== Create Player ====
            m_factory = new MediaPlayerFactory(true);
            m_player = m_factory.CreatePlayer<IDiskPlayer>();
            m_player.AspectRatio = AspectRatioMode.Default;
            m_player.Mute = frameInfo.mute;
           
            m_player.WindowHandle = this.pnlPlayerBack.Handle;

            UISync.Init(this);
            #endregion ======================

            #region ==== Contents play ====
            FileInfo contentsFileInfo = new FileInfo(@frameInfo.contentsFileName);
            m_media = m_factory.CreateMedia<IMediaFromFile>(contentsFileInfo.FullName);
            
            m_player.Open(m_media);
            m_media.Parse(true);

            m_player.Play();
            #endregion =====================
        }

        private class UISync
        {
            private static ISynchronizeInvoke Sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                Sync = sync;
            }

            public static void Execute(Action action)
            {
                Sync.BeginInvoke(action, null);
            }
        }
    }

}

