namespace Barotrauma;

class Resource : MapEntity
{
    public Resource(MapEntityPrefab prefab, Submarine submarine, ushort id) : base(prefab, submarine, id)
    {
        
    }

    public override MapEntity Clone()
    {
        return null;
    }
}
