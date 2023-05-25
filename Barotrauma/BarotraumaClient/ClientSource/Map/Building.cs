using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma;

partial class BlockGrid
{
    public Sprite GridSprite;
    public virtual void Draw(SpriteBatch batch)
    {
        if (GridSprite == null)
            return;
        foreach (var block in blocks)
        {
            GridSprite.Draw(batch, new Vector2(block.Position.X, -block.Position.Y), 0f, Scale, SpriteEffects.None);
        }
        GridSprite.Draw(batch, new Vector2(rootBlock.Position.X, -rootBlock.Position.Y), 0f, Scale, SpriteEffects.None);
    }
}

partial class Block : Item
{
    public bool GetLocalPlayer(out Character character)
    {
        character = Character.Controlled;
        return character != null;
    }
    
    public override void Draw(SpriteBatch spriteBatch, bool editing, bool back = true)
    {
        if (GetLocalPlayer(out Character mainPlayer))
        {
            Vector2 point = blockGrid.GetValidPoint(mainPlayer.CursorWorldPosition, out BlockGrid parent, out float dist);
            if (!float.IsPositiveInfinity(dist) && dist <= 160f)
            {
                LuaCsLogger.Log("draw");
                blockGrid.Draw(spriteBatch);
            }
        }
        base.Draw(spriteBatch, editing, back);
    }
}
