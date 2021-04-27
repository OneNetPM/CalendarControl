using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
namespace LJYControls
{
    public partial class LJYDateTime : UserControl
    {
        public delegate void DisposeDatetime(int  Days);
        public event DisposeDatetime DisposeDay;
        private int SelectDay = 0;
        private Color  Colors;
        public int col =0;
        public int ro = 0;
        Dictionary<int, int> dicToday = new Dictionary<int, int>();


        #region 圆角处理
        public void SetWindowRegion()
        {

            System.Drawing.Drawing2D.GraphicsPath FormPath;

            FormPath = new System.Drawing.Drawing2D.GraphicsPath();

            Rectangle rect = new Rectangle(0, 22, this.Width, this.Height - 22);

            FormPath = GetRoundedRectPath(rect, 15);  

            this.Region = new Region(FormPath);

        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {

            int diameter = radius;

            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            GraphicsPath path = new GraphicsPath();

            // 左上角

            path.AddArc(arcRect, 180, 90);

            // 右上角

            arcRect.X = rect.Right - diameter;

            path.AddArc(arcRect, 270, 90);

            // 右下角

            arcRect.Y = rect.Bottom - diameter;

            path.AddArc(arcRect, 0, 90);

            // 左下角

            arcRect.X = rect.Left;

            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();

            return path;

        }

        protected override void OnResize(System.EventArgs e)
        {

            this.Region = null;

            SetWindowRegion();

        }
        #endregion

        public LJYDateTime()
        { 
            InitializeComponent();

            //修改前景色
            this.GriDay.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#444444");

            DataGridViewCellStyle style = new DataGridViewCellStyle();
 
            foreach (DataGridViewColumn col in this.GriDay.Columns)
            {
                col.HeaderCell.Style = style;
            }

            this.GriDay.EnableHeadersVisualStyles = false;

            GriDay.AllowUserToResizeRows = false;

            // 禁止用户改变列头的高度  
            GriDay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            int vYear = DateTime.Now.Year;
            string vMonth = (DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()).ToString();
            this.LabYearNum.Text = vYear.ToString();

            this.LabMonthNum.Text = vMonth;

            direction dir = direction.none;

            GriDayFresh(dir);
        }

        [Category("年份"), Description("修改年份内容")]
        public string YearTxt
        {
            set { LabYearNum.Text = value; this.Invalidate(); }
            get { return LabYearNum.Text; }
        }
        [Category("月份"), Description("修改月份内容")]
        public string MonthTxt
        {
            set { LabMonthNum.Text = value; this.Invalidate(); }
            get { return LabMonthNum.Text; }
        }
        [Category("背景颜色"), Description("修改背景内容")]
        public Color ColorName
        {
            get
            {
                return this.Colors;
            }
            set
            {
                this.Colors = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 选择的日期
        /// </summary>
        [Description("选择的日期"), Category("自定义属性"), DefaultValue(false)]
        public int Day
        {
            get
            {
                return this.SelectDay;
            }
            set
            {
                this.SelectDay = value;
                base.Invalidate();
            }
        }



     

        /// <summary>
        /// 选择的日期
        /// </summary>
        [Description("设置是否允许过期时间"), Category("自定义属性"), DefaultValue(false)] 
        public bool SetGetExpiryDateTime { set; get; }

        public class Week {
            /// <summary>
            /// 周日
            /// </summary>
            public string Sun { get; set; }  
            /// <summary>
            /// 周一
            /// </summary>
            public string Mon { get; set; }
            /// <summary>
            /// 周二
            /// </summary>
            public string Tue { get; set; }
            /// <summary>
            /// 周三
            /// </summary>
            public string Wed { get; set; }
            /// <summary>
            /// 周四
            /// </summary>
            public string Thu { get; set; }
            /// <summary>
            /// 周五
            /// </summary>
            public string Fri { get; set; }
            /// <summary>
            /// 周六
            /// </summary>
            public string Sta { get; set; }  
        
        }

        //单元格点击事件
        private void GriDay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.MonthTxt = LabMonthNum.Text;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { return; } 
            this.Day = Convert.ToInt32(GriDay.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            int vday = Convert.ToInt32(GriDay.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

          

            if (vday == 0 || (vday < DateTime.Now.Day && Convert.ToInt32(YearTxt) < DateTime.Now.Year)
                      || (vday < DateTime.Now.Day && Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
                      || (vday < DateTime.Now.Day && Convert.ToInt32(MonthTxt) == DateTime.Now.Month && Convert.ToInt32(YearTxt) == DateTime.Now.Year)
                      || Convert.ToInt32(YearTxt) < DateTime.Now.Year || Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
            {
                if (SetGetExpiryDateTime)   //TODO:此处
                {
                    this.GriDay.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#5496ed");
                    this.YearTxt = LabYearNum.Text;
                    if (DisposeDay != null)
                    {
                        DisposeDay.Invoke(vday);
                    }
                }
                else
                {
                    this.GriDay.DefaultCellStyle.SelectionBackColor = Color.White;

                    this.GriDay.ClearSelection();
                     
                    return;
                }

               
            }
            else
            {
                this.GriDay.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#5496ed");
                this.YearTxt = LabYearNum.Text;
                if (DisposeDay != null)
                {
                    DisposeDay.Invoke(vday);
                }
            }
        }

        //左按钮
        private void PtLeft_Click(object sender, EventArgs e)
        {
           
            direction dir = direction.left;
            GriDayFresh(dir);
            this.GriDay.ClearSelection();
        }

        //右按钮
        private void PtRight_Click(object sender, EventArgs e)
        {
            
            direction dir = direction.right;
            GriDayFresh(dir);
            this.GriDay.ClearSelection();
        }
        public void GriDayFresh(direction dir)
        {
            //改变日历数据
            switch (dir)
            {
                case direction.left:
                    if (this.LabMonthNum.Text == "01")
                    {
                        this.LabYearNum.Text = (Convert.ToInt32(this.LabYearNum.Text) - 1).ToString();
                        this.LabMonthNum.Text = "12";
                    }
                    else if (Convert.ToInt32(this.LabMonthNum.Text) > 10)
                    {
                        this.LabMonthNum.Text = (Convert.ToInt32(this.LabMonthNum.Text) - 1).ToString();
                    }
                    else
                    {
                        this.LabMonthNum.Text ="0"+(Convert.ToInt32(this.LabMonthNum.Text) - 1).ToString();
                    }
                    break;
                case direction.right:
                     if (this.LabMonthNum.Text == "12")
                    {
                        this.LabYearNum.Text = (Convert.ToInt32(this.LabYearNum.Text) + 1).ToString();
                        this.LabMonthNum.Text = "01";
                    }
                    else if (Convert.ToInt32(this.LabMonthNum.Text) >= 9)
                    {
                        this.LabMonthNum.Text = (Convert.ToInt32(this.LabMonthNum.Text) + 1).ToString();
                    }
                    else
                    {
                        this.LabMonthNum.Text ="0"+(Convert.ToInt32(this.LabMonthNum.Text) + 1).ToString();
                    }
                    break; 
                default:
                    break;
            }
            //重新绑定日历数据 
            int Day = DateTime.DaysInMonth(Convert.ToInt32(this.LabYearNum.Text), Convert.ToInt32(this.LabMonthNum.Text));
            string strStartDate = this.LabYearNum.Text + "." + this.LabMonthNum.Text + ".1";
            string strEndDate = this.LabYearNum.Text + "." + this.LabMonthNum.Text + "." + Day.ToString();
            Dictionary<int, int> dict = GetGroupWeekByDateRange(strStartDate, strEndDate);
            Dictionary<int, int> dictd = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            dicToday = dictd;
            string DayFirst = CaculateWeekDay(Convert.ToInt32(this.LabYearNum.Text), Convert.ToInt32(this.LabMonthNum.Text),1);
            string DayFirstd = DayFirst;
            //循环绑定数据
            List<Week> Weeks = new List<Week>();
            foreach (KeyValuePair<int, int> kvp in dictd)
            { 
                Week we = new Week();
                if (kvp.Key == 1)
                {
                    #region   第一行数据判断第一天周几
                    switch (DayFirst)
                    {
                        case "Mon":
                            we.Mon = "1";
                            we.Tue = "2";
                            we.Wed = "3";
                            we.Thu = "4";
                            we.Fri = "5";
                            we.Sta = "6";
                            we.Sun = "7";
                            break;
                        case "Tue": 
                            we.Tue = "1";
                            we.Wed = "2";
                            we.Thu = "3";
                            we.Fri = "4";
                            we.Sta = "5";
                            we.Sun = "6";
                            break;
                        case "Wed": 
                            we.Wed = "1";
                            we.Thu = "2";
                            we.Fri = "3";
                            we.Sta = "4";
                            we.Sun = "5";
                            break;
                        case "Thu": 
                            we.Thu = "1";
                            we.Fri = "2";
                            we.Sta = "3";
                            we.Sun = "4";
                            break;
                        case "Fri": 
                            we.Fri = "1";
                            we.Sta = "2";
                            we.Sun = "3";
                            break;
                        case "Sta": 
                            we.Sta = "1";
                            we.Sun = "2";
                            break;
                        case "Sun": we.Sun = "1";
                            break;
                    }
                    #endregion

                    Weeks.Add(we); 
                }
                else {
                    if (kvp.Key <= Day)
                    {
                        we.Mon = kvp.Key.ToString();
                    }
                    if (kvp.Key+1 <= Day)
                    {
                        we.Tue = (Convert.ToInt32(kvp.Key) + 1).ToString();
                    }
                    if (kvp.Key + 2 <= Day)
                    {
                        we.Wed = (Convert.ToInt32(kvp.Key) + 2).ToString();
                    }
                    if (kvp.Key + 3 <= Day)
                    {
                        we.Thu = (Convert.ToInt32(kvp.Key) + 3).ToString();
                    }
                    if (kvp.Key + 4 <= Day)
                    {
                        we.Fri = (Convert.ToInt32(kvp.Key) + 4).ToString();
                    }
                    if (kvp.Key + 5 <= Day)
                    {
                        we.Sta = (Convert.ToInt32(kvp.Key) + 5).ToString();
                    }
                    if (kvp.Key + 6 <= Day)
                    {
                        we.Sun = (Convert.ToInt32(kvp.Key) + 6).ToString();
                    } 
                    Weeks.Add(we);

                }
            }
            this.GriDay.DataSource = Weeks;
            for (int i = 0; i < GriDay.Rows.Count; i++)
            {
                for (int k = 0; k < GriDay.Columns.Count; k++)
                {
                    //如果当前文本小于今天文字变浅色 
                    if ((Convert.ToInt32(GriDay.Rows[i].Cells[k].Value) < DateTime.Now.Day &&
                         Convert.ToInt32(YearTxt) < DateTime.Now.Year)
                        ||
                        (Convert.ToInt32(GriDay.Rows[i].Cells[k].Value) < DateTime.Now.Day &&
                         Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
                        ||
                        (Convert.ToInt32(GriDay.Rows[i].Cells[k].Value) < DateTime.Now.Day &&
                         Convert.ToInt32(MonthTxt) == DateTime.Now.Month &&
                         Convert.ToInt32(YearTxt) == DateTime.Now.Year)
                        || Convert.ToInt32(YearTxt) < DateTime.Now.Year ||
                        Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
                    {
                        if (SetGetExpiryDateTime)  //TODO:此处
                        { 
                            GriDay.Rows[i].Cells[k].Style.SelectionBackColor = Color.CornflowerBlue;
                        }
                        else
                        {
                            GriDay.Rows[i].Cells[k].Style.ForeColor = ColorTranslator.FromHtml("#808080");
                        }

                        
                    }
                    
                    else if (i == 3 && k == 3)
                    { 
                        GriDay.Rows[i].Cells[k].Style.SelectionBackColor = Color.CornflowerBlue;
                    }
                }
            }
        }


        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LJYDateTime_Load(object sender, EventArgs e)
        {
            this.GriDay.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#5496ed");

            //初始化年数据
            this.YearTxt = DateTime.Now.Year.ToString();

            //初始化月数据
            this.MonthTxt = DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString();

            //清除已选择的
            this.GriDay.ClearSelection();

            //获取行数
            for (int i = 0; i < GriDay.Rows.Count; i++)
            {
                //获取列数
                for (int k = 0; k < GriDay.Columns.Count; k++)
                {
                    if (SetGetExpiryDateTime)  //TODO:此处
                    {
                        break;
                    }
                    else
                    {
                        //如果当前文本小于今天文字变浅色
                        if (Convert.ToInt32(GriDay.Rows[i].Cells[k].Value) < DateTime.Now.Day)
                        {
                            GriDay.Rows[i].Cells[k].Style.ForeColor = ColorTranslator.FromHtml("#808080");
                        }
                    }
     
                }
            }
            string dt = DateTime.Today.DayOfWeek.ToString();
            int Cloumn = 0;
            switch (dt)
            {
                case "Monday":
                    Cloumn = 0;
                    break;
                case "Tuesday":
                    Cloumn = 1;
                    break;
                case "Wednesday":
                    Cloumn = 2;
                    break;
                case "Thursday":
                    Cloumn = 3;
                    break;
                case "Friday":
                    Cloumn = 4;
                    break;
                case "Saturday":
                    Cloumn = 5;
                    break;
                case "Sunday":
                    Cloumn = 6;
                    break;
            }
            int vDays = DateTime.Now.Day; 
            for (int index = 0; index < dicToday.Count; index++)
            {
                var item = dicToday.ElementAt(index);
                if (vDays >= item.Key && vDays <= item.Value)
                {
                    vDays = index;
                    continue;
                }
            }
            ro = vDays;
            col = Cloumn;
            if (Cloumn != 6)
            {
                this.GriDay[Cloumn + 1, vDays].Selected = true;
            }
            else
            {
                this.GriDay[Cloumn - 6, vDays].Selected = true;
            } 

        }
        
        public enum direction { 
           none=-1,
           left=0,
           right=1, 
        } 
        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1) m = 13;
            if (m == 2) m = 14;
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7 + 1;
            string weekstr = "";
            switch (week)
            {
                case 1: weekstr = "Mon"; break;
                case 2: weekstr = "Tue"; break;
                case 3: weekstr = "Wed"; break;
                case 4: weekstr = "Thu"; break;
                case 5: weekstr = "Fri"; break;
                case 6: weekstr = "Sta"; break;
                case 7: weekstr = "Sun"; break;
            } 
            return weekstr;
        }
        /// <summary>  
        /// 根据时间范围获取每年每月每周的分组  
        /// </summary>  
        /// <param name="strStartDate">起始时间</param>  
        /// <param name="strEndDate">结束时间</param>  
        /// <returns>返回每周起始结束键值对</returns>  
        public static Dictionary<int, int> GetGroupWeekByDateRange(string strStartDate, string strEndDate)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            DateTime dtStartDate = DateTime.Parse(strStartDate);
            DateTime dtEndDate = DateTime.Parse(strEndDate);

            //同年  
            if (dtStartDate.Year == dtEndDate.Year)
            {
                GetGroupWeekByYear(dict, dtStartDate, dtEndDate);
            }
            //不同年  
            else
            {
                int WhileCount = dtEndDate.Year - dtStartDate.Year;

                //某年一共有多少天  
                int YearDay = DateTime.IsLeapYear(dtStartDate.Year) ? 366 : 365;
                DateTime dtTempStartDate = dtStartDate;

                DateTime dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                //根据时间范围获取每月每周的分组  
                GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                for (int i = 1; i < (WhileCount + 1); i++)
                {
                    //某年某月一共有多少天  
                    YearDay = DateTime.IsLeapYear(dtTempStartDate.Year + 1) ? 366 : 365;
                    dtTempStartDate = DateTime.Parse(DateTime.Parse((dtTempStartDate.Year + 1) + "." + dtTempStartDate.Month + "." + "01").ToString("yyyy.MM.dd"));
                    dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                    //根据时间范围获取每月每周的分组  
                    GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                }
            }

            return dict;
        }  
        /// <summary>  
        /// 根据时间范围(年)获取每月每周的分组  
        /// </summary>  
        /// <param name="dict">每周起始结束键值对</param>  
        /// <param name="strStartDate">起始时间</param>  
        /// <param name="strEndDate">结束时间</param>  
        public static void GetGroupWeekByYear(Dictionary<int, int> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
            //不同月  
            if ((dtEndDate.Month - dtStartDate.Month) >= 1)
            {
                int WhileCount = dtEndDate.Month - dtStartDate.Month;

                //某年某月一共有多少天  
                int MonthDay = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month);
                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtTempStartDate.AddDays(MonthDay - 1);

                //根据时间范围获取每月每周的分组  
                GetGroupWeekByMonth(dict, dtTempStartDate, dtTempEndDate);

                for (int i = 1; i < (WhileCount + 1); i++)
                {
                    //某年某月一共有多少天  
                    MonthDay = DateTime.DaysInMonth(dtTempStartDate.Year, dtTempStartDate.Month + 1);
                    dtTempStartDate = DateTime.Parse(DateTime.Parse(dtTempStartDate.Year + "." + (dtTempStartDate.Month + 1) + "." + "01").ToString("yyyy.MM.dd"));
                    dtTempEndDate = dtTempStartDate.AddDays(MonthDay - 1);

                    //根据时间范围获取每月每周的分组  
                    GetGroupWeekByMonth(dict, dtTempStartDate, dtTempEndDate);

                }
            }
            //同月  
            else
            {
                //根据时间范围获取每月每周的分组  
                GetGroupWeekByMonth(dict, dtStartDate, dtEndDate);
            }
        }
        /// <summary>  
        /// 根据时间范围(月)获取每月每周的分组  
        /// </summary>  
        /// <param name="dict">每周起始结束键值对</param>  
        /// <param name="strStartDate">起始时间</param>  
        /// <param name="strEndDate">结束时间</param>  
        public static void GetGroupWeekByMonth(Dictionary<int, int> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
            //一周  
            if ((dtEndDate.Day - dtStartDate.Day) < 7)
            {
                DayOfWeek day = dtStartDate.DayOfWeek;
                string dayString = day.ToString();

                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtEndDate;
                DateTime dtTempDate = DateTime.Now;
                switch (dayString)
                {
                    case "Monday":
                        dict.Add(dtTempStartDate.Day, dtTempEndDate.Day);
                        break;
                    case "Tuesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+5);
                        break;
                    case "Wednesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+4);
                        break;
                    case "Thursday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+3);
                        break;
                    case "Friday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+2);
                        break;
                    case "Saturday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+1);
                        break;
                    case "Sunday":
                        dtTempDate = dtTempStartDate;
                        break;
                }
                if (!dayString.Equals("Monday"))
                {
                    dict.Add(dtTempStartDate.Day, dtTempDate.Day);
                    dtTempDate = dtTempDate.Date.AddDays(+1);
                    if (DateTime.Compare(dtTempDate, dtEndDate) <= 0)
                    {
                        dict.Add(dtTempDate.Day, dtTempEndDate.Day);
                    }
                }
            }
            //多周  
            else
            {
                DayOfWeek day = dtStartDate.DayOfWeek;
                string dayString = day.ToString();

                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtEndDate;
                DateTime dtTempDate = DateTime.Now;

                #region 起始

                switch (dayString)
                {
                    case "Monday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+6);
                        break;
                    case "Tuesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+5);
                        break;
                    case "Wednesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+4);
                        break;
                    case "Thursday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+3);
                        break;
                    case "Friday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+2);
                        break;
                    case "Saturday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+1);
                        break;
                    case "Sunday":
                        dtTempDate = dtTempStartDate;
                        break;
                }
                dict.Add(dtTempStartDate.Day, dtTempDate.Day);

                dtTempStartDate = dtTempDate.Date.AddDays(+1);
                #endregion

                #region 结束

                day = dtEndDate.DayOfWeek;
                dayString = day.ToString();

                switch (dayString)
                {
                    case "Monday":
                        dtTempDate = dtEndDate;
                        break;
                    case "Tuesday":
                        dtTempDate = dtEndDate.Date.AddDays(-1);
                        break;
                    case "Wednesday":
                        dtTempDate = dtEndDate.Date.AddDays(-2);
                        break;
                    case "Thursday":
                        dtTempDate = dtEndDate.Date.AddDays(-3);
                        break;
                    case "Friday":
                        dtTempDate = dtEndDate.Date.AddDays(-4);
                        break;
                    case "Saturday":
                        dtTempDate = dtEndDate.Date.AddDays(-5);
                        break;
                    case "Sunday":
                        dtTempDate = dtEndDate.Date.AddDays(-6);
                        break;
                }

                dict.Add(dtTempDate.Day, dtEndDate.Day);

                dtTempEndDate = dtTempDate.Date.AddDays(-1);

                #endregion

                int WhileCount = ((dtTempEndDate.Day - dtTempStartDate.Day) / 7);
                if (WhileCount == 0)
                {
                    dict.Add(dtTempStartDate.Day, dtTempEndDate.Day);
                }
                else
                {
                    for (int i = 0; i < (WhileCount + 1); i++)
                    {
                        dtTempDate = dtTempStartDate.Date.AddDays(+6);
                        dict.Add(dtTempStartDate.Day, dtTempDate.Day);
                        dtTempStartDate = dtTempDate.Date.AddDays(+1); ;
                    }
                }
            }
        }

        private void GriDay_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.MonthTxt = LabMonthNum.Text;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { return; }
            this.Day = Convert.ToInt32(GriDay.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            int vday = Convert.ToInt32(GriDay.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            if (vday == 0 || (vday < DateTime.Now.Day && Convert.ToInt32(YearTxt) < DateTime.Now.Year)
                      || (vday < DateTime.Now.Day && Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
                      || (vday < DateTime.Now.Day && Convert.ToInt32(MonthTxt) == DateTime.Now.Month && Convert.ToInt32(YearTxt) == DateTime.Now.Year)
                      || Convert.ToInt32(YearTxt) < DateTime.Now.Year || Convert.ToInt32(MonthTxt) < DateTime.Now.Month)
            {
                this.GriDay.DefaultCellStyle.SelectionBackColor = Color.White;
                this.GriDay.ClearSelection();
                return;
            }
        }

        private void GriDay_MouseDown(object sender, MouseEventArgs e)
        {
            // 通过鼠标按下的位置获取所在行的信息
            var hitTest = this.GriDay.HitTest(e.X, e.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell)
                return;

            // 记下拖动源数据行的索引及已鼠标按下坐标为中心的不会开始拖动的范围
            indexOfItemUnderMouseToDrag = hitTest.RowIndex;
            if (indexOfItemUnderMouseToDrag > -1)
            {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }
        DataTable dataTable = new DataTable();
        // 拖动的源数据行索引
        private int indexOfItemUnderMouseToDrag = -1;
        // 拖动的目标数据行索引
        private int indexOfItemUnderMouseToDrop = -1;
        // 拖动中的鼠标所在位置的当前行索引
        private int indexOfItemUnderMouseOver = -1;
        // 不启用拖放的鼠标范围
        private Rectangle dragBoxFromMouseDown = Rectangle.Empty;

        private void GriDay_MouseMove(object sender, MouseEventArgs e)
        {
            // 不是鼠标左键按下时移动
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            // 如果鼠标在不启用拖动的范围内
            if (dragBoxFromMouseDown == Rectangle.Empty || dragBoxFromMouseDown.Contains(e.X, e.Y))
                return;
            // 如果源数据行索引值不正确
            if (indexOfItemUnderMouseToDrag < 0)
                return;

            // 开始拖动，第一个参数表示要拖动的数据，可以自定义，一般是源数据行
            var row = GriDay.Rows[indexOfItemUnderMouseToDrag];
            DragDropEffects dropEffect = GriDay.DoDragDrop(row, DragDropEffects.All);
            this.GriDay.ClearSelection();
            //拖动过程结束后清除拖动位置行的红线效果
           // OnRowDragOver(-1);
        }
        
    }
}
