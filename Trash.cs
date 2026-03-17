using System;
using Godot;

public partial class Trash : Table
{
    public override void TryPlaceItem(Node3D item)
    {
        base.TryPlaceItem(item);

        GetNode<Node3D>("Item").RemoveChild(placedItem); // TODO: check for pans and plates
        placedItem = null;
    }
}
