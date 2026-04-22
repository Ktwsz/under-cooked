using System;
using Godot;

public partial class CuttingTable : Table
{
    private Timer _timer;
    public Timer Timer => _timer;
    private ProgressBar _progressBar;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerEnded;

        _progressBar = GetNode<ProgressBar>("CuttingProgressBar/ProgressBarViewport/ProgressBar");
    }

    public override void _Process(double delta)
    {
        var timeLeft = _timer.GetTimeLeft();
        var waitTime = _timer.GetWaitTime();

        _progressBar.Value = 100 * (waitTime - timeLeft) / waitTime;
    }

    public override Node3D PickupItem()
    {
        if (!_timer.IsStopped())
            return null;

        return base.PickupItem();
    }

    public void OnTimerEnded()
    {
        _timer.Stop();
        GetNode<Sprite3D>("CuttingProgressBar").SetVisible(false);

        GetNode<Node3D>("Item").RemoveChild(PlacedItem);
        var itemName = PlacedItem.GetName();
        PlacedItem =
            ResourceLoader.Load<PackedScene>(FoodConstants.CuttableFood[itemName]).Instantiate()
            as Node3D;
        AddItemToScene();
    }

    public bool CanCut() =>
        PlacedItem != null && FoodConstants.CuttableFood.ContainsKey(PlacedItem.GetName());

    public void StartInteract()
    {
        if (_timer.IsStopped())
            _timer.Start();
        else
            _timer.SetPaused(false);

        GetNode<Sprite3D>("CuttingProgressBar").SetVisible(true);
    }

    public void StopInteract() => _timer.SetPaused(true);
}
