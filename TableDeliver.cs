using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TableDeliver : Table
{
	public override void TryPlaceItem(Node3D item)
	{
		if (!(item is Plate))
			return;

		var orders = (Godot.Collections.Array<PanelContainer>)
			GetNode<MarginContainer>("../../LevelUI/Orders").Get("order_items");

		var orderIx = OrderExists(item as Plate, orders);

		if (orderIx == -1)
			return;

		PlacedItem = item;
		AddItemToScene();
		foreach (var child in GetNode<Node3D>("Item").GetChildren())
			child.QueueFree();

		GetNode<CanvasLayer>("../../LevelUI")
			.Call("increase_score", orders[orderIx].Call("get_score"));

		orders[orderIx].Set("is_completed", true);
	}

	private int OrderExists(Plate plate, Godot.Collections.Array<PanelContainer> orders)
	{
		var plateList = ExtractPlate(plate);
		for (int i = 0; i < orders.Count; ++i)
		{
			var order = orders[i];

			if (
				plateList.SequenceEqual(
					ExtractRecipe((Godot.Collections.Array<String>)order.Get("ingredient_names"))
				)
			)
				return i;
		}
		return -1;
	}

	private List<string> ExtractRecipe(Godot.Collections.Array<String> recipe)
	{
		var result = new List<string>();
		foreach (var element in recipe)
		{
			result.Add(MapIngredientName(element));
		}
		result.Sort();
		return result;
	}

	private List<string> ExtractPlate(Plate plate)
	{
		var result = new List<string>();
		if (plate.Bun != null)
		{
			result.Add("Bun");

			foreach (var name in plate.Bun.GetContentsNames())
			{
				result.Add(name);
			}
		}
		else
		{
			foreach (var child in GetNode<Node3D>("Items").GetChildren())
				result.Add(child.GetName());
		}

		result.Sort();
		return result;
	}

	private string MapIngredientName(String ingredient) =>
		ingredient switch
		{
			"Bread" => "Bun",
			"MeatPattyCooked" => "CookedMeat",
			"Cabbage" => "CabbageSliced",
			"Tomato" => "TomatoSliced",
			"" => "",
		};
}
