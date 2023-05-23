using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
#if CLIENT
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

partial class Block
{
    public string Name,
        Description;

    private int durability = 0;
    public Sprite Sprite;

    public float Scale;
    
    public Block(XElement element)
    {
        Name = element.GetAttributeString("Name", "no-name");
        Description = element.GetAttributeString("description", "");
        durability = element.GetAttributeInt("durability", 10); // max should be 100
        foreach (var e in element.Elements())
        {
            switch (e.Name.ToString().ToLower())
            {
                case "sprite":
                    string path = element.GetAttributeString("path", "");
                    Texture2D texture = TextureLoader.FromFile(path);
                    Sprite = new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height),
                        new Vector2(texture.Width / 2, texture.Height / 2), 0f, path);
                    break;
                default:
                    LuaCsLogger.Log("unimplemented component in xml: " + e.Name + ", type does not exist");
                    break;
            }
        }
    }
}

partial class Building : Submarine
{
    public static Building CreateBuilding(Block startBlock)
    {
        return null;
    }
    
    private Building(SubmarineInfo info, bool showErrorMessages = true, Func<Submarine, List<MapEntity>> loadEntities = null, IdRemap linkedRemap = null) : base(info, showErrorMessages, loadEntities, linkedRemap)
    {
    }
}
