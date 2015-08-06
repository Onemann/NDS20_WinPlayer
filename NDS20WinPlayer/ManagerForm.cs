﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net.Json;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DevExpress.XtraGrid;

namespace NDS20WinPlayer
{
    public partial class ManagerForm : DevExpress.XtraEditors.XtraForm
    {
        public ManagerForm()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            //JsonArrayCollection arrCol = new JsonArrayCollection();


            //treeList1.DataSource = new System.Collections.Generic.List<System.Net.Json.JsonArrayCollection>();

            //List<scheduleclass> shclist = new List<scheduleclass>();

            //shclist.Add(new scheduleclass
            //    {
            //        tlclScheduleField = "schedule1"
            //    });


            string jsonSchedule = "[" +
                "{'tlclScheduleField':'sc2', 'tlclTypeField':'기본','tlclStartDateField':'2015-07-28T12:00Z', 'tlclEndDateField':'2015-08-30T12:00:00Z'}," +
                "{'tlclScheduleField':'sc3', 'tlclTypeField':'기본', 'tlclStartDateField':'2015-07-28T12:00Z', 'tlclEndDateField':'2015-08-30T12:00:00Z'}," +
                "{'tlclScheduleField':'sc4', 'tlclTypeField':'이벤트', 'tlclStartDateField':'2015-07-28T12:00Z', 'tlclEndDateField':'2015-08-30T12:00:00Z'}" +
                "]";

            scheduleclass[] shclist = JsonConvert.DeserializeObject<scheduleclass[]>(jsonSchedule, new IsoDateTimeConverter());

            treeList1.DataSource = shclist;

            /*
            arSchedule.Add(
                "{" +
                " \"tlclSchedule\": \"A.avi\"," +
                " \"xPos\": 400," +
                " \"yPos\": 100," +
                " \"width\": 700," +
                " \"height\": 396," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.avi\"," +
                " \"mute\": true " +
                "}"
                );

            arSchedule.Add(
                "{" +
                " \"tlclSchedule\": \"A.avi\"," +
                " \"xPos\": 400," +
                " \"yPos\": 100," +
                " \"width\": 700," +
                " \"height\": 396," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.avi\"," +
                " \"mute\": true " +
                "}"
                );
             */
            
            //pLinqServerModeSource1.Source = (System.Collections.Generic.IEnumerable<System.Net.Json.JsonArrayCollection>)null /* TODO: Assign the real System.Collections.Generic.IEnumerable source here.*/;
// This line of code is generated by Data Source Configuration Wizard
//treeList1.DataSource = new System.Collections.Generic.List<System.Net.Json.JsonArrayCollection>();
        }

