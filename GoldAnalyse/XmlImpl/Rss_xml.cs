using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace GoldAnalyse.XmlImpl
{
    class Rss_xml
    {
        XmlDocument document = new XmlDocument();
        XmlElement root;

        private void xml_Initial()
        {

        }

        public String[] getUrls()
        {
            document.Load("..//..//Record//RssUrl.xml");
            root = document.DocumentElement;

            String[] urls = {"","","","","","","","","",""};

            if (root == null)
                return null;
            if (root is XmlElement)
            {
                urls[0] = root.FirstChild.InnerText;
                urls[1] = root.FirstChild.NextSibling.InnerText;
                urls[2] = root.FirstChild.NextSibling.NextSibling.InnerText;
                urls[3] = root.FirstChild.NextSibling.NextSibling.NextSibling.InnerText;
                urls[4] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                urls[5] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                urls[6] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                urls[7] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                urls[8] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                urls[9] = root.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
            }

            return urls;
        }
    }
}
