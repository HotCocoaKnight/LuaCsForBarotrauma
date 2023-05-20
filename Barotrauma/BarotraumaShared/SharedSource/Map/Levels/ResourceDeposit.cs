using System.Collections.Generic;
using System.Threading;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Voronoi2;

namespace Barotrauma;

static class ResourceDeposit
{
    public static void AddDeposit(Vector2 position, float normal, ResourcePrefab resourcePrefab)
    {
        new Resource(resourcePrefab, position, normal);
    }

    public static void CellVertToVector(Vector2 vertex, Vector2 translation, out Vector2 position)
    {
        position = new Vector2(vertex.X + translation.X, -(vertex.Y + translation.Y));
    }

    public static void GenerateAllDeposits(Level level)
    { 
        List<VoronoiCell> cells = level.GetAllCells();
        foreach (VoronoiCell cell in cells)
        {
            foreach (var vert in cell.BodyVertices)
            {
                CellVertToVector(vert, cell.Translation, out Vector2 p);
                AddDeposit(p, 0f, ResourcePrefab.Prefabs[Rand.Range(0,ResourcePrefab.Prefabs.Count-1)]);
            }
        }
    }
}
