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
        if (PlacedItem != null)
        {
            if (item is FryingPan)
            { // bun or plate...
                if (PlacedItem is Bun)
                {
                    (PlacedItem as Bun).Add((item as FryingPan).GetItem());
                    return;
                }
                (item as FryingPan).Add(PlacedItem);
            }
            if (PlacedItem is FryingPan)
            {
                (PlacedItem as FryingPan).Add(item);
                return;
            }

            return;
        }

        PlacedItem = item;
        AddItemToScene();
    }

    public virtual Node3D PickupItem()
    {
        var tmpItem = PlacedItem;
        PlacedItem = null;
        return tmpItem;
    }
}
