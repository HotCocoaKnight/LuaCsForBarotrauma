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

        public GridBlock(Vector2 position)
        {
            Position = position;
        }
    }

    protected GridBlock rootBlock;
    
    protected List<GridBlock> blockGrids;

    protected BlockGrid(GridBlock block)
    {
        rootBlock = block;
        rootBlock.Parent = this;
        blockGrids.Add(rootBlock);
    }

    public static BlockGrid CreateGrid(Vector2 position)
    {
        return new BlockGrid(new GridBlock(position));
    }
}

partial class Block : Item
{
    public float durability = 30;
    public Block(ItemPrefab itemPrefab, Vector2 WorldPosition) : base(itemPrefab, WorldPosition, null, Entity.NullEntityID, true)
    {
        
    }

    public Block(Rectangle newRect, ItemPrefab itemPrefab, Submarine submarine, bool callOnItemLoaded = true, ushort id = Entity.NullEntityID) : base(newRect, itemPrefab, submarine, callOnItemLoaded, id)
    {
    }
}
