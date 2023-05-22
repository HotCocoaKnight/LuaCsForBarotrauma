using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Barotrauma;

public class ResourceFile
{
    private XDocument document;
    private string path;

    void ParseResource(XElement resource)
    {
        if (ResourcePrefab.Prefabs == null)
        {
            ResourcePrefab.Prefabs = new List<ResourcePrefab>();
        }
        ResourcePrefab.Prefabs.Add(new ResourcePrefab(resource));
    }
    
    void ParseResources(XElement parent)
    {
        foreach (var e in parent.Elements())
        {
            switch (e.Name.ToString())
            {
                case "Resource":
                    ParseResource(e);
                    break;
            }
        }
    }

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
            switch (element.Name.ToString())
            {
                case "Resources":
                    ParseResources(element);
                    break;
            }
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
