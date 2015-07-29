namespace NDS20WinPlayer
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
            this.tlclType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tlclStartDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.tlclEndDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.xtabpg2Download = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.trlstSchedule = new DevExpress.XtraTreeList.TreeList();
            this.tlcScheCatagory = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tlcScheType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tkcScheKey = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tlcScheName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tlcScheStartdate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tlcScheEnddate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemDateEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemDateEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.xtabpg3Log = new DevExpress.XtraTab.XtraTabPage();
            this.xtabpg4Setup = new DevExpress.XtraTab.XtraTabPage();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
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
            this.xtabpg2Download.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trlstSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
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
            this.spltcontCntlManager.Panel2.ShowCaption = true;
            this.spltcontCntlManager.Panel2.Text = "프레임별 재생 정보";
            this.spltcontCntlManager.Size = new System.Drawing.Size(951, 474);
            this.spltcontCntlManager.SplitterPosition = 396;
            this.spltcontCntlManager.TabIndex = 0;
            this.spltcontCntlManager.Text = "NDS2.0 Manager";
            this.spltcontCntlManager.Paint += new System.Windows.Forms.PaintEventHandler(this.spltcontCntlManager_Paint);
            // 
            // treeList1
            // 
            this.treeList1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.treeList1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeList1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tlclSchedule,
            this.tlclType,
            this.tlclStartDate,
            this.tlclEndDate});
            this.treeList1.CustomizationFormBounds = new System.Drawing.Rectangle(478, 476, 206, 175);
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.KeyFieldName = "schedule";
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.LookAndFeel.SkinName = "Dark Side";
            this.treeList1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList1.OptionsView.AutoWidth = false;
            this.treeList1.OptionsView.EnableAppearanceEvenRow = true;
            this.treeList1.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.treeList1.OptionsView.ShowSummaryFooter = true;
            this.treeList1.ParentFieldName = "Name";
            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            this.treeList1.Size = new System.Drawing.Size(392, 450);
            this.treeList1.TabIndex = 0;
            // 
            // tlclSchedule
            // 
            this.tlclSchedule.AppearanceHeader.Options.UseTextOptions = true;
            this.tlclSchedule.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlclSchedule.Caption = "스케줄";
            this.tlclSchedule.ColumnEdit = this.repositoryItemTextEdit1;
            this.tlclSchedule.FieldName = "tlclScheduleField";
            this.tlclSchedule.MinWidth = 52;
            this.tlclSchedule.Name = "tlclSchedule";
            this.tlclSchedule.OptionsColumn.AllowEdit = false;
            this.tlclSchedule.OptionsColumn.AllowFocus = false;
            this.tlclSchedule.OptionsColumn.ReadOnly = true;
            this.tlclSchedule.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.tlclSchedule.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Count;
            this.tlclSchedule.SummaryFooterStrFormat = "{0}건";
            this.tlclSchedule.Visible = true;
            this.tlclSchedule.VisibleIndex = 0;
            this.tlclSchedule.Width = 162;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // tlclType
            // 
            this.tlclType.Caption = "타입";
            this.tlclType.FieldName = "tlclTypeField";
            this.tlclType.Name = "tlclType";
            this.tlclType.OptionsColumn.AllowEdit = false;
            this.tlclType.OptionsColumn.AllowFocus = false;
            this.tlclType.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.tlclType.Visible = true;
            this.tlclType.VisibleIndex = 1;
            this.tlclType.Width = 55;
            // 
            // tlclStartDate
            // 
            this.tlclStartDate.AllowIncrementalSearch = false;
            this.tlclStartDate.AppearanceHeader.Options.UseTextOptions = true;
            this.tlclStartDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlclStartDate.Caption = "시작일";
            this.tlclStartDate.ColumnEdit = this.repositoryItemDateEdit1;
            this.tlclStartDate.FieldName = "tlclStartDateField";
            this.tlclStartDate.Name = "tlclStartDate";
            this.tlclStartDate.OptionsColumn.AllowEdit = false;
            this.tlclStartDate.OptionsColumn.AllowFocus = false;
            this.tlclStartDate.OptionsColumn.ReadOnly = true;
            this.tlclStartDate.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.tlclStartDate.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.tlclStartDate.Visible = true;
            this.tlclStartDate.VisibleIndex = 2;
            this.tlclStartDate.Width = 86;
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
            this.tlclEndDate.OptionsColumn.AllowFocus = false;
            this.tlclEndDate.OptionsColumn.ReadOnly = true;
            this.tlclEndDate.Visible = true;
            this.tlclEndDate.VisibleIndex = 3;
            this.tlclEndDate.Width = 73;
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
            this.xtabpg2Download.Controls.Add(this.splitContainerControl1);
            this.xtabpg2Download.Image = ((System.Drawing.Image)(resources.GetObject("xtabpg2Download.Image")));
            this.xtabpg2Download.Name = "xtabpg2Download";
            this.xtabpg2Download.Size = new System.Drawing.Size(951, 474);
            this.xtabpg2Download.Text = "스케줄";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.splitContainerControl1.Panel1.Controls.Add(this.trlstSchedule);
            this.splitContainerControl1.Panel1.ShowCaption = true;
            this.splitContainerControl1.Panel1.Text = "재생 스케줄";
            this.splitContainerControl1.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.splitContainerControl1.Panel2.Controls.Add(this.memoEdit1);
            this.splitContainerControl1.Panel2.ShowCaption = true;
            this.splitContainerControl1.Panel2.Text = "프레임별 재생 정보";
            this.splitContainerControl1.Size = new System.Drawing.Size(951, 474);
            this.splitContainerControl1.SplitterPosition = 471;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "NDS2.0 Manager";
            // 
            // trlstSchedule
            // 
            this.trlstSchedule.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.trlstSchedule.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trlstSchedule.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.trlstSchedule.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tlcScheCatagory,
            this.tlcScheType,
            this.tkcScheKey,
            this.tlcScheName,
            this.tlcScheStartdate,
            this.tlcScheEnddate});
            this.trlstSchedule.CustomizationFormBounds = new System.Drawing.Rectangle(478, 476, 206, 175);
            this.trlstSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trlstSchedule.KeyFieldName = "schedule";
            this.trlstSchedule.Location = new System.Drawing.Point(0, 0);
            this.trlstSchedule.LookAndFeel.SkinName = "Dark Side";
            this.trlstSchedule.LookAndFeel.UseDefaultLookAndFeel = false;
            this.trlstSchedule.Name = "trlstSchedule";
            this.trlstSchedule.OptionsBehavior.PopulateServiceColumns = true;
            this.trlstSchedule.OptionsView.AutoWidth = false;
            this.trlstSchedule.OptionsView.EnableAppearanceEvenRow = true;
            this.trlstSchedule.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.trlstSchedule.OptionsView.ShowSummaryFooter = true;
            this.trlstSchedule.ParentFieldName = "Name";
            this.trlstSchedule.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit2,
            this.repositoryItemDateEdit3,
            this.repositoryItemDateEdit4});
            this.trlstSchedule.Size = new System.Drawing.Size(467, 450);
            this.trlstSchedule.TabIndex = 0;
            this.trlstSchedule.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.trlstSchedule_FocusedNodeChanged);
            this.trlstSchedule.Load += new System.EventHandler(this.trlstSchedule_Load);
            // 
            // tlcScheCatagory
            // 
            this.tlcScheCatagory.Caption = "분류";
            this.tlcScheCatagory.FieldName = "scheCatagory";
            this.tlcScheCatagory.Name = "tlcScheCatagory";
            this.tlcScheCatagory.OptionsColumn.AllowEdit = false;
            this.tlcScheCatagory.OptionsColumn.AllowFocus = false;
            this.tlcScheCatagory.OptionsColumn.ReadOnly = true;
            this.tlcScheCatagory.Visible = true;
            this.tlcScheCatagory.VisibleIndex = 0;
            this.tlcScheCatagory.Width = 50;
            // 
            // tlcScheType
            // 
            this.tlcScheType.Caption = "유형";
            this.tlcScheType.FieldName = "scheType";
            this.tlcScheType.Name = "tlcScheType";
            this.tlcScheType.OptionsColumn.AllowEdit = false;
            this.tlcScheType.OptionsColumn.AllowFocus = false;
            this.tlcScheType.OptionsColumn.ReadOnly = true;
            this.tlcScheType.Visible = true;
            this.tlcScheType.VisibleIndex = 1;
            this.tlcScheType.Width = 50;
            // 
            // tkcScheKey
            // 
            this.tkcScheKey.Caption = "스케줄키";
            this.tkcScheKey.FieldName = "ctscKey";
            this.tkcScheKey.Name = "tkcScheKey";
            this.tkcScheKey.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.tkcScheKey.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.tkcScheKey.Visible = true;
            this.tkcScheKey.VisibleIndex = 2;
            this.tkcScheKey.Width = 60;
            // 
            // tlcScheName
            // 
            this.tlcScheName.AppearanceHeader.Options.UseTextOptions = true;
            this.tlcScheName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlcScheName.Caption = "스케줄";
            this.tlcScheName.ColumnEdit = this.repositoryItemTextEdit2;
            this.tlcScheName.FieldName = "ctscName";
            this.tlcScheName.MinWidth = 52;
            this.tlcScheName.Name = "tlcScheName";
            this.tlcScheName.OptionsColumn.AllowEdit = false;
            this.tlcScheName.OptionsColumn.AllowFocus = false;
            this.tlcScheName.OptionsColumn.ReadOnly = true;
            this.tlcScheName.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Count;
            this.tlcScheName.SummaryFooterStrFormat = "{0}건";
            this.tlcScheName.Visible = true;
            this.tlcScheName.VisibleIndex = 3;
            this.tlcScheName.Width = 162;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // tlcScheStartdate
            // 
            this.tlcScheStartdate.AllowIncrementalSearch = false;
            this.tlcScheStartdate.AppearanceHeader.Options.UseTextOptions = true;
            this.tlcScheStartdate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlcScheStartdate.Caption = "시작일";
            this.tlcScheStartdate.FieldName = "ctscStartdate";
            this.tlcScheStartdate.Name = "tlcScheStartdate";
            this.tlcScheStartdate.OptionsColumn.AllowEdit = false;
            this.tlcScheStartdate.OptionsColumn.AllowFocus = false;
            this.tlcScheStartdate.OptionsColumn.ReadOnly = true;
            this.tlcScheStartdate.Visible = true;
            this.tlcScheStartdate.VisibleIndex = 4;
            this.tlcScheStartdate.Width = 70;
            // 
            // tlcScheEnddate
            // 
            this.tlcScheEnddate.AppearanceHeader.Options.UseTextOptions = true;
            this.tlcScheEnddate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlcScheEnddate.Caption = "종료일";
            this.tlcScheEnddate.FieldName = "ctscEnddate";
            this.tlcScheEnddate.Name = "tlcScheEnddate";
            this.tlcScheEnddate.OptionsColumn.AllowEdit = false;
            this.tlcScheEnddate.OptionsColumn.AllowFocus = false;
            this.tlcScheEnddate.OptionsColumn.ReadOnly = true;
            this.tlcScheEnddate.Visible = true;
            this.tlcScheEnddate.VisibleIndex = 5;
            this.tlcScheEnddate.Width = 70;
            // 
            // repositoryItemDateEdit3
            // 
            this.repositoryItemDateEdit3.AutoHeight = false;
            this.repositoryItemDateEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit3.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit3.Name = "repositoryItemDateEdit3";
            // 
            // repositoryItemDateEdit4
            // 
            this.repositoryItemDateEdit4.AutoHeight = false;
            this.repositoryItemDateEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit4.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit4.Name = "repositoryItemDateEdit4";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.EditValue = "";
            this.memoEdit1.Location = new System.Drawing.Point(0, 0);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(471, 450);
            this.memoEdit1.TabIndex = 0;
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManagerForm_KeyDown);
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
            this.xtabpg2Download.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trlstSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
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
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlclType;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTreeList.TreeList trlstSchedule;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcScheCatagory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcScheType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tkcScheKey;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcScheName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcScheStartdate;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcScheEnddate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit4;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;


    }
}