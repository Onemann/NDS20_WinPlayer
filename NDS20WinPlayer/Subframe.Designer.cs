namespace NDS20WinPlayer
{
    partial class Subframe
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
            this.pnlPlayerBack = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlPlayerBack
            // 
            this.pnlPlayerBack.BackColor = System.Drawing.Color.Black;
            this.pnlPlayerBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayerBack.Location = new System.Drawing.Point(0, 0);
            this.pnlPlayerBack.Name = "pnlPlayerBack";
            this.pnlPlayerBack.Size = new System.Drawing.Size(562, 332);
            this.pnlPlayerBack.TabIndex = 3;
            // 
            // Subframe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(562, 332);
            this.ControlBox = false;
            this.Controls.Add(this.pnlPlayerBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Subframe";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Subframe";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPlayerBack;

    }
}