using System;
using System.Collections.Generic;
using Barotrauma.Items.Components;
using Barotrauma.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma;

class DynamicSubmarine : Entity, IServerPositionSync, IDrawableComponent
{
    public static List<DynamicSubmarine> DynamicObjects = new List<DynamicSubmarine>();
    public DynamicSubmarine(Sprite objectSprite, Vector2 Position) : base(null, (ushort)Rand.Range(0, 200))
    {
        this.Position = Position;
        this.objectSprite = objectSprite;
        DynamicObjects.Add(this);
    }

    public Sprite objectSprite;

    public override Vector2 Position { get; }

    public void ClientEventRead(IReadMessage msg, float sendingTime)
    {
        throw new NotImplementedException();
    }

    public void ClientReadPosition(IReadMessage msg, float sendingTime)
    {
        throw new NotImplementedException();
    }

    public Vector2 DrawSize { get; }
    public void Draw(SpriteBatch spriteBatch, bool editing, float itemDepth = -1)
    {
        objectSprite.Draw(spriteBatch, DrawPosition, Color.White);
    }
}
