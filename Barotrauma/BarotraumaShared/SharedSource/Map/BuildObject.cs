using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Barotrauma.Items.Components;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter.Compatibility;
using Steamworks.ServerList;
#if CLIENT
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

public class GridComponent
{
    public class GridBlock
    {
        public Vector2 position;
        public bool filled;

        private BuildObject filledWith;

        public Vector2 ConvertToSimSpace()
        {
            return new Vector2(position.X, -position.Y);
        }
        
        public GridBlock(Vector2 position)
        {
            this.position = position;
        }

        public GridBlock(float x, float y)
        {
            this.position = new Vector2(x, y);
        }
    }
    public Vector2 Position;
    private List<GridBlock> points = new List<GridBlock>();
    private Sprite Sprite;

    public GridBlock GetCenter()
    {
        return points[points.Count / 2 - 1];
    }

    public int GetCount()
    {
        return points.Count;
    }

    public GridBlock GetPoint(Vector2 target)
    {
        GridBlock returnBlock = null;
        float lastdist = float.PositiveInfinity;
        foreach (var p in points)
        {
            float dist = Vector2.Distance(target, p.position);
            if (dist < lastdist && p.filled == false)
            {
                lastdist = dist;
                returnBlock = p;
            }
        }
        return returnBlock;
    }

#if CLIENT
    public virtual void Draw(SpriteBatch batch, float scale)
    {
        foreach (GridBlock edge in points)
        {
            Sprite.Draw(batch, Position + edge.position, Color.White, Sprite.size/2, 0f, scale);
        }
    }
#endif
    public GridComponent(Vector2 worldPosition, string grid_path)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                points.Add(new GridBlock(i * 64, j * 64));
                points.Add(new GridBlock(-i * 64, -j * 64));
                points.Add(new GridBlock(-i * 64, j * 64));
                points.Add(new GridBlock(i * 64, -j * 64));
            }
            points.Add(new GridBlock(i * 64, 0));
            points.Add(new GridBlock(-i * 64, 0));

        }

        points[0].filled = true;
            this.Position = worldPosition;
#if CLIENT
        Sprite = new Sprite(TextureLoader.FromFile(grid_path), new Rectangle(0, 0, 64, 64), Vector2.Zero, 0f,
            grid_path);
#endif
    }
}

partial class BuildObject : Item
{
    public Character Owner { get; }

    public static List<BuildObject> SpawnedBuildings = new List<BuildObject>();

    public GridComponent GridArea;

    public BuildObject(ItemPrefab itemPrefab, Vector2 position) : base(itemPrefab, position, null, (ushort)BuildObject.FindFreeIdBlock(1), true)
    {
        FreeID();
        GridArea = new GridComponent(position, "Content/BuildObjects/snap_grid.png");
        GridArea.Position = body.Position;
        SpawnedBuildings.Add(this);
    }

    public BuildObject(Rectangle newRect, ItemPrefab itemPrefab, Submarine submarine, bool callOnItemLoaded = true, ushort id = Entity.NullEntityID) : base(newRect, itemPrefab, submarine, callOnItemLoaded, id)
    {
    }
}

