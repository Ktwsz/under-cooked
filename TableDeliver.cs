using System;
using Godot;

public partial class TableDeliver : Table
{
    public override void TryPlaceItem(Node3D item)
    {
        if (!(item is Plate))
            return;

        // TODO: check if recipe is wanted, if not return

        PlacedItem = item;
        AddItemToScene();
        foreach (var child in GetNode<Node3D>("Item").GetChildren())
            child.QueueFree();

        // TODO: increase score
    }
}
