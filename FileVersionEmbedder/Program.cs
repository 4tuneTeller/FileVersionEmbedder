using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;

namespace FileVersionEmbedder
{
    class Program
    {
        static string fileName = "Index.html";

        static void Main(string[] args)
        {
            
            if (args.Length > 0)
            {
                fileName = args.First();
            }
            if (!File.Exists(fileName))
            {
                Console.WriteLine("ERROR! Can't get access to file " + fileName + "!");
                Console.ReadKey();
                return;
            }

            try
            {
                HtmlDocument indexFile = new HtmlDocument();
                indexFile.Load(fileName);
                foreach (HtmlNode script in indexFile.DocumentNode.SelectNodes("//script[@src]"))
                {
                    HtmlAttribute src = script.Attributes["src"];
                    if (src == null) continue;
                    src.Value = makeNewSrcValue(src);
                }
                foreach (HtmlNode link in indexFile.DocumentNode.SelectNodes("//link[@href]"))
                {
                    HtmlAttribute src = link.Attributes["href"];
                    if (src == null) continue;
                    src.Value = makeNewSrcValue(src);
                }
                indexFile.Save(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR! " + ex.Message);
                Console.ReadKey();
            }
        }

        static string makeNewSrcValue(HtmlAttribute src)
        {
            var filePath = makeFilePath(src.Value);
            string newSrcValue = src.Value;
            if (!src.Value.StartsWith("http") && File.Exists(filePath))
            {
                DateTime fileModifiedDate = File.GetLastWriteTime(filePath);
                newSrcValue = src.Value.SetUrlParameter("version", fileModifiedDate.ToFileTime().ToString());
            }
            return newSrcValue;
        }

        static string makeFilePath(string urlPath)
        {
            string directory = Directory.GetCurrentDirectory();
            var filenamePos = fileName.LastIndexOf('\\');
            if (filenamePos != -1)
            {
                directory = fileName.Remove(filenamePos - 1);
            }

            var qPos = urlPath.IndexOf('?');
            if (qPos > 0)
            {
                urlPath = urlPath.Remove(qPos);
            }
            string path = urlPath.Replace('/', '\\');
            return directory + "\\" + path;
        }
    }
}
