using System;
using Godot;

public partial class Bun : Node3D
{
    public void Add(Node3D tmp)
    {
        tmp.Reparent(this, false);
    }
}
