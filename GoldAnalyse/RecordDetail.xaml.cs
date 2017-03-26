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

namespace GoldAnalyse
{
    /// <summary>
    /// RecordDetail.xaml 的交互逻辑
    /// </summary>
    public partial class RecordDetail : Window
    {
        private static RecordDetail instance = null;

        private RecordDetail()
        {
            InitializeComponent();
        }

        //将其构造函数私有
        private RecordDetail(String title,String date,String content,double x,double y)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Top = x;  //X轴与主窗体相同
            this.Left = y + 1110; //主窗体宽1110，因此在右侧显示为y+1110
            //绑定内容
            textBox_Title.Text = title; 
            textBox_Date.Text = date;
            textBlock_Detail.Text = content;
        }

        public static RecordDetail getRecordDetail(String title,String date,String content,double x,double y)
        {
            //判断是否已经建立过窗体
            if (instance == null)
            {
                instance = new RecordDetail(title, date, content, x, y);
            }
            else //如果建立，则更新内容
            {
                instance.textBlock_Detail.Text = title;
                instance.textBox_Date.Text = date;
                instance.textBlock_Detail.Text = content;
            }
            return instance;
        }

    }
}
