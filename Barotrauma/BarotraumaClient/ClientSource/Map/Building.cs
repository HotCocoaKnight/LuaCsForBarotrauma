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
            GridSprite.Draw(batch, new Vector2(StoredBlock.WorldPosition.X + block.Position.X, -(block.Position.Y + StoredBlock.WorldPosition.Y)), 0f, Scale, SpriteEffects.None);
        }
        GridSprite.Draw(batch, new Vector2((rootBlock.Position.X + StoredBlock.WorldPosition.X), -(rootBlock.Position.Y + StoredBlock.WorldPosition.Y)), 0f, Scale, SpriteEffects.None);
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
        base.Draw(spriteBatch, editing, back);
    }
}
