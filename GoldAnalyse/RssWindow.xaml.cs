using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GoldAnalyse.RSSReader;
using GoldAnalyse.XmlImpl;
using System.Data;
using System.Threading;


namespace GoldAnalyse
{
    /// <summary>
    /// MiniWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MiniWindow : Window
    {
        RssRead rssread;
        Rss_xml rssxml = new Rss_xml();
        static String[] urls = new String[10];
        static DataTable[] dt_rss = new DataTable[5];
        static Thread iThread;
        static Thread oThread;

        public MiniWindow()
        {
            InitializeComponent();
            InitializeWork();
            iThread = new Thread(new ThreadStart(rss_first_Bind));
            iThread.Start();
        }

        private void urls_Reset(){
            urls = rssxml.getUrls();
        }

        private void rss_first_Bind()
        {
//            rss_first_Get();
//            iThread.Abort();
//            this.dataGridShowInfo.ItemsSource = rssread.ReadRss(urls[0]).DefaultView;
            oThread = new Thread(new ThreadStart(rss_dt_Get));
            oThread.Start();
       
        }

        private void rss_first_Get()
        {
            dt_rss[0] = rssread.ReadRss(urls[0]);
        }

        private void rss_dt_Get()
        {
            dt_rss[0] = rssread.ReadRss(urls[0]);
            dt_rss[1] = rssread.ReadRss(urls[1]);
            dt_rss[2] = rssread.ReadRss(urls[2]);
            dt_rss[3] = rssread.ReadRss(urls[3]);
            dt_rss[4] = rssread.ReadRss(urls[4]);
            oThread.Abort();
        }

        private void DataGrid_Bind(DataTable dt_rss)
        {
            dataGridShowInfo.ColumnWidth = DataGridLength.SizeToHeader;
            //默认信息表显示
            this.dataGridShowInfo.ItemsSource = dt_rss.DefaultView;
            //this.listBoxWebSite.DataContext = rssread.ReadTextFiles(@"Resource/rssSite.txt");

        }

        private void head_Set()
        {
            Selection_1.Content = urls[5];
            Selection_2.Content = urls[6];
            Selection_3.Content = urls[7];
            Selection_4.Content = urls[8];
            Selection_5.Content = urls[9];
        }


        private void InitializeWork()
        {
            rssread = new RssRead();
            //默认网站
            //String url = "http://news.baidu.com/n?cmd=1&class=shizheng&tn=rss&sub=0";

            urls_Reset();
            head_Set();
            //DataGridShowInfo数据绑定
            this.dataGridShowInfo.ItemsSource = rssread.ReadRss(urls[0]).DefaultView;

        }



        private void dataGridShowInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void dataGridShowInfo_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*
            if (e.OriginalSource is System.Windows.Documents.Hyperlink)
            {
                Hyperlink h = e.OriginalSource as Hyperlink;

                string hashtag = h.Tag.ToString();
                string hashtagUrl = String.Format(Settings.Default.HashtagUrl, Uri.EscapeDataString(hashtag));

                try
                {
                    System.Diagnostics.Process.Start(hashtagUrl);
                }
                catch
                {
                    // TODO: Warn the user? Log the error? Do nothing since Witty itself is not affected?
                }

                e.Handled = true;
            }*/
            String url = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[3].ToString();
            System.Diagnostics.Process.Start(url);

        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this) this.DragMove();
        }

        private void dataGridShowInfo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this.dataGridShowInfo) this.DragMove();
        }

        private void dataGridShowInfo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dataGridShowInfo.CurrentCell.IsValid == false)
            {
            } 
            else
            {
                String url = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[3].ToString();

                //Title.Content = url;
                String content = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[5].ToString();
                content_Block.Text = content;
            }
        }

        private void dataGridShowInfo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (groupBox1.Visibility == Visibility.Visible)
            {
                groupBox1.Visibility = Visibility.Hidden;
            } 
            else
            {
                groupBox1.Visibility = Visibility.Visible;
            }

            if (dataGridShowInfo.CurrentCell.IsValid == false)
            {
            }
            else
            {
                String url = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[3].ToString();
                Title2.Content = url;
               // hyperlink0.NavigateUri = new System.Uri(url);
                String title = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[0].ToString();
                Title.Content = title;
                String content = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[5].ToString();
                content_Block.Text = content;
            }
        }

        private void hyperlink0_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Title2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(Title2.Content.ToString());
        }

        private void MenuItem_Record_Click(object sender, RoutedEventArgs e)
        {
            String title = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[0].ToString();
            String date = Convert.ToDateTime((((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[2].ToString())).ToShortDateString();
            String content = ((System.Data.DataRowView)(dataGridShowInfo.CurrentCell.Item)).Row.ItemArray[5].ToString();
            RecordEdit re = new RecordEdit(date,title,content);
            re.Show();
        }

        private void Selection_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!oThread.IsAlive)
            {
                DataGrid_Bind(dt_rss[0]);
            }
        }

        private void Selection_2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!oThread.IsAlive)
            {
                DataGrid_Bind(dt_rss[1]);
            }
        }

        private void Selection_3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!oThread.IsAlive)
            {
                DataGrid_Bind(dt_rss[2]);
            }
        }

        private void Selection_4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!oThread.IsAlive)
            {
                DataGrid_Bind(dt_rss[3]);
            }
        }

        private void Selection_5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!oThread.IsAlive)
            {
                DataGrid_Bind(dt_rss[4]);
            }
        }

        private void Selection_Setting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
