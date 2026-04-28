using System;
using Godot;

public partial class Table : StaticBody3D
{
    [Export]
    public PackedScene InitPlacedItem;

    public Node3D PlacedItem;

    protected void AddItemToScene()
    {
        if (PlacedItem.GetParent() == null)
        {
            GetNode<Node>("Item").AddChild(PlacedItem);
        }
        else
        {
            if (PlacedItem.GetNode("../..") is Player)
                (PlacedItem.GetNode("../..") as Player).HeldItem = null;

            PlacedItem.Reparent(GetNode<Node>("Item"), false);
        }
        PlacedItem.SetPosition(new Vector3(0, 0.5f, 0));
    }

    public override void _Ready()
    {
        if (InitPlacedItem != null)
        {
            PlacedItem = InitPlacedItem.Instantiate() as Node3D;
            AddItemToScene();
        }
    }

    public virtual void TryPlaceItem(Node3D item)
    {
        if (PlacedItem == null)
        {
            PlacedItem = item;
            AddItemToScene();
            return;
        }

        if (item is FryingPan fryingPan)
        {
            if (PlacedItem is Bun placedBun)
                placedBun.Add(item);
            else if (PlacedItem is Plate placedPlate)
                placedPlate.Add(item);
            else
                fryingPan.Add(PlacedItem);
        }
        else if (item is Bun bun)
        {
            if (PlacedItem is Plate placedPlate)
                placedPlate.Add(item);
            else
                bun.Add(PlacedItem);
        }
        else if (item is Plate plate)
        {
            plate.Add(PlacedItem);
        }
        else
        {
            if (PlacedItem is FryingPan placedFryingPan)
                placedFryingPan.Add(item);
            else if (PlacedItem is Bun placedBun)
                placedBun.Add(item);
            else if (PlacedItem is Plate placedPlate)
                placedPlate.Add(item);
        }
    }

    public virtual Node3D PickupItem()
    {
        var tmpItem = PlacedItem;
        PlacedItem = null;
        return tmpItem;
    }
}
