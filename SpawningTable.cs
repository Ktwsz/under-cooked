using System;
using Godot;

public partial class SpawningTable : Table
{
    [Export]
    public PackedScene spawnedFood;

    public override Node3D PickupItem()
    {
        if (placedItem != null)
        {
            return base.PickupItem();
        }

        return spawnedFood.Instantiate() as Node3D;
    }
}
