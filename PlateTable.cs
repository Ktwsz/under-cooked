using System;
using System.Collections;
using Godot;

public partial class PlateTable : Table
{
	private Stack _plates = new Stack();
	private float _stackHeight = 0;
	private Timer _timer;
	private PackedScene _plateScene = ResourceLoader.Load<PackedScene>("res://plate.tscn");

	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
	}

	public override Node3D PickupItem()
	{
		if (_plates.Count == 0)
			return null;

		return PopPlate();
	}

	public override void TryPlaceItem(Node3D item) { }

	private float ExtractSize(Plate plate) =>
		plate.GetNode<MeshInstance3D>("Plate/plate").GetAabb().Size.Y * plate.GetScale().Y;

	private void AddPlate()
	{
		var newPlate = _plateScene.Instantiate() as Plate;
		AddChild(newPlate);
		newPlate.SetPosition(new Vector3(0, _stackHeight, 0));
		_stackHeight += ExtractSize(newPlate) + 0.1f;
		_plates.Push(newPlate);
	}

	private Plate PopPlate()
	{
		var lastPlate = _plates.Pop() as Plate;
		_stackHeight -= ExtractSize(lastPlate) + 0.1f;
		return lastPlate;
	}

	private void OnTimerTimeout()
	{
		if (_plates.Count < 6)
			AddPlate();
	}
}
