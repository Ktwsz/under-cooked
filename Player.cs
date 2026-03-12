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
    private bool _isInteracting = false;

    public override void _PhysicsProcess(double delta)
    {
        if (_isInteracting)
            return;

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
        var children = parent.FindChildren("*Table*");
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

    private void PickupAction()
    {
        if (_lastHighlightedTable == null)
            return;

        if (_heldItem == null)
        {
            _heldItem = _lastHighlightedTable.PickupItem();
            if (_heldItem != null)
            {
                _heldItem.Reparent(GetNode<Node3D>("Pivot"), false);
                _heldItem.SetPosition(new Vector3(0, 0, -1.2f));
            }
        }
        else
        {
            if (_lastHighlightedTable.TryPlaceItem(_heldItem))
            {
                _heldItem = null;
            }
        }
    }

    private void InterractionEnded()
    {
        _isInteracting = false;
        _lastHighlightedTable.GetTimer().Timeout -= InterractionEnded;
    }

    public override void _Process(double delta)
    {
        if (!_isInteracting)
        {
            HighlightCurrentTable();

            if (Input.IsActionJustPressed("pickup"))
            {
                PickupAction();
            }

            if (
                Input.IsActionJustPressed("interact")
                && _lastHighlightedTable != null
                && _lastHighlightedTable.CanInteract()
            )
            {
                _lastHighlightedTable.GetTimer().Timeout += InterractionEnded;
                _lastHighlightedTable.StartInteract();
                _isInteracting = true;
            }
        }

        if (
            Input.IsActionJustReleased("interact")
            && _lastHighlightedTable != null
            && _lastHighlightedTable.CanInteract()
        )
        {
            _lastHighlightedTable.GetTimer().Timeout -= InterractionEnded;
            _lastHighlightedTable.StopInteract();
            _isInteracting = false;
        }
    }
}
