using Godot;
using System;

public class App : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    Pathfinding pathFinder;

    Grid grid;
    
    
    public override void _Ready()
    {
        pathFinder = GetNode<Pathfinding>("Pathfinding");
        grid = GetNode<Grid>("Grid");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("escape"))
        {
            GetTree().Quit();
        }

        if(Input.IsActionJustPressed("searchPath"))
        {
            pathFinder.FindPath();
        }

        if(Input.IsActionJustPressed("resetGrid"))
        {
            grid.FullReset();
        }
    }

    
}
