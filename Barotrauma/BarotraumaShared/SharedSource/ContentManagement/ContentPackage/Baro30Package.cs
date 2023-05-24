using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Barotrauma;

public class ResourceFile
{
    private XDocument document;
    private string path;

    public void ParseBuildingObject(XElement element)
    {
        
    }

    public void ParseBuildingObjects(XElement elements)
    {
        foreach (var e in elements.Elements())
        {
            ParseBuildingObject(e);
        }
    }
    
    public void Load()
    {
        foreach (var element in document.Elements())
        {
            
        }
    }
    
    public ResourceFile(XDocument doc, string p)
    {
        document = doc;
        path = p;
    }
}

class Baro30Package : ContentPackage
{
    public Baro30Package(XDocument doc, string path) : base(doc, path)
    {
        ContentPath p = ContentPath.FromRaw(this, path);
        new ResourceFile(doc, path).Load();
    }
}
