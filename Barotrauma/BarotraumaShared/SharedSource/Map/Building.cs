using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Barotrauma.Networking;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using HarmonyLib;
using Microsoft.Xna.Framework;
#if CLIENT
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

partial class BlockPrefab
{
    public string Name,
        Description;

    public int durability = 0;
    public Sprite Sprite;

    public float Scale;

    public static List<BlockPrefab> Prefabs = new List<BlockPrefab>();
    public static List<ItemPrefab> ItemPrefabs = new List<ItemPrefab>();

    public XElement element;

    public BlockPrefab(XElement element)
    {
        this.element = element;
        Name = element.GetAttributeString("name", "no-name");
        Description = element.GetAttributeString("description", "");
        durability = element.GetAttributeInt("durability", 10); // max should be 100
        Scale = element.GetAttributeFloat("scale", 0.1f);
        foreach (var e in element.Elements())
        {
            switch (e.Name.ToString().ToLower())
            {
                case "sprite":
                    string path = e.GetAttributeString("path", "");
                    #if CLIENT
                    Texture2D texture = TextureLoader.FromFile(path);
                    Sprite = new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height),
                        new Vector2(texture.Width, texture.Height) / 2, 0f, path);
                    #endif
                    break;
                default:
                    LuaCsLogger.Log("unimplemented component in xml: " + e.Name + ", type does not exist");
                    break;
            }
        }
    }
}

partial class BlockGrid
{
    protected class GridBlock
    {
        public Vector2 Position;
        public BlockGrid Parent;

        protected Block filled;

        public static GridBlock Empty = new GridBlock(new Vector2(float.PositiveInfinity, float.PositiveInfinity));

        public Block getBlock()
        {
            return filled;
        }
        
        public GridBlock(Vector2 position)
        {
            Position = position;
        }
    }

    protected GridBlock rootBlock;
    
    protected List<GridBlock> blocks;

    public Vector2 GetValidPoint(Vector2 Point, out BlockGrid parent, bool setFilled = true)
    {
        GridBlock Block = GridBlock.Empty;
        float lastDist = float.PositiveInfinity;
        foreach (GridBlock block in blocks)
        {
            Vector2.Distance(ref Point, ref block.Position, out float dist);
            if (dist < lastDist)
            {
                dist = lastDist;
                Block.Position = block.Position;
            }
        }

        parent = Block.Parent;
        return Block.Position;
    }

    protected void CreateGridBlock(Vector2 pos)
    {
        blocks.Add(new GridBlock(pos));
    }
    
    protected BlockGrid(GridBlock block)
    {
        rootBlock = block;
        rootBlock.Parent = this;
        blocks = new List<GridBlock>();
        blocks.Add(rootBlock);
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                CreateGridBlock(new Vector2(-x,y));
                CreateGridBlock(new Vector2(x,-y));
                CreateGridBlock(new Vector2(-x,-y));
                CreateGridBlock(new Vector2(x,y));
            }
            CreateGridBlock(new Vector2(x,0));
            CreateGridBlock(new Vector2(-x,0));
        }
    }

    public static BlockGrid CreateGrid(Vector2 position)
    {
        return new BlockGrid(new GridBlock(position));
    }
}

partial class Block : Item
{
    protected BlockGrid blockGrid;

    public BlockGrid GetGrid()
    {
        return blockGrid;
    }
    
    public Block(ItemPrefab itemPrefab, Vector2 WorldPosition) : base(itemPrefab, WorldPosition, null, Entity.NullEntityID, true)
    {
        blockGrid = BlockGrid.CreateGrid(WorldPosition);
    }

    public Block(Rectangle newRect, ItemPrefab itemPrefab, Submarine submarine, bool callOnItemLoaded = true, ushort id = Entity.NullEntityID) : base(newRect, itemPrefab, submarine, callOnItemLoaded, id)
    {
    }
}
