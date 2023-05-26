using System.Linq;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma.Items.Components;

partial class Constructor : ItemComponent
{
    public Block GetNearestBlock(out bool anyBlocks)
    {
        if (Block.RegisteredBlocks.Any() && Character.Controlled != null)
        {
            Block returnBlock = null;
            float lastDistance = float.PositiveInfinity;
            foreach (var block in Block.RegisteredBlocks)
            {
                float dist = Vector2.Distance(block.Position, Character.Controlled.WorldPosition);
                if (dist < lastDistance)
                {
                    lastDistance = dist;
                    returnBlock = block;
                }
            }

            anyBlocks = true;
            return returnBlock;
        }

        anyBlocks = false;
        return null;
    }
    
    public virtual void DrawBlock(SpriteBatch spriteBatch, Character character, Camera camera)
    {
        ActiveBlueprint.Sprite.Draw(spriteBatch, new Vector2(Owner.CursorWorldPosition.X, -Owner.CursorWorldPosition.Y), Color.White, Vector2.Zero, 0f, ActiveBlueprint.Scale);
        if (PlayerInput.PrimaryMouseButtonClicked())
        {
            new Block(ItemPrefab.GetItemPrefab(ActiveBlueprint.Identifier.ToString().ToLower()), Owner.CursorWorldPosition); // we don't have an entity spawner for the custom class so we have to do it this way
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        ActiveBlueprint = ItemPrefab.GetItemPrefab("sg");
        if (GetLocalPlayer(out Character mainPlayer))
        {
            Block block = GetNearestBlock(out bool anyBlocks);
            if (anyBlocks)
            {
                Vector2 point = block.GetGrid().GetValidPoint(mainPlayer.CursorWorldPosition, out BlockGrid parent, out float dist);
                if (!float.IsPositiveInfinity(dist) && dist <= 160f)
                {
                    LuaCsLogger.Log("draw");
                    block.GetGrid().Draw(spriteBatch);
                }
                LuaCsLogger.Log(dist.ToString());   
            }
        }
        DrawBlock(spriteBatch, Owner, camera);
    }
}
