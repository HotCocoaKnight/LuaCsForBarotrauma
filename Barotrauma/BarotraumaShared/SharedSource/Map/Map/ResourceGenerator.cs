using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Voronoi2;

namespace Barotrauma;

class ResourceGeneration
{
    public static void Generate(Level loaded)
    {
        Resource.Resources = new List<Resource>();
        foreach (VoronoiCell cell in loaded.GetAllCells())
        {
            foreach (Vector2 v in cell.BodyVertices)
            {
                Resource.Resources.Add(new Resource(Resource.Prefabs[0], Resource.Prefabs[0].Sprite, (ushort)Resource.FindFreeIdBlock(1), v));
            }
        }
    }
}
