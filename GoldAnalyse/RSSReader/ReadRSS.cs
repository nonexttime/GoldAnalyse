using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

namespace GoldAnalyse.RSSReader
{
    public class RssRead
    {
        public RssRead()
        {
        }

        /// <summary>
        /// 读取rss内容
        /// </summary>
        /// <param name="RssURL">有效的rssURL</param>
        /// <returns>datatable对象</returns>
        public DataTable ReadRss(string RssURL)
        {
            DataTable Dt = new DataTable();

            DataColumn Title = new DataColumn("Title", typeof(string));
            DataColumn Author = new DataColumn("Author", typeof(string));
            DataColumn PubDate = new DataColumn("PubDate", typeof(string));
            DataColumn Link = new DataColumn("Link", typeof(string));
            DataColumn description = new DataColumn("description", typeof(string));

            DataColumn source = new DataColumn("source", typeof(string));

            Dt.Columns.Add(Title);
            Dt.Columns.Add(Author);
            Dt.Columns.Add(PubDate);
            Dt.Columns.Add(Link);
            Dt.Columns.Add(description);
            Dt.Columns.Add(source);

            System.Net.WebRequest myRequest = System.Net.WebRequest.Create(RssURL);
            System.Net.WebResponse myResponse = myRequest.GetResponse();

            System.IO.Stream rssStream = myResponse.GetResponseStream();
            System.Xml.XmlDocument rssDoc = new System.Xml.XmlDocument();
            rssDoc.Load(rssStream);

            System.Xml.XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");
            //设定
            int Count = 10;
            for (int i = 0; i < Count; i++)
            {
                DataRow Row = Dt.NewRow();
                System.Xml.XmlNode rssDetail;
                //标题  
                rssDetail = rssItems.Item(i).SelectSingleNode("title");
                if (rssDetail != null)
                {
                    Row["Title"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Title"] = "";
                }
                //作者  
                rssDetail = rssItems.Item(i).SelectSingleNode("author");
                if (rssDetail != null)
                {
                    Row["Author"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Author"] = "";
                }
                //发布时间  
                rssDetail = rssItems.Item(i).SelectSingleNode("pubDate");
                if (rssDetail != null)
                {
                    Row["PubDate"] = Convert.ToDateTime(rssDetail.InnerText).ToString("yyyy年MM月dd日");
                }
                else
                {
                    Row["PubDate"] = "";
                }
                //链接地址  
                rssDetail = rssItems.Item(i).SelectSingleNode("link");
                if (rssDetail != null)
                {
                    Row["Link"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Link"] = "";
                }
                rssDetail = rssItems.Item(i).SelectSingleNode("description");
                if (rssDetail != null)
                {
                    Row["description"] = rssDetail.InnerText;
                }
                else
                {
                    Row["description"] = "";
                }
                rssDetail = rssItems.Item(i).SelectSingleNode("title");
                if (rssDetail != null)
                {
                    Row["source"] = rssDetail.InnerText;
                }
                else
                {
                    Row["source"] = "";
                }


                Dt.Rows.Add(Row);
            }
            return Dt;
        }


        /// <summary>
        /// 读取文件中存储的网站
        /// </summary>
        /// <param name="files">文件路径</param>
        /// <returns>字符串列表对象</returns>
        public ObservableCollection<String> ReadTextFiles(string files)
        {
            ObservableCollection<String> lines = new ObservableCollection<string>();
            try
            {
                int i = 0;
                using (StreamReader sr = new StreamReader(files, Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                        i++;
                    }
                    sr.Close();
                }
            }
            catch
            {

            }
            return lines;
        }
        /// <summary>
        /// 将订阅的rssRUL写入文件中
        /// </summary>
        /// <param name="subscribRUL">rul</param>
        /// <param name="files"></param>
        public void WriteTextFiles(string subscribRUL, string files)
        {
            try
            {
                DataTable Dt = ReadRss(subscribRUL);
            }
            catch
            {
                System.Windows.MessageBox.Show("不是合法的rss feed地址");
                return;
            }

            try
            {
                if (File.Exists(files))
                {
                    using (StreamReader sr = new StreamReader(files, Encoding.Default))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == subscribRUL)//判断是否重复
                            {
                                System.Windows.MessageBox.Show("该资源已订阅过");
                                return;
                            }
                        }
                        sr.Close();
                    }
                }
                else
                {
                    FileStream fstream = new FileStream(files, FileMode.Create);
                    fstream.Close();
                }

            }
            catch
            {

            }

            //如果此rssRUL没有订阅过，则写入文件中
            FileStream fs = new FileStream(files, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Flush();
            sw.WriteLine(subscribRUL);
            sw.Flush();
            sw.Close();
            fs.Close();
            //System.Windows.MessageBox.Show("订阅成功！");
        }

        //删除
        public void DeleteTextFiles(int rssIndex, string files)
        {
            try
            {
                if (File.Exists(files))
                {
                    List<string> lines = new List<string>(File.ReadAllLines(files));
                    lines.RemoveAt(rssIndex);
                    File.WriteAllLines(files, lines.ToArray());

                    System.Windows.MessageBox.Show("已经删除");
                }
                else
                {
                    System.Windows.MessageBox.Show("文件不存在");
                }

            }
            catch
            {

            }
        }
    }
}
