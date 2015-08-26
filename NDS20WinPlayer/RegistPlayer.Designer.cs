namespace NDS20WinPlayer
{
    partial class RegistPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistPlayer));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancelRegist = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.edtPlayerId = new DevExpress.XtraEditors.TextEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.btnRequestRegist = new DevExpress.XtraEditors.SimpleButton();
            this.lblMessage = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPlayerId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            resources.ApplyResources(this.labelControl1, "labelControl1");
            this.labelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl1.Name = "labelControl1";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl2.Appearance.Font")));
            resources.ApplyResources(this.labelControl2, "labelControl2");
            this.labelControl2.Name = "labelControl2";
            // 
            // btnCancelRegist
            // 
            this.btnCancelRegist.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btnCancelRegist.Appearance.Font")));
            this.btnCancelRegist.Appearance.Options.UseFont = true;
            this.btnCancelRegist.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelRegist.ImageIndex = 0;
            this.btnCancelRegist.ImageList = this.imageCollection1;
            this.btnCancelRegist.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            resources.ApplyResources(this.btnCancelRegist, "btnCancelRegist");
            this.btnCancelRegist.LookAndFeel.SkinName = "Blueprint";
            this.btnCancelRegist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancelRegist.Name = "btnCancelRegist";
            this.btnCancelRegist.Click += new System.EventHandler(this.btnCancelRegist_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertGalleryImage("cancel_16x16.png", "images/actions/cancel_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), 0);
            this.imageCollection1.Images.SetKeyName(0, "cancel_16x16.png");
            this.imageCollection1.InsertGalleryImage("apply_16x16.png", "images/actions/apply_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_16x16.png"), 1);
            this.imageCollection1.Images.SetKeyName(1, "apply_16x16.png");
            this.imageCollection1.InsertGalleryImage("borules_16x16.png", "images/business%20objects/borules_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/borules_16x16.png"), 2);
            this.imageCollection1.Images.SetKeyName(2, "borules_16x16.png");
            this.imageCollection1.InsertGalleryImage("info_16x16.png", "images/support/info_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/info_16x16.png"), 3);
            this.imageCollection1.Images.SetKeyName(3, "info_16x16.png");
            this.imageCollection1.InsertGalleryImage("index_16x16.png", "images/support/index_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/index_16x16.png"), 4);
            this.imageCollection1.Images.SetKeyName(4, "index_16x16.png");
            // 
            // edtPlayerId
            // 
            resources.ApplyResources(this.edtPlayerId, "edtPlayerId");
            this.edtPlayerId.Name = "edtPlayerId";
            this.edtPlayerId.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.edtPlayerId.Properties.LookAndFeel.SkinName = "Dark Side";
            this.edtPlayerId.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // pictureEdit2
            // 
            resources.ApplyResources(this.pictureEdit2, "pictureEdit2");
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.AllowFocused = false;
            this.pictureEdit2.Properties.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("pictureEdit2.Properties.Appearance.BackColor")));
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowMenu = false;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::NDS20WinPlayer.Properties.Resources.cj_png_powercast_hr_eng_1;
            resources.ApplyResources(this.pictureEdit1, "pictureEdit1");
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("pictureEdit1.Properties.Appearance.BackColor")));
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            // 
            // btnRequestRegist
            // 
            this.btnRequestRegist.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btnRequestRegist.Appearance.Font")));
            this.btnRequestRegist.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btnRequestRegist, "btnRequestRegist");
            this.btnRequestRegist.ImageIndex = 1;
            this.btnRequestRegist.ImageList = this.imageCollection1;
            this.btnRequestRegist.LookAndFeel.SkinName = "Blueprint";
            this.btnRequestRegist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnRequestRegist.Name = "btnRequestRegist";
            this.btnRequestRegist.Click += new System.EventHandler(this.btnRequestRegist_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("lblMessage.Appearance.Font")));
            this.lblMessage.Appearance.ImageIndex = 4;
            this.lblMessage.Appearance.ImageList = this.imageCollection1;
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.HtmlImages = this.imageCollection1;
            this.lblMessage.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblMessage.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lblMessage.Name = "lblMessage";
            // 
            // RegistPlayer
            // 
            this.AcceptButton = this.btnRequestRegist;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelRegist;
            this.ControlBox = true;
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnRequestRegist);
            this.Controls.Add(this.edtPlayerId);
            this.Controls.Add(this.btnCancelRegist);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.pictureEdit2);
            this.DoubleBuffered = true;
            this.Name = "RegistPlayer";
            this.ShowIcon = false;
            this.SplashImage = ((System.Drawing.Image)(resources.GetObject("$this.SplashImage")));
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegistPlayer_FormClosed);
            this.Load += new System.EventHandler(this.RegistPlayer_Load);
            this.Shown += new System.EventHandler(this.RegistPlayer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPlayerId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.SimpleButton btnCancelRegist;
        private DevExpress.XtraEditors.TextEdit edtPlayerId;
        private DevExpress.XtraEditors.SimpleButton btnRequestRegist;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.LabelControl lblMessage;
    }
}
