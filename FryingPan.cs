using System;
using Godot;

public partial class FryingPan : Node3D
{
    public Node3D Item;

    private Timer _timer;
    private ProgressBar _progressBar;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerEnded;

        _progressBar = GetNode<ProgressBar>("ProgressBar/ProgressBarViewport/ProgressBar");
    }

    public override void _Process(double delta)
    {
        var timeLeft = _timer.GetTimeLeft();
        var waitTime = _timer.GetWaitTime();

        _progressBar.Value = 100 * (waitTime - timeLeft) / waitTime;
    }

    public void OnTimerEnded()
    {
        _timer.Stop();
        GetNode<Sprite3D>("ProgressBar").SetVisible(false);

        RemoveChild(Item);
        Item = ResourceLoader.Load<PackedScene>("res://cooked_meat.tscn").Instantiate() as Node3D;
        AddChild(Item); // TODO: separate function, set appropriate position?
        Item.SetPosition(new Vector3(0, 0.1f, 0));

        // TDOD: start burning the meat, separate timer?
    }

    public void Add(Node3D tmp)
    {
        if (Item != null || !FoodConstants.IsFood(tmp) || tmp.GetName() != "UncookedMeat")
            return;

        if (tmp is FryingPan fryingPan)
        {
            Add(fryingPan.Item);
            return;
        }

        Item = tmp;
        if (tmp.GetParent() == null)
        {
            AddChild(tmp); // TODO: separate function, set appropriate position?
        }
        else
        {
            if (tmp.GetNode("../..") is Player player)
                player.HeldItem = null;

            if (tmp.GetNode("../..") is Table table)
                table.PlacedItem = null;

            if (tmp.GetParent() is FryingPan fryingPan)
                fryingPan.Item = null;

            tmp.Reparent(this, false); // TODO: separate function, set appropriate position?
        }
        Item.SetPosition(new Vector3(0, 0.1f, 0));
    }

    public void StartFrying()
    {
        if (Item == null || (Item as Node3D).GetName() == "CookedMeat")
            return;

        if (_timer.IsStopped())
            _timer.Start();
        else
            _timer.SetPaused(false);

        GetNode<Sprite3D>("ProgressBar").SetVisible(true);
    }

    public void StopFrying() => _timer.SetPaused(true);
}
