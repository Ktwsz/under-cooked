using System;
using Godot;

public partial class Trash : Table
{
    public override bool TryPlaceItem(Node3D item)
    {
        base.TryPlaceItem(item);

        GetNode<Node3D>("Item").RemoveChild(placedItem);
        placedItem = null;

        return true;
    }
}
