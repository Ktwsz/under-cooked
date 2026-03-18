using System;
using Godot;

public partial class SpawningTable : Table
{
    [Export]
    public PackedScene SpawnedFood;

    public override Node3D PickupItem()
    {
        if (PlacedItem != null)
            return base.PickupItem();

        return SpawnedFood.Instantiate() as Node3D;
    }
}
