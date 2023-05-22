using System;
using System.Collections.Generic;
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
        
        BuildObject closestBuilding = GetClosestBuild(Owner.CursorWorldPosition, out float distance);
        Vector2 drawPos = new Vector2(Owner.CursorWorldPosition.X, -Owner.CursorWorldPosition.Y);
        if (distance < 150.0f)
        {
            drawPos = closestBuilding.GridArea.GetPoint(Owner.CursorWorldPosition).position;
            closestBuilding.GridArea.Draw(spriteBatch, this.item.Scale);
        }
        Bp.bpPrefab.Sprite.Draw(spriteBatch, drawPos, new Color(150, 150, 255, 100), 0f, this.item.Scale, SpriteEffects.None, 0.0f);
        Update(0, camera);
    }
}
