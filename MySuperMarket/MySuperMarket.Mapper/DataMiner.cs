using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using MySuperMarket.ClassLibrary;

namespace MySuperMarket.Mapper
{
    public class DataMiner
    {

        //WebRequest request = null;
        //WebResponse response = null;
        //Stream datastream = null;
        //StreamReader reader = null;
        WebClient webClient = null;


        public void DownloadFile(string url, string file)
        {
            webClient = new WebClient();
            webClient.UseDefaultCredentials = true;
            webClient.Headers.Add("User-Agent: Other");
            webClient.Headers.Add("referer", url);
            webClient.DownloadFile(url, file);
        }

        public XDocument UnzipFileToXDocument(string filePath)
        {
            var xmlDoc = new XmlDocument();
            using (var file = File.Open(filePath, FileMode.Open))
            using (var zip = new GZipStream(file, CompressionMode.Decompress))
            {
                xmlDoc.Load(zip);
            }
            var xDoc = new XDocument();
            xDoc = XDocument.Load(new XmlNodeReader(xmlDoc));

            return xDoc;
        }

    }
}
