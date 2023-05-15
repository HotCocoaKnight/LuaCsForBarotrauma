using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Barotrauma;

public class ContentPackage30
{
    public class ContentData
    {
        public ContentData()
        {
            name = string.Empty;
            iscore = false;
        }
        public string name;
        public bool iscore;
    }

    public ContentPackage30(string name, bool core, XElement contentpackage)
    {
        this.desc = new ContentData();
        desc.name = name;
        desc.iscore = core;
        this.contentPackage = contentpackage;
    }

    private XElement contentPackage;
    
    private ContentData desc = new ContentData();

    public virtual void LoadElement(XElement element)
    {
        switch (element.Name.ToString())
        {
            case "Resource":
            {
                string name = element.GetAttributeString("name", "no-name");
                string path = element.GetAttributeString("sprite", "notexture");
                int durability = element.GetAttributeInt("durability", 10);
                ResourcePrefab resourcePrefab = new ResourcePrefab(path, name, durability);
            } break;
        }
    }
    
    public virtual void Load()
    {
        foreach(XElement e in contentPackage.Elements())
            LoadElement(e);
    }
}

public static class Baro30Loader
{
    public static List<ContentPackage30> PackagesToLoad = new List<ContentPackage30>();
    public static List<ContentPackage30> LoadedPackageList = new List<ContentPackage30>();

    private static void ParseBtElement(XElement element)
    {
        switch (element.Name.ToString())
        {
            case "contentpackage":
                ContentPackage30 content = new ContentPackage30("noname", true, element);
                PackagesToLoad.Add(content);
                break;
        }
    }
    public static void LoadBT30()
    {
        XDocument xDoc = XDocument.Load("Content/ContentPackages/BT30.xml");
        foreach (XElement element in xDoc.Elements())
        {
            ParseBtElement(element);
        }

        foreach (ContentPackage30 package in PackagesToLoad)
        {
            package.Load();
        }
    }
}