        private void ManagerForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;

            }
        }

        //스케줄 파일에서 Json 스케줄 텍스트를 읽어 grid에 넣기
        private void AssignScheduleFileToTreeList()
        {
            List<clssSchedule> dataSource = new List<clssSchedule>();

            clssSchedule[] scheFileList;
            //clssScheduleFileList scheFileOneRecord;
            DirectoryInfo dirIfo = null;

            dirIfo = new DirectoryInfo(AppInfoStrc.DirOfSchedule);
            if (!dirIfo.Exists) dirIfo.Create();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(AppInfoStrc.DirOfSchedule);

            string scheFullPath = "";
            string scheduleInfoJson = "";
            string scheType = "";

            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                scheFullPath = f.FullName;

                #region 파일 JSON text 읽어서 grid에 채우기
                scheduleInfoJson = System.IO.File.ReadAllText(@scheFullPath);

                scheFileList = JsonConvert.DeserializeObject<clssSchedule[]>(scheduleInfoJson, new IsoDateTimeConverter());

                #region 파일에 포함된 Json 배열의 스케줄을 Datasource에 추가하기
                foreach (clssSchedule scheFileOneRecord in scheFileList)
                {
                    #region 스케줄 코드에 따라 분류와 종류 입력

                    scheType = scheFileOneRecord.scheType;

                    switch (scheType)
                    {
                        case "01":
                            scheFileOneRecord.scheCategory = "일반";
                            scheFileOneRecord.scheKind = "기본";
                            break;
                        case "02":
                            scheFileOneRecord.scheCategory = "일반";
                            scheFileOneRecord.scheKind = "이벤트";
                            break;
                        case "03":
                            scheFileOneRecord.scheCategory = "동기화";
                            scheFileOneRecord.scheKind = "기본";
                            break;
                        case "04":
                            scheFileOneRecord.scheCategory = "동기화";
                            scheFileOneRecord.scheKind = "이벤트";
                            break;
                        case "05":
                            scheFileOneRecord.scheCategory = "사내방송";
                            scheFileOneRecord.scheKind = "기본";
                            break;
                        case "06":
                            scheFileOneRecord.scheCategory = "사내방송";
                            scheFileOneRecord.scheKind = "이벤트";
                            break;
                    }
                    #endregion

                    // 파일을 읽기 위해 파일명을 저장함
                    scheFileOneRecord.scheFileName = f.Name;

                    dataSource.Add(scheFileOneRecord);
                }
                #endregion
            }
            scheFileList = null;

            trlstSchedule.DataSource = dataSource;
            
            #endregion
        }

        // 로그 폴더에 있는 모든 로그파일 이름을 Grid에 넣기
        private void AssignLogFileToTreeList()
        {
            List<clssLogFileList> dataSource = new List<clssLogFileList>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(AppInfoStrc.DirOfLog);

            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                clssLogFileList item = new clssLogFileList();
                item.logFileName = f.Name;
                dataSource.Add(item);
            }

            trlstLogFile.DataSource = dataSource;
            trlstLogFile.MoveFirst();

        }

        private void trlstSchedule_Load(object sender, EventArgs e)
        {
            AssignScheduleFileToTreeList();
        }

        private void trlstSchedule_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

            string jsonScheduleFile = e.Node.GetDisplayText("scheFileName");
            string scheduleName = e.Node.GetDisplayText("ctscName");

            jsonScheduleToContentsGrid(jsonScheduleFile, scheduleName);

            //MessageBox.Show(scheduleFilePath);
            
        }

        
        #region Read JSON schedule files and fill in the contents grid
        private void jsonScheduleToContentsGrid(string jsonScheduleFile, string scheduleName)
        {
            if (jsonScheduleFile == "")
                return;

            try
            {
                string scheduleFilePath = AppInfoStrc.DirOfSchedule + "\\" + jsonScheduleFile;
                string scheduleText = System.IO.File.ReadAllText(@scheduleFilePath);

                dynamic dynSchedule = JsonConvert.DeserializeObject(scheduleText);

                string cn = dynSchedule[0].ctscName;
                string contentsText = "";

                for (int idx = 0; idx < dynSchedule.Count; idx++)
                {
                    if (dynSchedule[idx].ctscName == scheduleName)
                    {
                        contentsText = dynSchedule[idx].Contents.Base;
                        break;
                    }
                }

                return;

                string startContentsDelemeter = "\"Contents\":[";
                bgrdvContents.Bands[3].Columns.Clear();

                if (scheduleText.IndexOf(startContentsDelemeter) > 0) // if some contents were included in this schedule
                {
                    int posStartContetnsDelemeter = scheduleText.IndexOf(startContentsDelemeter) + startContentsDelemeter.Length - 1;
                    int posEndOfContentsDelemeter = scheduleText.IndexOf("]");
                    int contentsLength = posEndOfContentsDelemeter - posStartContetnsDelemeter;
                    contentsText = scheduleText.Substring(posStartContetnsDelemeter, contentsLength + 1);

                    bgrdvContents.Bands[bgrdvContents.Bands.IndexOf(grdbndSector)].Visible = false;


                    #region 구간 밴드 동적 생성
                    List<DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn> lstsector = new List<DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn>();

                    for (int idx = 0; idx < 10; idx++)
                    {
                        lstsector.Add(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn());
                        lstsector[idx].Caption = (idx + 1).ToString();
                        lstsector[idx].OptionsColumn.AllowEdit = false;
                        lstsector[idx].OptionsColumn.AllowFocus = false;
                        lstsector[idx].OptionsColumn.ReadOnly = true;
                        lstsector[idx].ColumnEdit = repositoryItemCheckEdit1;
                        //lstsector[idx].FilterMode = ColumnFilterMode.DisplayText;
                        lstsector[idx].Width = 25;
                        lstsector[idx].Visible = true;
                        lstsector[idx].FieldName = "sector" + (idx + 1).ToString();

                        bgrdvContents.Bands[bgrdvContents.Bands.IndexOf(grdbndSector)].Columns.Add(lstsector[idx]);

                    }
                    bgrdvContents.Bands[bgrdvContents.Bands.IndexOf(grdbndSector)].Visible = true;

                    for (int idx = 0; idx < 10; idx++)
                    {
                        lstsector[idx].Width = 25;
                    }

                    lstsector.Clear();
                    lstsector = null;
                    #endregion

                }


                clssContents[] cntsList = JsonConvert.DeserializeObject<clssContents[]>(contentsText, new IsoDateTimeConverter());

                grdContents.DataSource = cntsList;

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void trlstLogFile_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            string logFileName = e.Node.GetDisplayText("logFileName");
            string logFilePath = AppInfoStrc.DirOfLog + "\\" + logFileName;
            string logTextJson = "[" + System.IO.File.ReadAllText(@logFilePath) + "]";

            clssLogList[] logList = JsonConvert.DeserializeObject<clssLogList[]>(logTextJson, new IsoDateTimeConverter());

            grdctrlLog.DataSource = logList;
        }

        private void trlstLogFile_Load(object sender, EventArgs e)
        {
            AssignLogFileToTreeList();
        }

        private void grdctrlLog_Load(object sender, EventArgs e)
        {
            grdvLog.GroupPanelText = "그룹을 지으시려면 컬럼 해더를 여기로 드래그하시요";
        }

        private void grdContents_Load(object sender, EventArgs e)
        {
            bgrdvContents.GroupPanelText = "그룹을 지으시려면 컬럼 해더를 여기로 드래그하시요";
        }

        private void grdContents_Click(object sender, EventArgs e)
        {

        }

        private void bgrdvContents_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            /*
                    object cellValue = Convert.ToBoolean(true);
                    this.bgrdvContents.Columns[7].View.SetRowCellValue(e.RowHandle, "sector1", cellValue);
      //      DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bgc as sender;
                    this.bgrdvContents.RefreshData();

            */
        }

        private void bgrdvContents_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            /*
            try
            {
                if (e.Column.AbsoluteIndex == 7 && this.bgrdvContents.FocusedRowHandle >= 0)
                {
                    
                    object cellValue = Convert.ToBoolean(true);
                    //this.bgrdvContents.Columns.View.SetRowCellValue(this.bgrdvContents.FocusedRowHandle, "sector1", cellValue);
                    //e.Column.View.SetRowCellValue(,"sector1", cellValue);
                    
                    //e.Value = cellValue;
                    e.DisplayText = "true";
                    e.Column.Width = 40;
                    //e.Column.View.SetRowCellValue(1, "sector1", true);
                }
            }
            catch (Exception ex)
            {
                //Handle exception here
            }
            */

        }
        
    }
}