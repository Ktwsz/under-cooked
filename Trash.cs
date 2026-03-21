using System;
using Godot;

public partial class Trash : Table
{
    public override void TryPlaceItem(Node3D item)
    {
        if (item is FryingPan fryingPan)
        {
            TryPlaceItem(fryingPan.Item);
            return;
        }

        if (item is Plate)
        {
            foreach (var child in item.GetNode<Node3D>("Items").GetChildren())
            {
                TryPlaceItem(child as Node3D);
            }
            return;
        }

        base.TryPlaceItem(item);

        GetNode<Node3D>("Item").RemoveChild(PlacedItem); // TODO: check for pans and plates
        PlacedItem = null;
    }
}
