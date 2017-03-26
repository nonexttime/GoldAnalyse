using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Windows.Data;
using System.IO;

namespace GoldAnalyse.GoldImpl
{
    public class DataImpl
    {
        public DataTable Table_Get(String fileName,double Cut,double Base)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Value", typeof(double)));

            using (Stream resourceStream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(resourceStream, Encoding.GetEncoding("GB2312")))
                {
                    //读每一行
                    var strings = reader.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //获取股票名称
                    //goldName = strings[0].Replace("\r", "");

                    //var res = new List<GoldInfo>(strings.Length - 2);

                    //第一行是股票名称, 第二行是类型名称, 第3行才是股票数据
                    for (int i = 2; i < strings.Length; i++)
                    {
                        string line = strings[i];
                        string[] subLines = line.Split('\t');

                        DataRow row = dt.NewRow();

                        row["Date"] = DateTime.Parse(subLines[0]);

                        row["Value"] = (Double.Parse(subLines[4])) / Cut-Base;
                        //                        Double middle = Double.Parse(subLines[6]);
                        dt.Rows.Add(row);
                    }

                }
            }
            return dt;
        }

        public DataTable Table_NNMGet(String fileName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Value", typeof(double)));

            using (Stream resourceStream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(resourceStream, Encoding.GetEncoding("GB2312")))
                {
                    //读每一行
                    var strings = reader.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //获取股票名称
                    //goldName = strings[0].Replace("\r", "");

                    //var res = new List<GoldInfo>(strings.Length - 2);

                    //第一行是股票名称, 第二行是类型名称, 第3行才是股票数据
                    for (int i = 2; i < strings.Length; i++)
                    {
                        string line = strings[i];
                        string[] subLines = line.Split('\t');

                        DataRow row = dt.NewRow();

                        row["Date"] = DateTime.Parse(subLines[0]);

                        row["Value"] = (Double.Parse(subLines[1]));
                        //                        Double middle = Double.Parse(subLines[6]);
                        dt.Rows.Add(row);
                    }

                }
            }
            return dt;
        }
    }
}
