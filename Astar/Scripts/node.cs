using Godot;
using System;

public class node : Node2D
{
    //UI effects etc 
    AnimationPlayer anim;
    Area2D mouseArea;

    bool mouseInside;
    bool alreadyChanged = false; 
    
    Grid grid;


    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        mouseArea = GetNode<Area2D>("Area2D");
        mouseArea.Connect("mouse_entered",this, nameof(MouseEntered));
        mouseArea.Connect("mouse_exited",this, nameof(MouseExited));
        mouseInside = false; 
        grid = GetParent<Grid>();
        SetProcess(true);
    }


    private void MouseEntered()
    {
        anim.Play("mouseHover");
        mouseInside = true;
    }

    private void MouseExited()
    {
        anim.Play("mouseLeave");
        mouseInside = false;
        alreadyChanged = false;
    }

    public override void _Process(float delta)
    {
        if(mouseInside)
        {
            if(Input.IsActionJustReleased("mouseLeft"))
            {
                alreadyChanged = false;
            }

            if(Input.IsActionPressed("mouseLeft") && !alreadyChanged)
            {
                alreadyChanged = true;

                open = !open;

                if(open)
                {
                    anim.Play("open");
                }
                else
                {
                    anim.Play("close");
                }
                
            }

            if(Input.IsActionJustPressed("mouseRight"))
            {
                grid.AddNode(this);
            }
        }
    }

    public void SetAsPathNode()
    {
        anim.Play("path");
    }

    public void ResetState()
    {
        anim.Play("open");
    }


    public void SetAsSearching()
    {
        anim.Play("searching");
    }
    //stuff for pathfinding
    private bool open = true;

    public bool Open
    {
        set
        {
            open = value;
        }
        get
        {
            return open; 
        }
    }

    public Vector2 posGrid;
    public int hCost = 0,gCost = 0;

    public int fCost
    {
        get
        {
            return hCost+gCost;
        }
    }

    public node parentNode = null;
}
