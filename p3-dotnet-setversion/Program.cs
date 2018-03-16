using System;
using System.IO;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.XPath;

namespace dotnet_setversion
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = args[0];

            var files = Directory.GetDirectories(dir);

            IterateDirectories(files, args[1]);
        }


        static void IterateDirectories(string[] dirs, string version)
        {
            foreach (var dir in dirs)
            {
                var files = Directory.GetFiles(dir, "*.csproj");

                foreach (var file in files)
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(file);
                    var project = document.FirstChild;

                    if (document.ChildNodes.Count == 2)
                    {
                        project = document.ChildNodes[1];
                    }

                 
                    var props = project.FirstChild;

                    var versionProp = props.SelectNodes("Version");

                    if (versionProp.Count == 1)
                    {
                        versionProp[0].InnerText = version;
                    }
                    else
                    {
                        var node = document.CreateNode(XmlNodeType.Element, "Version", null);
                        node.InnerText = version;

                        props.AppendChild(node);
                    }

                    document.Save(new StreamWriter(file));



                }

                IterateDirectories(Directory.GetDirectories(dir), version);
            }
        }
    }
}
