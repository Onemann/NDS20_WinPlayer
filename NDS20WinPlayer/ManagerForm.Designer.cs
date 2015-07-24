﻿namespace NDS20WinPlayer
{
    partial class ManagerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerForm));
            this.xtabManager = new DevExpress.XtraTab.XtraTabControl();
            this.xtabpg1PlayStatus = new DevExpress.XtraTab.XtraTabPage();
            this.spltcontCntlManager = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.tlclSchedule = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tlclStartDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.tlclEndDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.xtabpg2Download = new DevExpress.XtraTab.XtraTabPage();
            this.xtabpg3Log = new DevExpress.XtraTab.XtraTabPage();
            this.xtabpg4Setup = new DevExpress.XtraTab.XtraTabPage();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.grdctrFramePlayStatus = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pLinqServerModeSource1 = new DevExpress.Data.PLinq.PLinqServerModeSource();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.xtabManager)).BeginInit();
            this.xtabManager.SuspendLayout();
            this.xtabpg1PlayStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltcontCntlManager)).BeginInit();
            this.spltcontCntlManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdctrFramePlayStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pLinqServerModeSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtabManager
            // 
            this.xtabManager.AppearancePage.Header.Options.UseTextOptions = true;
            this.xtabManager.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.xtabManager.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.Black;
            this.xtabManager.AppearancePage.HeaderActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.xtabManager.AppearancePage.HeaderActive.Options.UseBackColor = true;
            this.xtabManager.AppearancePage.HeaderActive.Options.UseFont = true;
            this.xtabManager.AppearancePage.HeaderActive.Options.UseForeColor = true;
            this.xtabManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtabManager.Location = new System.Drawing.Point(2, 2);
            this.xtabManager.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.xtabManager.Name = "xtabManager";
            this.xtabManager.SelectedTabPage = this.xtabpg1PlayStatus;
            this.xtabManager.Size = new System.Drawing.Size(957, 505);
            this.xtabManager.TabIndex = 0;
            this.xtabManager.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtabpg1PlayStatus,
            this.xtabpg2Download,
            this.xtabpg3Log,
            this.xtabpg4Setup});
            this.xtabManager.TabPageWidth = 100;
            // 
            // xtabpg1PlayStatus
            // 
            this.xtabpg1PlayStatus.Appearance.PageClient.BackColor = System.Drawing.Color.White;
            this.xtabpg1PlayStatus.Appearance.PageClient.BackColor2 = System.Drawing.Color.White;
            this.xtabpg1PlayStatus.Appearance.PageClient.Options.UseBackColor = true;
            this.xtabpg1PlayStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.xtabpg1PlayStatus.Controls.Add(this.spltcontCntlManager);
            this.xtabpg1PlayStatus.Image = ((System.Drawing.Image)(resources.GetObject("xtabpg1PlayStatus.Image")));
            this.xtabpg1PlayStatus.Name = "xtabpg1PlayStatus";
            this.xtabpg1PlayStatus.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.xtabpg1PlayStatus.Size = new System.Drawing.Size(951, 474);
            this.xtabpg1PlayStatus.Text = "재생상태";
            // 
            // spltcontCntlManager
            // 
            this.spltcontCntlManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltcontCntlManager.Location = new System.Drawing.Point(0, 0);
            this.spltcontCntlManager.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.spltcontCntlManager.Name = "spltcontCntlManager";
            this.spltcontCntlManager.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.spltcontCntlManager.Panel1.Controls.Add(this.treeList1);
            this.spltcontCntlManager.Panel1.ShowCaption = true;
            this.spltcontCntlManager.Panel1.Text = "재생 스케줄";
            this.spltcontCntlManager.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.spltcontCntlManager.Panel2.Controls.Add(this.grdctrFramePlayStatus);
            this.spltcontCntlManager.Panel2.ShowCaption = true;
            this.spltcontCntlManager.Panel2.Text = "프레임별 재생 정보";
            this.spltcontCntlManager.Size = new System.Drawing.Size(951, 474);
            this.spltcontCntlManager.SplitterPosition = 280;
            this.spltcontCntlManager.TabIndex = 0;
            this.spltcontCntlManager.Text = "NDS2.0 Manager";
            // 
            // treeList1
            // 
            this.treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeList1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tlclSchedule,
            this.tlclStartDate,
            this.tlclEndDate});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.KeyFieldName = "Count";
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.LookAndFeel.SkinName = "Dark Side";
            this.treeList1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.treeList1.Name = "treeList1";
            this.treeList1.ParentFieldName = "Name";
            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            this.treeList1.Size = new System.Drawing.Size(276, 450);
            this.treeList1.TabIndex = 0;
            // 
            // tlclSchedule
            // 
            this.tlclSchedule.AppearanceHeader.Options.UseTextOptions = true;
            this.tlclSchedule.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlclSchedule.Caption = "스케줄";
            this.tlclSchedule.ColumnEdit = this.repositoryItemTextEdit1;
            this.tlclSchedule.FieldName = "tlclScheduleField";
            this.tlclSchedule.Name = "tlclSchedule";
            this.tlclSchedule.OptionsColumn.AllowEdit = false;
            this.tlclSchedule.Visible = true;
            this.tlclSchedule.VisibleIndex = 0;
            this.tlclSchedule.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // tlclStartDate
            // 
            this.tlclStartDate.AppearanceHeader.Options.UseTextOptions = true;
            this.tlclStartDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlclStartDate.Caption = "시작일";
            this.tlclStartDate.ColumnEdit = this.repositoryItemDateEdit1;
            this.tlclStartDate.FieldName = "tlclStartDateField";
            this.tlclStartDate.Name = "tlclStartDate";
            this.tlclStartDate.OptionsColumn.AllowEdit = false;
            this.tlclStartDate.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.tlclStartDate.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.tlclStartDate.Visible = true;
            this.tlclStartDate.VisibleIndex = 1;
            this.tlclStartDate.Width = 53;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // tlclEndDate
            // 
            this.tlclEndDate.AppearanceHeader.Options.UseTextOptions = true;
            this.tlclEndDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlclEndDate.Caption = "종료일";
            this.tlclEndDate.ColumnEdit = this.repositoryItemDateEdit2;
            this.tlclEndDate.FieldName = "tlclEndDateField";
            this.tlclEndDate.Name = "tlclEndDate";
            this.tlclEndDate.OptionsColumn.AllowEdit = false;
            this.tlclEndDate.Visible = true;
            this.tlclEndDate.VisibleIndex = 2;
            this.tlclEndDate.Width = 54;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // xtabpg2Download
            // 
            this.xtabpg2Download.Image = ((System.Drawing.Image)(resources.GetObject("xtabpg2Download.Image")));
            this.xtabpg2Download.Name = "xtabpg2Download";
            this.xtabpg2Download.Size = new System.Drawing.Size(951, 474);
            this.xtabpg2Download.Text = "다운로드 상태";
            // 
            // xtabpg3Log
            // 
            this.xtabpg3Log.Image = ((System.Drawing.Image)(resources.GetObject("xtabpg3Log.Image")));
            this.xtabpg3Log.Name = "xtabpg3Log";
            this.xtabpg3Log.Size = new System.Drawing.Size(951, 474);
            this.xtabpg3Log.Text = "로그 확인";
            // 
            // xtabpg4Setup
            // 
            this.xtabpg4Setup.Image = ((System.Drawing.Image)(resources.GetObject("xtabpg4Setup.Image")));
            this.xtabpg4Setup.Name = "xtabpg4Setup";
            this.xtabpg4Setup.Size = new System.Drawing.Size(951, 474);
            this.xtabpg4Setup.Text = "설정";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2013";
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.xtabManager);
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(961, 509);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(959, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // grdctrFramePlayStatus
            // 
            this.grdctrFramePlayStatus.DataSource = this.pLinqServerModeSource1;
            this.grdctrFramePlayStatus.Location = new System.Drawing.Point(-3, 0);
            this.grdctrFramePlayStatus.MainView = this.gridView1;
            this.grdctrFramePlayStatus.Name = "grdctrFramePlayStatus";
            this.grdctrFramePlayStatus.Size = new System.Drawing.Size(664, 433);
            this.grdctrFramePlayStatus.TabIndex = 0;
            this.grdctrFramePlayStatus.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName});
            this.gridView1.GridControl = this.grdctrFramePlayStatus;
            this.gridView1.Name = "gridView1";
            // 
            // pLinqServerModeSource1
            // 
            this.pLinqServerModeSource1.DefaultSorting = "IsReadOnly ASC";
            this.pLinqServerModeSource1.ElementType = typeof(System.Net.Json.JsonArrayCollection);
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 528);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NDS2.0 Player";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.xtabManager)).EndInit();
            this.xtabManager.ResumeLayout(false);
            this.xtabpg1PlayStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltcontCntlManager)).EndInit();
            this.spltcontCntlManager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdctrFramePlayStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pLinqServerModeSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtabManager;
        private DevExpress.XtraTab.XtraTabPage xtabpg1PlayStatus;
        private DevExpress.XtraTab.XtraTabPage xtabpg2Download;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.SplitContainerControl spltcontCntlManager;
        private DevExpress.XtraTab.XtraTabPage xtabpg3Log;
        private DevExpress.XtraTab.XtraTabPage xtabpg4Setup;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlclSchedule;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlclStartDate;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlclEndDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraGrid.GridControl grdctrFramePlayStatus;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.Data.PLinq.PLinqServerModeSource pLinqServerModeSource1;
        private DevExpress.XtraGrid.Columns.GridColumn colName;


    }
}