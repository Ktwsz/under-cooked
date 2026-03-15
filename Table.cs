using System;
using Godot;

public partial class Table : StaticBody3D
{
    [Export]
    public PackedScene InitPlacedItem;

    protected Node3D placedItem = null;

    protected void AddItemToScene()
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
            placedItem = InitPlacedItem.Instantiate() as Node3D;
            AddItemToScene();
        }
    }

    public virtual bool TryPlaceItem(Node3D item)
    {
        if (placedItem != null)
            return false;

        placedItem = item;
        AddItemToScene();
        return true;
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

    public virtual Timer GetTimer() => null;
}
