using System;
using Godot;

public partial class Table : StaticBody3D
{
    [Export]
    public string InitPlacedItem { get; set; } = null; // TODO: spawner of food

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
            placedItem = ResourceLoader.Load<PackedScene>(InitPlacedItem).Instantiate() as Node3D;
            placedItem.SetScale(new Vector3(4, 4, 4));
            placedItem.SetName("Tomato"); // TODO: spawner of food
            AddItemToScene();
        }
    }

    public bool TryPlaceItem(Node3D item)
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
