using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Barotrauma;

class Baro30Package : ContentPackage
{
    public Baro30Package(XDocument doc, string path) : base(doc, path)
    {
        ContentPath p = ContentPath.FromRaw(this, path);
        new ResourceFile(this, p, doc).LoadFile();
    }
}
