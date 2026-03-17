using System;
using Godot;

public partial class Table : StaticBody3D
{
    [Export]
    public PackedScene InitPlacedItem;

    public Node3D placedItem = null;

    protected void AddItemToScene()
    {
        if (placedItem.GetParent() == null)
        {
            GetNode<Node>("Item").AddChild(placedItem);
        }
        else
        {
            if (placedItem.GetNode("../..") is Player)
            {
                (placedItem.GetNode("../..") as Player).SetHeldItem(null);
            }
            placedItem.Reparent(GetNode<Node>("Item"), false);
        }
        placedItem.SetPosition(new Vector3(0, 0.5f, 0));
    }

    public override void _Ready()
    {
        if (InitPlacedItem != null)
        {
            placedItem = InitPlacedItem.Instantiate() as Node3D;
            AddItemToScene();
        }
    }

    public virtual void TryPlaceItem(Node3D item)
    {
        if (placedItem != null)
        {
            if (item is FryingPan)
            { // bun or plate...
                if (placedItem is Bun)
                {
                    (placedItem as Bun).Add((item as FryingPan).GetItem());
                    return;
                }
                (item as FryingPan).Add(placedItem);
            }
            if (placedItem is FryingPan)
            {
                (placedItem as FryingPan).Add(item);
                return;
            }

            return;
        }

        placedItem = item;
        AddItemToScene();
    }

    public virtual Node3D PickupItem()
    {
        var tmpItem = placedItem;
        placedItem = null;
        return tmpItem;
    }

    public virtual bool CanInteract() => false;

    public virtual void StartInteract() { }

    public virtual void StopInteract() { }

    public virtual Timer GetTimer() => null; // TODO: remove this, only cutting table has interactions
}
