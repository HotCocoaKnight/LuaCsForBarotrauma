using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework;

namespace Barotrauma;

class ResourcePrefab : MapEntityPrefab
{
    public ResourcePrefab(Identifier identifier) : base(identifier)
    {
    }

    public ResourcePrefab(ContentXElement e, ContentFile file) : base(e, file)
    {
        string name = e.GetAttributeString("name", "un-named");
        int dur = e.GetAttributeInt("dur", 10);
        string path = e.GetAttributeString("sprite", "Content/Resources/Ice.png");
        int width = e.GetAttributeInt("width", 0);
        int height = e.GetAttributeInt("height", 0);
#if CLIENT
        this.Sprite = new Sprite(TextureLoader.FromFile(path), new Rectangle(width/2, height/2, width, height), Vector2.Zero);
#endif
        this.Name = name;
        this.Category = MapEntityCategory.Material;
        this.Scale = Rand.Range(1, 4);
        if (Resource.Prefabs == null)
            Resource.Prefabs = new List<ResourcePrefab>();
        
        Resource.Prefabs.Add(this);
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite Sprite { get; }
    public override string OriginalName { get; }
    public override LocalizedString Name { get; }
    public override ImmutableHashSet<Identifier> Tags { get; }
    public override ImmutableHashSet<Identifier> AllowedLinks { get; }
    public override MapEntityCategory Category { get; }
    public override ImmutableHashSet<string> Aliases { get; }
    protected override void CreateInstance(Rectangle rect)
    {
        throw new System.NotImplementedException();
    }
}
