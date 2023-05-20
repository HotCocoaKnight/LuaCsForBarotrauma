using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Barotrauma.Items.Components;
using Barotrauma.Networking;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#if CLIENT
using FarseerPhysics;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

partial class ResourcePrefab
{
    #if CLIENT
    public Sprite ObjectSprite;
    #endif
    public float width;
    public float height;
    public static List<ResourcePrefab> Prefabs;
    public ResourcePrefab(XElement resourceElement)
    {
        width = (float)resourceElement.GetAttributeInt("width", 300);
        height = (float)resourceElement.GetAttributeInt("height", 300);
#if CLIENT
        Texture2D t = TextureLoader.FromFile(resourceElement.GetAttributeString("sprite", "no-path"));
        ObjectSprite = new Sprite(t, new Rectangle(0, 0, (int)width, (int)height), new Vector2(-(width/2), -(height/2)));
#endif
    }
}


partial class Resource : Entity, IDrawableComponent, IServerPositionSync
{
    private Vector2 spritePosition;
    public Vector2 Scale;
    public Sprite Sprite;
    public override Vector2 Position { get; }
    public static List<Resource> LoadedResources;

    public float angle;
    
    public void SetPosition(Vector2 pos)
    {
        spritePosition = pos;
    }
    
    public Resource(ResourcePrefab prefab, Vector2 position, float angle) : base(null, (ushort)Resource.FindFreeIdBlock(1))
    {
        this.Position = position;
        this.angle = angle;
        this.Scale = new Vector2(prefab.width, prefab.height);
#if CLIENT
      this.Sprite = prefab.ObjectSprite;
        spritePosition = position;
#endif
        if (LoadedResources == null)
        {
            LoadedResources = new List<Resource>();
        }
        LoadedResources.Add(this);
    }
    
#if CLIENT
    public Vector2 DrawSize { get; }
    
    public void Draw(SpriteBatch spriteBatch, bool editing, float itemDepth = -1)
    {
        Sprite.Draw(spriteBatch, spritePosition, Color.White, angle);
    }
#endif
    
    public void ClientEventRead(IReadMessage msg, float sendingTime)
    {
        
    }

    public void ClientReadPosition(IReadMessage msg, float sendingTime)
    {
        spritePosition = new Vector2(msg.ReadSingle(), msg.ReadSingle());
    }
    public void ServerWritePosition(ReadWriteMessage a, Client b)
    {
        a.WriteSingle(spritePosition.X);
        a.WriteSingle(spritePosition.Y);
    }
    public void ServerEventWrite(IWriteMessage a, Client b, NetEntityEvent.IData idata)
    {
    }
}
