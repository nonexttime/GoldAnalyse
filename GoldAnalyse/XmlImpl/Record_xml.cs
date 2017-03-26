using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
namespace GoldAnalyse.XmlImpl
{
    class Record_xml
    {
        XmlDocument document = new XmlDocument();
        XmlElement root;

        //初始化document和root
        private void xml_Initial()
        {
            document.Load("..//..//Record//Record.xml");
            root = document.DocumentElement;
        }

        //保存document
        private void xml_Save()
        {
            document.Save("..//..//Record//Record.xml");
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        public void Add(String MID,String date,String title,String detail)
        {
            xml_Initial();
            //定义新的XmlElement
            XmlElement newRecord = document.CreateElement("Matter");
            XmlElement newDate = document.CreateElement("Date");
            XmlElement newMID = document.CreateElement("MID");
            XmlElement newTitle = document.CreateElement("Title");
            XmlElement newContent = document.CreateElement("Content");

            //定义新的XmlText
            XmlText date2 = document.CreateTextNode(date);
            XmlText mid2 = document.CreateTextNode(MID);
            XmlText title2 = document.CreateTextNode(title);
            XmlText content2 = document.CreateTextNode(detail);

            //将XmlElement置入newRecord作为子节点
            newRecord.AppendChild(newDate);
            newRecord.AppendChild(newMID);
            newRecord.AppendChild(newTitle);
            newRecord.AppendChild(newContent);

            //给子元素赋值
            newDate.AppendChild(date2);
            newMID.AppendChild(mid2);
            newTitle.AppendChild(title2);
            newContent.AppendChild(content2);

            //添加记录newRecord，在xml文件的末端
            root.InsertAfter(newRecord, root.LastChild);
            //调用函数保存document,一定要保存，否则添加失败
            xml_Save();
        }

    }


}
