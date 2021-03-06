﻿namespace NDS20WinPlayer
{
    partial class NDSMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            
            if (TmrGatherPcInfo != null) TmrGatherPcInfo.Dispose();     // Stop timer
            if (_tmrSeverConnection != null) _tmrSeverConnection.Dispose(); // Stop Timer

            if (_pCpu != null) _pCpu.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            LogFile.ThreadWriteLog("====================NDS2.0 Player Closed!!====================", LogType.LOG_INFO);

        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new DevExpress.XtraEditors.PanelControl();
            this.pnlBottom = new DevExpress.XtraEditors.PanelControl();
            this.lblServerConnectionStatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Appearance.BackColor = System.Drawing.Color.Black;
            this.pnlHeader.Appearance.BackColor2 = System.Drawing.Color.White;
            this.pnlHeader.Appearance.BorderColor = System.Drawing.Color.White;
            this.pnlHeader.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlHeader.Appearance.Options.UseBackColor = true;
            this.pnlHeader.Appearance.Options.UseBorderColor = true;
            this.pnlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlHeader.ContentImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlHeader.Size = new System.Drawing.Size(1024, 52);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Appearance.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Appearance.BackColor2 = System.Drawing.Color.Black;
            this.pnlBottom.Appearance.BorderColor = System.Drawing.Color.Black;
            this.pnlBottom.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlBottom.Appearance.Options.UseBackColor = true;
            this.pnlBottom.Appearance.Options.UseBorderColor = true;
            this.pnlBottom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlBottom.ContentImageAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 729);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlBottom.Size = new System.Drawing.Size(1024, 39);
            this.pnlBottom.TabIndex = 1;
            // 
            // lblServerConnectionStatus
            // 
            this.lblServerConnectionStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblServerConnectionStatus.Location = new System.Drawing.Point(373, 309);
            this.lblServerConnectionStatus.Name = "lblServerConnectionStatus";
            this.lblServerConnectionStatus.Size = new System.Drawing.Size(282, 25);
            this.lblServerConnectionStatus.TabIndex = 2;
            // 
            // NDSMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::NDS20WinPlayer.Properties.Resources.Logo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.lblServerConnectionStatus);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "NDSMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "NDS20 Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NDSMain_FormClosing);
            this.Load += new System.EventHandler(this.NDSMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NDSMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlHeader;
        private DevExpress.XtraEditors.PanelControl pnlBottom;
        private DevExpress.XtraEditors.LabelControl lblServerConnectionStatus;



    }
}

