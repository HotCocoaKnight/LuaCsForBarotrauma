using System.Collections.Generic;
using System.Xml.Linq;

namespace Barotrauma;

partial class ResourceFile : ContentFile
{
    public ResourceFile(ContentPackage contentPackage, ContentPath path, XDocument xDoc) : base(contentPackage, path)
    {
        this._xDocument = xDoc;
    }

    private XDocument _xDocument;

    public void ParseResource(XElement e)
    {
        ResourcePrefab newPrefab = new ResourcePrefab(new ContentXElement(this.ContentPackage, e), this);
    }
    
    public void ParseResources(IEnumerable<XElement> elements)
    {
        foreach (XElement e in elements)
        {
            switch (e.Name.ToString())
            {
                case "Resource":
                {
                    ParseResource(e);
                } break;
            }
        }
    }
    
    public override void LoadFile()
    {
        foreach (XElement element in _xDocument.Elements())
        {
            if (element.Name.ToString() == "Resources")
            {
                ParseResources(element.Elements());
            }
        }
    }

    public override void UnloadFile()
    {
        throw new System.NotImplementedException();
    }

    public override void Sort()
    {
        throw new System.NotImplementedException();
    }
}
