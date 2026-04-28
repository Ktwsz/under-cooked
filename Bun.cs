using System;
using System.Linq;
using Godot;

public partial class Bun : Node3D
{
    private float _currentPos;

    private bool IsAllowedIngredient(string name) =>
        (name == "CookedMeat" || name == "TomatoSliced" || name == "CabbageSliced")
        && GetChildren().All(child => name != child.GetName());

    private float ExtractSize(Node3D node) =>
        (node.GetChildren()[0] as MeshInstance3D).GetAabb().Size.Y * node.GetScale().Y;

    public override void _Ready()
    {
        _currentPos = ExtractSize(GetNode<Node3D>("BreadBottom"));
    }

    public void Add(Node3D tmp)
    {
        if (tmp == null)
            return;

        if (tmp is FryingPan fryingPan)
        {
            Add(fryingPan.Item);
            return;
        }

        if (!(IsAllowedIngredient(tmp.GetName())))
            return;

        Reparent(tmp);

        tmp.SetPosition(new Vector3(0, _currentPos, 0));
        _currentPos += ExtractSize(tmp.GetChildren()[0] as Node3D);
        GetNode<Node3D>("BreadTop").SetPosition(new Vector3(0, _currentPos, 0));
    }

    private void Reparent(Node3D tmp)
    {
        if (tmp.GetParent() == null)
        {
            AddChild(tmp);
            return;
        }

        if (tmp.GetNode("../..") is Player player)
            player.HeldItem = null;

        if (tmp.GetNode("../..") is Table table)
            table.PlacedItem = null;

        if (tmp.GetParent() is FryingPan)
            (tmp.GetParent() as FryingPan).Item = null;

        tmp.Reparent(this, false);
    }
}
