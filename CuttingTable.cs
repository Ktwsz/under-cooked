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
        placedItem =
            ResourceLoader
                .Load<PackedScene>("res://KitchenChaos/Assets/Meshes/TomatoSliced.fbx")
                .Instantiate() as Node3D; // TODO: food spawner
        placedItem.SetScale(new Vector3(4, 4, 4));
        placedItem.SetName("TomatoSliced"); // TODO: spawner of food
        AddItemToScene();
    }

    // TODO: add map of asset => chopped asset and check if asset is present in map
    public override bool CanInteract() => placedItem != null && placedItem.GetName() == "Tomato";

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
