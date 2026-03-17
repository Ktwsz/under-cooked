using System;
using Godot;

public partial class FryingTable : Table
{
    public override void TryPlaceItem(Node3D item)
    {
        if (placedItem == null && !(item is FryingPan))
            return;

        base.TryPlaceItem(item);
        if (placedItem != null)
            (placedItem as FryingPan).StartFrying();
    }

    public override Node3D PickupItem()
    {
        (placedItem as FryingPan).StopFrying();
        return base.PickupItem();
    }
}
