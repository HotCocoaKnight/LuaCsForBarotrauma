using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma.Items.Components;

partial class Blueprint
{
    public virtual BuildObject GetClosestBuild(Vector2 position, out float distance)
    {
        BuildObject buildObject = null;
        float lastDist = float.PositiveInfinity;
        foreach (var vr in BuildObject.SpawnedBuildings)
        {
            float dist = Vector2.Distance(vr.Position, position);
            if (dist < lastDist)
            {
                buildObject = vr;
                lastDist = dist;
            }
        }

        distance = lastDist;
        return buildObject;
    }
    
    public virtual void DrawPlacement(SpriteBatch spriteBatch, Camera camera)
    {
        if (!ShowPlacement || Bp.bpPrefab == null)
            return;

        Vector2 drawpoint = new Vector2(Owner.CursorWorldPosition.X, -Owner.CursorWorldPosition.Y);

        LuaCsLogger.Log("previous draw" + drawpoint.ToString());
        
        BuildObject ClosestObject = GetClosestBuild(Owner.CursorPosition, out float dist);
        if (dist < 128.0f)
        {
            GridComponent.GridBlock block = ClosestObject.GridArea.GetPoint(Owner.CursorWorldPosition);
            Vector2 gridPos = ClosestObject.DrawPosition;
            drawpoint = new Vector2(gridPos.X, -gridPos.Y);
        }

        Bp.bpPrefab.Sprite.Draw(spriteBatch, drawpoint, new Color(150, 150, 255, 100), 0f, this.item.Scale, SpriteEffects.None, 0.0f);
        Update(0, camera);
    }
}
