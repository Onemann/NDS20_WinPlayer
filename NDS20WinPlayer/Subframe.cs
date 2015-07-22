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

            # region Create Player
            m_factory = new MediaPlayerFactory(true);
            m_player = m_factory.CreatePlayer<IDiskPlayer>();
            //            m_player.Events.PlayerPositionChanged += new EventHandler<MediaPlayerPositionChanged>(Events_PlayerPositionChanged);

            //m_player.WindowHandle = Subframe.ActiveForm.Handle;
            #endregion

            JsonObjectCollection col = (JsonObjectCollection)paramSchedule;
            frameInfoStrc frameInfo = new frameInfoStrc();

            frameInfo.xPos = int.Parse(col["xPos"].GetValue().ToString());
            frameInfo.yPos = int.Parse(col["yPos"].GetValue().ToString());

            this.Location = new System.Drawing.Point(frameInfo.xPos, frameInfo.yPos);
        }

        private void Subframe_Activated(object sender, EventArgs e)
        {
            //this.Parent.Focus();
        }

        private void Subframe_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("ddd");
        }

    }

}

