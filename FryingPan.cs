using System;
using Godot;

public partial class FryingPan : Node3D
{
    private Node3D item = null;

    private Timer timer;
    private ProgressBar progressBar;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerEnded;

        progressBar = GetNode<ProgressBar>("ProgressBar/ProgressBarViewport/ProgressBar");
    }

    public override void _Process(double delta)
    {
        var timeLeft = timer.GetTimeLeft();
        var waitTime = timer.GetWaitTime();

        progressBar.Value = 100 * (waitTime - timeLeft) / waitTime;
    }

    public void OnTimerEnded()
    {
        timer.Stop();
        GetNode<Sprite3D>("ProgressBar").SetVisible(false);

        RemoveChild(item);
        item = ResourceLoader.Load<PackedScene>("res://cooked_meat.tscn").Instantiate() as Node3D;
        AddChild(item); // TODO: separate function, set appropriate position?
        item.SetPosition(new Vector3(0, 0.1f, 0));

        // TDOD: start burning the meat, separate timer?
    }

    public Node3D GetItem() => item;

    public void Add(Node3D tmp)
    {
        if (!FoodConstants.IsFood(tmp) || tmp.GetName() != "UncookedMeat")
            return;

        item = tmp;
        if (tmp.GetParent() == null)
        {
            AddChild(tmp); // TODO: separate function, set appropriate position?
        }
        else
        {
            if (tmp.GetNode("../..") is Player)
                (tmp.GetNode("../..") as Player).HeldItem = null;

            if (tmp.GetNode("../..") is Table)
                (tmp.GetNode("../..") as Table).PlacedItem = null;

            tmp.Reparent(this, false); // TODO: separate function, set appropriate position?
        }
        item.SetPosition(new Vector3(0, 0.1f, 0));
    }

    public void StartFrying()
    {
        if (item == null || (item as Node3D).GetName() == "CookedMeat")
            return;

        if (timer.IsStopped())
            timer.Start();
        else
            timer.SetPaused(false);

        GetNode<Sprite3D>("ProgressBar").SetVisible(true);
    }

    public void StopFrying() => timer.SetPaused(true);
}
