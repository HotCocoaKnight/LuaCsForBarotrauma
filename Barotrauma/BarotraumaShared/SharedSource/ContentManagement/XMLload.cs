using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Barotrauma;

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
                if (e.Name.ToString().Equals("Resource"))
                {
                    string path = e.GetAttributeString("file", "");
                    if (path.Equals(""))
                    {
                        DebugConsole.ThrowError("ERROR PATH NOT SPECIFIED", new Exception("Path not specified for barotrauma 3.0 package"), true);
                        return;
                    }
                    XDocument doc = XDocument.Load(path);
                    toLoad.Add(new Baro30Package(doc, path));
                }
            }
        }

        if (toLoad.Any())
        {
            foreach (Baro30Package xd in toLoad)
            {
                xd.LoadContent();
            }
        }
    }
}
