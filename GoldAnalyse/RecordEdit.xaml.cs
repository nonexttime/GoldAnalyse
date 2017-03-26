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


using GoldAnalyse.XmlImpl;

namespace GoldAnalyse
{
    /// <summary>
    /// RecordEdit.xaml 的交互逻辑
    /// </summary>
    public partial class RecordEdit : Window
    {
        private Record_xml xml_record = new Record_xml();
        public RecordEdit()
        {
            InitializeComponent();
        }

        public RecordEdit(String date,double open,double high,double low,double close)
        {
            InitializeComponent();
            label_Date.Content ="日期："+ date;
            label_GoldInfo.Content = "价格：开:" + open + " 高:" + high + " 低:" + low + " 收:" + close;
        }

        public RecordEdit(String date,String title,String detail)
        {
            InitializeComponent();
            label_Date.Content = date;
            label_GoldInfo.Visibility = Visibility.Hidden;
            textBox_Title.Text = title;
            textBox_Detail.Text = detail;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            xml_record.Add("1",label_Date.Content.ToString() , textBox_Title.Text, textBox_Detail.Text);
            this.Close();
        }


        private void Record_Insert()
        {


        }

    }
}
