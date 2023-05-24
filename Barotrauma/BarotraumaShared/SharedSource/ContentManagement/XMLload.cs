using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Barotrauma;

public class BlockFile
{
    private string path;
    public BlockFile(string path)
    {
        this.path = path;
        XDocument document = XMLExtensions.TryLoadXml(path);
        foreach (var e in document.Elements())
        {
            switch (e.Name.ToString())
            {
                case "Blocks":
                    foreach (var block in e.Elements())
                    {
                        BlockPrefab.Prefabs.Add(new BlockPrefab(block));
                    }
                    break;
            }
        }
    }
}

public static class Baro30Loader
{
    public static void LoadBT30()
    {
        List<Baro30Package> toLoad = new List<Baro30Package>();
        XDocument xDoc = XDocument.Load("Content/ContentPackages/BT30.xml");
        foreach (XElement element in xDoc.Elements())
        {
            foreach (XElement e in element.Elements())
            {
                string path = e.GetAttributeString("file", "");
                switch (e.Name.ToString())
                {
                    case "Block":
                        new BlockFile(path);
                        break;
                }
            }
        }
    }
}
