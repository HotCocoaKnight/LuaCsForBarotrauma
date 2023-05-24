using System.Linq;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barotrauma.Items.Components;

partial class Constructor : ItemComponent
{
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
        DrawBlock(spriteBatch, Owner, camera);
    }
}
