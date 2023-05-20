using System.Collections.Generic;
using System.Xml.Linq;
using Barotrauma.Items.Components;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
#if CLIENT
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma;

class PhysicsEntity : Entity
{
    public Body body;
    private Transform WorldTransform;

    public virtual void Awake()
    {
        
    }
    
    public PhysicsEntity(Vector2 position, float width, float height) : base(null, (ushort)FindFreeIdBlock(1))
    {
        body = new Body();
        body.Position = position;
        body.BodyType = BodyType.Dynamic;
        List<Vector2> vertices = new List<Vector2>();
        vertices.Add(new Vector2(position.X - width, position.Y - height));
        vertices.Add(new Vector2(position.X + width, position.X - height));
        vertices.Add(new Vector2(position.X - width, position.Y + height));
        vertices.Add(new Vector2(position.X - width, position.Y - height));
        body.FixtureList.Add(new Fixture(new PolygonShape(new Vertices(vertices), 1.0f), Category.All, Category.All));
        WorldTransform = new Transform(position, 0f);
    }
}

class BuildObject : PhysicsEntity
{
    public static List<BuildObject> SpawnedBuildings = new List<BuildObject>();

    public List<BuildObject> Linked;
    
    public string DisplayName;
    public Sprite Sprite;
    public int durability;

#if SERVER
    private Character BuildingOwner;

    public virtual void SetOwner(Character Replacement)
    {
        BuildingOwner = Replacement;
    }
#endif

#if CLIENT

    public virtual void DrawObject(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, body.Position, Color.White, body.Rotation, 1f);
    }
    
#endif

    public BuildObject(Vector2 WorldPosition, BuildObjectPrefab bop) : base(WorldPosition, bop.ObjectSprite.size.X, bop.ObjectSprite.size.Y)
    {
        Linked = new List<BuildObject>();
        this.DisplayName = bop.ObjectName;
        this.durability = bop.dur;
        this.Sprite = bop.ObjectSprite;
        SpawnedBuildings.Add(this);
    }
}

class BuildObjectPrefab
{
    public static List<BuildObjectPrefab> Prefabs = new List<BuildObjectPrefab>();
    public Sprite ObjectSprite;
    public string ObjectName = string.Empty;
    public int dur;


    public BuildObjectPrefab(XElement element)
    {
#if CLIENT
        Texture2D texture2D = TextureLoader.FromFile(element.GetAttributeString("sprite", "none"));
        ObjectSprite = new Sprite(texture2D, new Rectangle(0, 0, (int)texture2D.Width, (int)texture2D.Height), null);
#endif        
        ObjectName = element.GetAttributeString("name", "default");
        dur = element.GetAttributeInt("dur", 10);
        Prefabs.Add(this);
    }
}

