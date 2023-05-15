using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace Barotrauma;

public class ResourcePrefab
{
    public class ResourceData
    {
        public ResourceData()
        {
            
        }
        public string SpritePath;
        public string Name;
        public float durability;
    }
    public static List<ResourcePrefab> Prefabs = new List<ResourcePrefab>();
    public ResourceData rData;
    
    public ResourcePrefab(string SpritePath, string Name, float durability)
    {
        rData = new ResourceData();
        rData.SpritePath = SpritePath;
        rData.Name = Name;
        rData.durability = durability;
        Prefabs.Add(this);
    }
}
