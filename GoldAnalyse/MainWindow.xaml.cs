using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Globalization;

using System.Threading;
using System.Windows.Threading;

using System.Xml;

using System.Data;

using System.Windows.Controls.DataVisualization.Charting;

using GoldAnalyse.GoldPrice;

using GoldAnalyse.Bean;
using GoldAnalyse.PyShell;
using GoldAnalyse.GoldImpl;
using GoldAnalyse.NeuralNetworkImpl;

namespace GoldAnalyse
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //定义定时器
        DispatcherTimer tm = new DispatcherTimer();
        //定义线程
        Thread nThread;


        public List<GoldInfo> Data { get; set; }
        string goldName;
        public GoldInfo curr_goldinfo = new GoldInfo();
        public AllIndex all_index = new AllIndex();
        public Spider spider = new Spider();
        public DataImpl data_impl = new DataImpl();
        public NeuralNetWork NNM = new NeuralNetWork();

        public DataTable dt_Record;
        private static Boolean signal_lock;
        private static String signal_date;

        private DataTable dt_gold_new = new DataTable();  //美元/盎司 买入价
        private DataTable dt_gold_new_sell = new DataTable(); //美元/盎司 卖出价

        static DateTime start = new DateTime();

        public MainWindow()
        {
            InitializeComponent();


            //初始化记录集dt_gold_new
            dt_gold_new.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt_gold_new.Columns.Add(new DataColumn("Value", typeof(double)));

            //初始化记录集dt_gold_new
            dt_gold_new_sell.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt_gold_new_sell.Columns.Add(new DataColumn("Value", typeof(double)));

            //初始化nThread
            nThread = new Thread(new ThreadStart(update_Timer));
            nThread.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化启动时间 start
            start = DateTime.Now;

            this.DataContext = this;
            stockChart.Charts[0].Collapse();

            signal_lock = false;

            LoadData("..//..//Data//data2.txt");
            stockSet1.ItemsSource = Data;






            //初始化Series10
            LineSeries10.DependentRangeAxis = new LinearAxis
            {
                Maximum = 2000,
                Minimum = 1200,
                Interval = 100,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            DateTime min = start;
            DateTime max = start.AddHours(1);
            LineSeries10.IndependentAxis = new DateTimeAxis
            {
                Maximum = max,
                Minimum = min,
                IntervalType = DateTimeIntervalType.Minutes,
                Interval = 5,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Time"
            };

            //初始化Series11,注意！！一定要用LineSeries10来赋值，否则是2个坐标系
            LineSeries11.DependentRangeAxis = LineSeries10.DependentRangeAxis;
            LineSeries11.IndependentAxis = LineSeries10.IndependentAxis;

        }

        /// <summary>
        /// 初始化dt_Record
        /// </summary>
        private void Initialize_dt_Record()
        {

        }



        private List<GoldInfo> LoadGoldInfo(string fileName)
        {
            using (Stream resourceStream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(resourceStream, Encoding.GetEncoding("GB2312")))
                {
                    //读每一行
                    var strings = reader.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //获取股票名称
                    goldName = strings[0].Replace("\r", "");

                    var res = new List<GoldInfo>(strings.Length - 2);

                    //第一行是股票名称, 第二行是类型名称, 第3行才是股票数据
                    for (int i = 2; i < strings.Length; i++)
                    {
                        string line = strings[i];
                        string[] subLines = line.Split('\t');

                        DateTime date = DateTime.Parse(subLines[0]);
                        Double open = Double.Parse(subLines[1]);
                        Double high = Double.Parse(subLines[2]);
                        Double low = Double.Parse(subLines[3]);
                        Double close = Double.Parse(subLines[4]);
                        Double volumn = Double.Parse(subLines[5]);
//                        Double middle = Double.Parse(subLines[6]);

                        res.Add(
                            new GoldInfo
                            {
                                date = date,
                                open = open,
                                high = high,
                                low = low,
                                close = close,
                                volume = volumn,
//                                MA_5 = middle
                            });
                    }
                    return res;
                }
            }
        }

        private void LoadData(string path)
        {
            Data = LoadGoldInfo(path);
            stockChart.Charts[1].Graphs[0].Title = goldName;
        }



        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this.tabControl1) this.DragMove();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this) this.DragMove();
        }

        private void Chart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void stockChart_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            curr_goldinfo.date = stockChart.Charts[1].Graphs[0].CurrentDataItem.Date;//获取日期
            curr_goldinfo.open = stockChart.Charts[1].Graphs[0].CurrentDataItem.Open;//获取开盘价
            curr_goldinfo.high = stockChart.Charts[1].Graphs[0].CurrentDataItem.High;//获取最高价
            curr_goldinfo.low = stockChart.Charts[1].Graphs[0].CurrentDataItem.Low;//获取最低价
            curr_goldinfo.close = stockChart.Charts[1].Graphs[0].CurrentDataItem.Close;

            signal_date = stockChart.Charts[1].Graphs[0].CurrentDataItem.Date.ToShortDateString();

            label2.Content = curr_goldinfo.high;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = curr_goldinfo.high;

            RecordEdit re = new RecordEdit(curr_goldinfo.date.ToShortDateString(),curr_goldinfo.open,curr_goldinfo.high,curr_goldinfo.low,curr_goldinfo.close);
            re.Show();

        }

        private void stockChart_MouseMove(object sender, MouseEventArgs e)
        {
           /* label1.Content = curr_goldinfo.high;
            if (curr_goldinfo.high == 0)
            {
                label1.Content = "abc";
            } 
            else
            {
                label1.Content = curr_goldinfo.high;
            }*/


            if (stockChart.Charts[1].Graphs[0].CurrentDataItem != null)
            {
                label1.Content = stockChart.Charts[1].Graphs[0].CurrentDataItem.High;

                if (signal_lock == false)
                {
                    int start_a = System.Environment.TickCount;
                    Find_Record(stockChart.Charts[1].Graphs[0].CurrentDataItem.Date.ToShortDateString());
                    int end_b = System.Environment.TickCount - start_a;
                }
                else
                {
                    Find_Record(signal_date);
                }

            } 
        }

        /// <summary>
        /// Find the record in the XML file.
        /// According to the Date Time
        /// </summary>
        private void Find_Record(String date)
        {
            //定义myReader
            XmlTextReader myReader;

            //创建记录集dt_grid
            DataTable dt_grid = new DataTable();
            //初始化记录集字段属性 Date,MID,Title,Content
            dt_grid.Columns.Add(new DataColumn("Date", typeof(String)));
            dt_grid.Columns.Add(new DataColumn("MID", typeof(long)));
            dt_grid.Columns.Add(new DataColumn("Title", typeof(String)));
            dt_grid.Columns.Add(new DataColumn("Content", typeof(String)));

            //打开文件
            myReader = new XmlTextReader("..//..//Record//Record.xml");

            //读取功能
            while (myReader.Read())
            {
                if (myReader.Name == "Matter" && myReader.NodeType == XmlNodeType.Element)
                {
                    //定义新的row
                    DataRow row = dt_grid.NewRow();
                    while (myReader.Read())
                    {
                        String date2;
                        myReader.MoveToContent();
                        date2 = myReader.ReadString();
                        if (myReader.Name == "Matter") break;
                        if (myReader.Name != "Date" || myReader.Name =="") continue;
                        if (date2 == date)
                        {
                            //赋值给Date
                            row["Date"] = date2;
                            myReader.Read();  //这一次是中间的空格
                            myReader.Read();  //这一次是MID的节点
                            row["MID"] = Convert.ToInt64(myReader.ReadString());
                            myReader.Read();
                            myReader.Read();
                            row["Title"] = myReader.ReadString();
                            myReader.Read();
                            myReader.Read();
                            row["Content"] = myReader.ReadString();
                            dt_grid.Rows.Add(row);
                        }
                        /*
                        if (myReader.NodeType != XmlNodeType.Element) continue;
                        switch (myReader.Name)
                        {
                            case "MID":
                                row["MID"] = Convert.ToInt64(myReader.ReadString());
                                break;
                            case "Date":
                                row["Date"] = myReader.ReadString();
                                break;
                            case "Title":
                                row["Title"] = Convert.ToString(myReader.ReadString());
                                break;
                            case "Content":
                                row["Content"] = Convert.ToString(myReader.ReadString());
                                break;
                            default:
                                break;
                        }
                        */
                    }

                } 
            }
            //和dataGrid_Record绑定
            dataGrid_Record.ItemsSource = dt_grid.DefaultView;
            //关闭myReader,释放资源
            myReader.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {



            DataTable dt = new DataTable("Sold");
            dt.Columns.Add(new DataColumn("X", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Y", typeof(int)));

            DataTable dt_Gold = data_impl.Table_Get("..//..//Data2012//黄金2012.txt",2000,0);
            DataTable dt_DJI = data_impl.Table_Get("..//..//Data2012//标普指数5002012.txt", 1500,0.2);
            DataTable dt_CCMP = data_impl.Table_Get("..//..//Data2012//纳斯达克2012.txt", 3200,0.4);
            DataTable dt_SCIN = data_impl.Table_Get("..//..//Data2012//上证指数2012.txt", 2500,0.5);
            DataTable dt_INX = data_impl.Table_Get("..//..//Data2012//标普指数5002012.txt", 1500,0.6);
 

            LineSeries1.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            DateTime max = Convert.ToDateTime("2012-12-31");
            DateTime min = Convert.ToDateTime("2012-01-01");
            LineSeries1.IndependentAxis = new DateTimeAxis
            {
                Maximum = max.AddDays(1),
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Day"
            };

            /*
            LineSeries2.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                //Orientation = AxisOrientation.Y,
                //Location = AxisLocation.Left,
                //Title = "Value"
            };
            LineSeries2.IndependentAxis = new DateTimeAxis
            {
                Maximum = max.AddDays(1),
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                //Orientation = AxisOrientation.X,
                //Location = AxisLocation.Bottom,
                //Title = "Day"
            };

            LineSeries3.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                //Orientation = AxisOrientation.Y,
                //Location = AxisLocation.Left,
                //Title = "Value"
            };
            LineSeries3.IndependentAxis = new DateTimeAxis
            {
                Maximum = max.AddDays(1),
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                //Orientation = AxisOrientation.X,
                //Location = AxisLocation.Bottom,
                //Title = "Day"
            };

            LineSeries4.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            LineSeries4.IndependentAxis = new DateTimeAxis
            {
                Maximum = max.AddDays(1),
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Day"
            };

            LineSeries5.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            */
            LineSeries5.IndependentAxis = LineSeries4.IndependentAxis = LineSeries3.IndependentAxis = LineSeries2.IndependentAxis = LineSeries1.IndependentAxis;
            LineSeries5.DependentRangeAxis = LineSeries4.DependentRangeAxis = LineSeries3.DependentRangeAxis = LineSeries2.DependentRangeAxis = LineSeries1.DependentRangeAxis;
            LineSeries1.IndependentValuePath = "Date";
            LineSeries1.DependentValuePath = "Value";
            LineSeries1.ItemsSource = dt_Gold.DefaultView;

            LineSeries2.IndependentValuePath = "Date";
            LineSeries2.DependentValuePath = "Value";
            LineSeries2.ItemsSource = dt_DJI .DefaultView;

            LineSeries3.IndependentValuePath = "Date";
            LineSeries3.DependentValuePath = "Value";
            LineSeries3.ItemsSource = dt_CCMP.DefaultView;

            LineSeries4.IndependentValuePath = "Date";
            LineSeries4.DependentValuePath = "Value";
            LineSeries4.ItemsSource = dt_SCIN.DefaultView;

            LineSeries5.IndependentValuePath = "Date";
            LineSeries5.DependentValuePath = "Value";
            LineSeries5.ItemsSource = dt_INX.DefaultView;


            /*
            all_index =  spider.GetIndex();
            button1.Content = all_index.DJI;
            */

            /*
            DataTable dt = new DataTable("Sold");
            dt.Columns.Add(new DataColumn("X", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Y", typeof(int)));


            DataRow row = dt.NewRow();
            row["X"] = "2012-01-01";
            row["Y"] = 1;
            dt.Rows.Add(row);
            DataRow row2 = dt.NewRow();
            row2["Y"] = 2;
            row2["X"] = "2012-01-02";
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3["Y"] = 3;
            row3["X"] = "2012-01-03";
            dt.Rows.Add(row3);
            DataRow row4 = dt.NewRow();
            row4["Y"] = 4;
            row4["X"] = "2012-01-04";
            dt.Rows.Add(row4);
            DataRow row5 = dt.NewRow();
            row5["Y"] = 5;
            row5["X"] = "2012-01-05";
            dt.Rows.Add(row5);
            DataRow row6 = dt.NewRow();
            row6["Y"] = 6;
            row6["X"] = "2012-01-06";
            dt.Rows.Add(row6);

            Random rd = new Random();

            for (int i = 0; i < 100; i++)
            {
                DataRow row_t = dt.NewRow();
                row_t["X"] = Convert.ToDateTime("2012-01-06").AddDays(i);
                row_t["Y"] = rd.Next(0,100);
                dt.Rows.Add(row_t);
            }

*/
        }

        private void LineSeries8_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            button1.Content = LineSeries1.SelectedItem.ToString();
        }

        private void LineSeries1_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            button1.Content = LineSeries1.SelectedItem.ToString();

        }

        private void LineSeries1_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void LineSeries1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void dataGrid_Record_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String title = ((System.Data.DataRowView)(dataGrid_Record.CurrentCell.Item)).Row.ItemArray[2].ToString();
            String date = ((System.Data.DataRowView)(dataGrid_Record.CurrentCell.Item)).Row.ItemArray[1].ToString();
            String detail = ((System.Data.DataRowView)(dataGrid_Record.CurrentCell.Item)).Row.ItemArray[3].ToString();
            RecordDetail rd = RecordDetail.getRecordDetail(title,date,detail,this.Top,this.Left);

            rd.Show();
        }

        private void MenuItem_Lock_Click(object sender, RoutedEventArgs e)
        {
            if (signal_lock)
            {
                MenuItem_Lock.Header = "Lock";
                signal_lock = false;
            } 
            else
            {
                MenuItem_Lock.Header = "UnLock";
                signal_lock = true;
            }

        }

        /// <summary>
        /// 定义定时器 函数 tm_Tick()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_Tick(object sender, EventArgs e)
        {

            DateTime now = DateTime.Now;
            if (now.Second == 0)
            {
                //爬虫获取最新的指数
                AllIndex a_index = spider.GetIndex();

                //绑定各种指数
                label_DJI.Content = a_index.DJI;
                label_DJI_Change.Content = a_index.DJI_Change;
                label_DJI_ChangePer.Content = a_index.DJI_ChangePer;
                label_DJI_Time.Content = a_index.DJI_Time;
                label_CCMP.Content = a_index.CCMP;
                label_CCMP_Change.Content = a_index.CCMP_Change;
                label_CCMP_ChangePer.Content = a_index.CCMP_ChangePer;
                label_CCMP_Time.Content = a_index.CCMP_Time;
                label_SCIN.Content = a_index.SCIN;
                label_SCIN_Change.Content = a_index.SCIN_Change;
                label_SCIN_ChangePer.Content = a_index.SCIN_ChangePer;
                label_SCIN_Time.Content = a_index.SCIN_Time;

                label_Price_Buy.Content = a_index.gold.Price_Buy;
                label_Price_Sell.Content = a_index.gold.Price_Sell;
                label_Price_Change.Content = a_index.gold.Price_Change;
                label_Price_ChangePer.Content = a_index.gold.Price_ChangePer;
                label_IntPrice_Buy.Content = a_index.gold.IntPrice_Buy;
                label_IntPrice_Sell.Content = a_index.gold.IntPrice_Sell;
                label_IntPrice_Change.Content = a_index.gold.IntPrice_Change;
                label_IntPrice_ChangePer.Content = a_index.gold.IntPrice_ChangePer;


                //更新dt_gold_new和dt_gold_new_sell黄金数据datable,
                DataRow row = dt_gold_new.NewRow();
                DataRow row2 = dt_gold_new_sell.NewRow();
                row2["Date"]=row["Date"] = DateTime.Now;
                row["Value"] = a_index.gold.IntPrice_Buy;
                row2["Value"] = a_index.gold.IntPrice_Sell;
                dt_gold_new.Rows.Add(row);
                dt_gold_new_sell.Rows.Add(row2);


                //此处无需考虑 资源使用冲突
                LineSeries11.IndependentValuePath=LineSeries10.IndependentValuePath = "Date";
                LineSeries11.DependentValuePath=LineSeries10.DependentValuePath = "Value";
                LineSeries10.ItemsSource = dt_gold_new.DefaultView;
                LineSeries11.ItemsSource = dt_gold_new_sell.DefaultView;
                /*
                LineSeries10.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            LineSeries10.ItemsSource = dt_gold_new.DefaultView;
                        }
                        ));
                 */ 
                /*
                //初始化nThread;
                Thread nThread = new Thread(new ThreadStart(update_Gold));
                nThread.Start();
                 */
            }
        }

        /*
        /// <summary>
        /// 线程
        /// </summary>
        private void update_Gold()
        {
            while (true)
            {
            AllIndex a_index = spider.GetIndex();

            DataRow row = dt_gold_new.NewRow();
            row["Date"] = DateTime.Now;
            row["Value"] = a_index.gold.IntPrice_Buy;
            dt_gold_new.Rows.Add(row);

            LineSeries10.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        LineSeries10.ItemsSource = dt_gold_new.DefaultView;
                    }
                    ));
          }
        }
        */



        private void update_Timer()
        {
            //初始化定时器tm
            tm.Tick += new EventHandler(tm_Tick);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();
        }

        /// <summary>
        /// 之前采用Timer时候的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void update_Gold()
        {
            //爬虫获取最新的指数
            AllIndex a_index = spider.GetIndex();

            //绑定各种指数
            label_DJI.Content = a_index.DJI;
            label_DJI_Change.Content = a_index.DJI_Change;
            label_DJI_ChangePer.Content = a_index.DJI_ChangePer;
            label_DJI_Time.Content = a_index.DJI_Time;
            label_CCMP.Content = a_index.CCMP;
            label_CCMP_Change.Content = a_index.CCMP_Change;
            label_CCMP_ChangePer.Content = a_index.CCMP_ChangePer;
            label_CCMP_Time.Content = a_index.CCMP_Time;
            label_SCIN.Content = a_index.SCIN;
            label_SCIN_Change.Content = a_index.SCIN_Change;
            label_SCIN_ChangePer.Content = a_index.SCIN_ChangePer;
            label_SCIN_Time.Content = a_index.SCIN_Time;

            label_Price_Buy.Content = a_index.gold.Price_Buy;
            label_Price_Sell.Content = a_index.gold.Price_Sell;
        


            //更新dt_gold_new黄金数据datable
            DataRow row = dt_gold_new.NewRow();
            row["Date"] = DateTime.Now;
            row["Value"] = a_index.gold.IntPrice_Buy;
            dt_gold_new.Rows.Add(row);

            //绑定，并防止 资源使用冲突
            LineSeries10.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        LineSeries10.ItemsSource = dt_gold_new.DefaultView;
                    }
                    ));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {


            /*

            DataTable dt = new DataTable("Sold");
            dt.Columns.Add(new DataColumn("X", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Y", typeof(int)));


            DataRow row = dt.NewRow();
            row["X"] = "2012-01-01";
            row["Y"] = 1;
            dt.Rows.Add(row);
            DataRow row2 = dt.NewRow();
            row2["Y"] = 2;
            row2["X"] = "2012-01-02";
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3["Y"] = 3;
            row3["X"] = "2012-01-03";
            dt.Rows.Add(row3);


            LineSeries_gold.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            DateTime min = Convert.ToDateTime("2012-01-01");
            DateTime max = Convert.ToDateTime("2012-01-04");

            LineSeries_gold.IndependentAxis = new DateTimeAxis
            {
                Maximum = max,
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Day"
            };
                         */

        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            DataTable dt_Gold33 = data_impl.Table_Get("..//..//Data2012//黄金2012.txt", 2000,0);
            LineSeries10.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1,
                Minimum = 0,
                Interval = 0.1,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            DateTime max = Convert.ToDateTime("2012-12-31");
            DateTime min = Convert.ToDateTime("2012-01-01");
            LineSeries10.IndependentAxis = new DateTimeAxis
            {
                Maximum = max.AddDays(1),
                Minimum = min,
                IntervalType = DateTimeIntervalType.Months,
                Interval = 1,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Day"
            };


            LineSeries10.IndependentValuePath = "Date";
            LineSeries10.DependentValuePath = "Value";
            LineSeries10.ItemsSource = dt_Gold33.DefaultView;
        }

        private void button_Compute_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt_goldprice = data_impl.Table_NNMGet("..//..//Data//goldprice.txt");
            double [] list =new double [60];
            double predict_value = 0;

            StreamWriter sw = new StreamWriter("..//..//Data//gold2.txt");
            sw.WriteLine("'XAUUSD'	黄金现货/美元	分时	");
            sw.WriteLine("时间	成交价	成交量	成交额");
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 60;j++)
                {
                    //取得list样本
                    list[j] = Convert.ToDouble(dt_goldprice.Rows[i + j][1])*100;
                }
                predict_value = NNM.get_NNMResult(list)/100;
                sw.WriteLine(dt_goldprice.Rows[i + 60][0] + "\t" + predict_value);

            }
            sw.Close();
            Compute_ResultCompare();
        }

        private void Compute_ResultCompare()
        {
            DataTable dt_Gold1 = data_impl.Table_NNMGet("..//..//Data//gold1.txt");
            DataTable dt_Gold_Predict = data_impl.Table_NNMGet("..//..//Data//gold2.txt");
            LineSeries13.DependentRangeAxis = new LinearAxis
            {
                Maximum = 1400,
                Minimum = 1390,
                Interval = 0.5,
                Orientation = AxisOrientation.Y,
                Location = AxisLocation.Left,
                Title = "Value"
            };
            DateTime max = Convert.ToDateTime("2013-05-16 07:40");
            DateTime min = Convert.ToDateTime("2013-05-16 07:00");
            LineSeries13.IndependentAxis = new DateTimeAxis
            {
                Maximum = max,
                Minimum = min,
                IntervalType = DateTimeIntervalType.Minutes,
                Interval = 1,
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Title = "Time"
            };
            LineSeries14.IndependentAxis = LineSeries13.IndependentAxis;
            LineSeries14.DependentRangeAxis = LineSeries13.DependentRangeAxis;


            LineSeries13.IndependentValuePath = "Date";
            LineSeries13.DependentValuePath = "Value";
            LineSeries13.ItemsSource = dt_Gold1.DefaultView;

            LineSeries14.IndependentValuePath = "Date";
            LineSeries14.DependentValuePath = "Value";
            LineSeries14.ItemsSource = dt_Gold_Predict.DefaultView;
        }

        private void button_Compare_Click(object sender, RoutedEventArgs e)
        {
            Compute_ResultCompare();
        }

    }
}
