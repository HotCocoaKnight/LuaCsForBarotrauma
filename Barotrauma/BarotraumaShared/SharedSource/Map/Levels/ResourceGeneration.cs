using System.Collections.Generic;
using System.Numerics;
using Voronoi2;

namespace Barotrauma;


public class ResourceGenerator
{
    private Level activeLevel;
    public static ResourceGenerator ActiveGenerator;

    public ResourceGenerator()
    {
        activeLevel = Level.Loaded;
        ActiveGenerator = this;
    }

    public void GenerateResource(Vector2 wrldpos)
    {
        
    }
}
