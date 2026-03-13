using System;
using Godot;

public partial class CuttingTable : Table
{
    private Timer timer;
    private ProgressBar progressBar;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerEnded;

        progressBar = GetNode<ProgressBar>("CuttingProgressBar/ProgressBarViewport/ProgressBar");
    }

    public override void _Process(double delta)
    {
        var timeLeft = timer.GetTimeLeft();
        var waitTime = timer.GetWaitTime();

        progressBar.Value = 100 * (waitTime - timeLeft) / waitTime;
    }

    public override Node3D PickupItem()
    {
        if (!timer.IsStopped())
            return null;

        return base.PickupItem();
    }

    public void OnTimerEnded()
    {
        timer.Stop();
        GetNode<Sprite3D>("CuttingProgressBar").SetVisible(false);

        GetNode<Node3D>("Item").RemoveChild(placedItem);
        var itemName = (placedItem as Food).GetFoodName();
        placedItem =
            ResourceLoader.Load<PackedScene>(FoodConstants.CuttableFood[itemName]).Instantiate()
            as Node3D;
        AddItemToScene();
    }

    public override bool CanInteract() =>
        placedItem != null
        && placedItem is Food
        && FoodConstants.CuttableFood.ContainsKey((placedItem as Food).GetFoodName());

    public override void StartInteract()
    {
        if (timer.IsStopped())
        {
            timer.Start();
        }
        else
        {
            timer.SetPaused(false);
        }
        GetNode<Sprite3D>("CuttingProgressBar").SetVisible(true);
    }

    public override void StopInteract()
    {
        timer.SetPaused(true);
    }

    public override Timer GetTimer() => timer;
}
