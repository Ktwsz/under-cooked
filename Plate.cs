using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Plate : Node3D
{
	private Bun _bun;
	public Bun Bun
	{
		get => _bun;
	}

	private float _currentPos;

	private float ExtractSize(Node3D node) =>
		(node.GetChildren()[0] as MeshInstance3D).GetAabb().Size.Y * node.GetScale().Y;

	public override void _Ready()
	{
		_currentPos = ExtractSize(GetNode<Node3D>("Plate"));
	}

	private bool IsAllowedIngredient(string name) =>
		(name == "CookedMeat" || name == "TomatoSliced" || name == "CabbageSliced" || name == "Bun")
		&& GetNode<Node3D>("Items").GetChildren().All(child => name != child.GetName());

	public void Add(Node3D tmp)
	{
		if (tmp == null)
			return;

		if (_bun != null)
		{
			_bun.Add(tmp);
			UpdateShownIngredients();
			return;
		}
		if (tmp is FryingPan fryingPan)
		{
			Add(fryingPan.Item);
			return;
		}

		if (!(IsAllowedIngredient(tmp.GetName())))
			return;

		Reparent(tmp);
		HideItemIndicator(tmp);

		if (tmp is Bun)
		{
			_bun = tmp as Bun;
			_bun.HideRecipeIndicator();
			foreach (var child in GetNode<Node3D>("Items").GetChildren())
			{
				if (child is Bun)
					continue;

				_bun.Add(child as Node3D);
			}
			_bun.SetPosition(new Vector3(0, 0.1f, 0));
		}
		else
		{
			tmp.SetPosition(new Vector3(0, _currentPos, 0));
			_currentPos += ExtractSize(tmp.GetChildren()[0] as Node3D);
		}

		UpdateShownIngredients();
	}

	private void Reparent(Node3D tmp)
	{
		if (tmp.GetParent() == null)
		{
			GetNode<Node3D>("Items").AddChild(tmp);
			return;
		}

		if (tmp.GetNode("../..") is Player player)
			player.HeldItem = null;

		if (tmp.GetNode("../..") is Table table)
			table.PlacedItem = null;

		if (tmp.GetParent() is FryingPan)
			(tmp.GetParent() as FryingPan).Item = null;

		tmp.Reparent(GetNode<Node3D>("Items"), false);
	}

	private void UpdateShownIngredients()
	{
		IEnumerable<string> children;
		if (_bun != null)
		{
			GetNode<TextureRect>("Indicator/SubViewport/HBoxContainer/BunRect").SetVisible(true);
			children = _bun.GetContentsNames();
		}
		else
		{
			children = GetNode<Node3D>("Items").GetChildren().Select(c => c.GetName().ToString());
		}

		foreach (var nodeName in children)
		{
			GetNode<TextureRect>(
					$"Indicator/SubViewport/HBoxContainer/{IngredientToRectName(nodeName)}"
				)
				.SetVisible(true);
		}
	}

	private string IngredientToRectName(string nodeName) =>
		nodeName switch
		{
			"CookedMeat" => "MeatRect",
			"TomatoSliced" => "TomatoRect",
			"CabbageSliced" => "CabbageRect",
			_ => "",
		};

	private void HideItemIndicator(Node3D item)
	{
		item.GetNode<Sprite3D>("Indicator").SetVisible(false);
	}
}
