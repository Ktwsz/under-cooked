using System;
using Godot;

public partial class Table : StaticBody3D
{
    [Export]
    public string InitPlacedItem { get; set; } = null;

    private Node3D placedItem = null;

    private void AddItemToScene()
    {
        if (placedItem.GetParent() == null)
            GetNode<Node>("Item").AddChild(placedItem);
        else
            placedItem.Reparent(GetNode<Node>("Item"), false);
        placedItem.SetPosition(new Vector3(0, 0.5f, 0));
    }

    public override void _Ready()
    {
        if (InitPlacedItem != null)
        {
            placedItem = ResourceLoader.Load<PackedScene>(InitPlacedItem).Instantiate() as Node3D;
            placedItem.SetScale(new Vector3(4, 4, 4));
            AddItemToScene();
        }
    }

    public bool tryPlaceItem(Node3D item)
    {
        if (placedItem != null)
            return false;

        placedItem = item;
        AddItemToScene();
        return true;
    }

    public Node3D pickupItem()
    {
        var tmpItem = placedItem;
        placedItem = null;
        return tmpItem;
    }
}
