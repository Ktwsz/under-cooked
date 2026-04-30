using System;
using Godot;

public partial class FryingTable : Table
{
	public override void TryPlaceItem(Node3D item)
	{
		if (PlacedItem == null && !(item is FryingPan))
			return;

		base.TryPlaceItem(item);
		if (PlacedItem != null)
			(PlacedItem as FryingPan).StartFrying();
	}

	public override Node3D PickupItem()
	{
		if (PlacedItem == null)
			return null;

		(PlacedItem as FryingPan).StopFrying();
		return base.PickupItem();
	}
}
