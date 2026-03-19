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
        if (tmp is FryingPan)
        {
            Add((tmp as FryingPan).GetItem());
            return;
        }

        if (!(IsAllowedIngredient(tmp.GetName())))
            return;

        if (tmp.GetParent() == null)
        {
            AddChild(tmp); // TODO: separate function, set appropriate position? also can be merged perhaps with frying pan Add()
        }
        else
        {
            if (tmp.GetNode("../..") is Player)
                (tmp.GetNode("../..") as Player).HeldItem = null;

            if (tmp.GetNode("../..") is Table)
                (tmp.GetNode("../..") as Table).PlacedItem = null;

            tmp.Reparent(this, false); // TODO: separate function, set appropriate position?
        }
        tmp.SetPosition(new Vector3(0, _currentPos, 0));
        _currentPos += ExtractSize(tmp.GetChildren()[0] as Node3D);
        GetNode<Node3D>("BreadTop").SetPosition(new Vector3(0, _currentPos, 0));
    }
}
