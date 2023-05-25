using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma;

partial class BlockGrid
{
    public Sprite GridSprite;
    public virtual void Draw(SpriteBatch batch)
    {
        GridSprite.Draw(batch, new Vector2(rootBlock.Position.X, -rootBlock.Position.Y), 0f, Scale, SpriteEffects.None);
    }
}
