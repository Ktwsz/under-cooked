using System;
using System.Linq;
using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed { get; set; } = 14;

    private Vector3 _targetVelocity = Vector3.Zero;
    private Table _lastHighlightedTable = null;
    private Node3D _heldItem = null;

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private float GetDistToTable(Node3D table) =>
        table.GetGlobalPosition().DistanceSquaredTo(GetGlobalPosition());

    private void HighlightCurrentTable()
    {
        if (_lastHighlightedTable != null)
        {
            (_lastHighlightedTable.FindChild("Highlight") as Node3D).SetVisible(false);
        }
        var parent = GetParent<Node3D>();
        var children = parent.FindChildren("Node3D*");
        // TODO: give priority to the tables in front of the player (filter tables by angle between it and players pivot?)
        var closestTable = children
            .Where(table => GetDistToTable(table as Node3D) <= 3.0)
            .DefaultIfEmpty(null)
            .Aggregate(
                (minTable, nextTable) =>
                    GetDistToTable(minTable as Node3D) < GetDistToTable(nextTable as Node3D)
                        ? minTable
                        : nextTable
            );
        if (closestTable != null)
        {
            (closestTable.FindChild("Highlight") as Node3D).SetVisible(true);
        }
        _lastHighlightedTable = closestTable as Table;
    }

    public override void _Process(double delta)
    {
        HighlightCurrentTable();

        if (Input.IsActionJustPressed("pickup") && _lastHighlightedTable != null)
        {
            if (_heldItem == null)
            {
                _heldItem = _lastHighlightedTable.pickupItem();
                if (_heldItem != null)
                {
                    _heldItem.Reparent(GetNode<Node3D>("Pivot"), false);
                    _heldItem.SetPosition(new Vector3(0, 0, -1.2f));
                }
            }
            else
            {
                if (_lastHighlightedTable.tryPlaceItem(_heldItem))
                {
                    _heldItem = null;
                }
            }
        }
    }
}
