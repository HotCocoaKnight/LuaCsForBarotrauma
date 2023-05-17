using System.Collections.Generic;
using Microsoft.Xna.Framework;
#if CLIENT
using System;
using Barotrauma.Items.Components;
using Barotrauma.Lights;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

class Resource : MapEntity
{
    public static List<ResourcePrefab> Prefabs;
    public static List<Resource> Resources;
    public int randScale = 1;
    public Resource(MapEntityPrefab prefab, Sprite sprite, ushort id, Vector2 pos) : base(prefab, null, id)
    {
        this.Position = pos;
        this.Sprite = sprite;
        randScale = Rand.Range(1, 4);
    }

    public Vector2 ScreenPos;
    public Camera cam;

    public override Vector2 Position { get; }
    public override Sprite Sprite { get; }

    public Vector2 GetScreenPos()
    {
        return cam.WorldToScreen(Position);
    }

    public void SetCamera(Camera cam)
    {
        this.cam = cam;
    }
    
    
    #if CLIENT
    public override void Draw(SpriteBatch spriteBatch, bool editing, bool back = true)
    {
        GameMain.LightManager.
        ScreenPos = GetScreenPos();
        Sprite.Draw(spriteBatch, ScreenPos, Color.White, 0.0f, randScale);
        base.Draw(spriteBatch, editing, back);
    }
    #endif

    public override MapEntity Clone()
    {
        throw new System.NotImplementedException();
    }
}
