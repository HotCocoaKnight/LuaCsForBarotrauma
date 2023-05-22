
using System;
using System.Collections.Generic;
using System.Timers;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
#if CLIENT
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Barotrauma.Items.Components;

partial class Blueprint : ItemComponent
{
    public class BlueprintData
    {
        public string bpLink = string.Empty;
        public ItemPrefab bpPrefab;

        public BlueprintData(ContentXElement xElement)
        {
            string name = xElement.GetAttributeString("linktobp", "Metal-Grid");
            bpLink = name;
            foreach (ItemPrefab p in ItemPrefab.Prefabs)
            {
                if (p.Name.ToString().ToLower() == bpLink.ToLower())
                {
                    bpPrefab = p;
                }
            }
        }
    }

    public static List<Blueprint> SpawnedBlueprints = new List<Blueprint>();

    public BlueprintData Bp { get; }
    public Item LinkedItem { get; }

    public bool ShowPlacement = false;

    public Character Owner;

    public Vector2 GamePos;

    public override void Equip(Character character)
    {
        ShowPlacement = true;
        Owner = character;
    }

    public override void Unequip(Character character)
    {
        ShowPlacement = false;
        Owner = null;
    }

    public Blueprint(Item item, ContentXElement element) : base(item, element)
    {
        Bp = new BlueprintData(element);
        LinkedItem = item;
        this.item = item;
        SpawnedBlueprints.Add(this);
    }
    
    /*
     * declare all draw functions as client-side
     */

    public override void Update(float deltaTime, Camera cam)
    {
#if CLIENT
        if (PlayerInput.PrimaryMouseButtonClicked() && Owner != null)
        {
            Vector2 newPosition = new Vector2(Owner.CursorWorldPosition.X, Owner.CursorWorldPosition.Y);
            BuildObject building = GetClosestBuild(newPosition, out float distance);
            if (distance < 150.0f)
            {
                GridComponent.GridBlock gb = building.GridArea.GetPoint(Owner.CursorWorldPosition);
                newPosition = gb.ConvertToSimSpace();
                gb.filled = true;
                BuildObject buildObject = new BuildObject(ItemPrefab.GetItemPrefab(Bp.bpLink), newPosition);
            }
            else
            {
                BuildObject buildObject = new BuildObject(ItemPrefab.GetItemPrefab(Bp.bpLink), newPosition);   
            }
        }
#endif
    }

    public static Blueprint AttemptLoad(ContentXElement element, Item parent)
    {
        return new Blueprint(parent, element);
    }
}
