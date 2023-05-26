using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace Barotrauma.Items.Components;

partial class Constructor : ItemComponent
{
    private bool bpActive;
    public ItemPrefab ActiveBlueprint;
    public Character Owner;
    public static List<Constructor> WorldDraws = new List<Constructor>();
    
    public Constructor(Item item, ContentXElement element) : base(item, element)
    {
        item.Components.AddItem(this);
    }
    
    public bool GetLocalPlayer(out Character character)
    {
        character = Character.Controlled;
        return character != null;
    }

    public override void Equip(Character character)
    {
        WorldDraws.Add(this);
        bpActive = true;
        Owner = character;
    }

    public override void Unequip(Character character)
    {
        bpActive = false;

        WorldDraws.Remove(this);
    }
}
