using Godot;
using System;

public class Grid : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    
    [Export]
    public int gridNodeWidth;

    [Export]
    public int gridNodeHeight;

    [Export]
    public int gridWidth;
    
    [Export]
    public int gridHeight;

    public node[,] nodes;    
    
    [Export]
    PackedScene node;

    public node startNode,endNode;

    int index = 0;

    public override void _Ready()
    {
        nodes = new node[gridWidth,gridHeight];
        //create Grid [25 x 14]
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                //create Node
                Node2D newNode = (Node2D)node.Instance();
                newNode.Position = new Vector2((x+1)*gridNodeWidth,(y+1)*gridNodeHeight);
                AddChild(newNode);
                
                node nodeInstance = (node)newNode;
                nodes[x,y] = nodeInstance;
                nodeInstance.posGrid = new Vector2(x,y);
            }
        }         
    }

    public override void _Process(float delta)
    {
        Update();
    }

    #region only ui management
    public void AddNode(node node)
    {
        if(index == 0)
        {
            if(startNode != null)
            {
                startNode.GetNode<AnimationPlayer>("AnimationPlayer").Play("open");  
            }
            startNode = node;
            node.GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
            GD.Print(node.posGrid);
            index ++;
        }
        else{
            if(endNode != null)
            {
                endNode.GetNode<AnimationPlayer>("AnimationPlayer").Play("open");  
            }
            endNode = node;
            index = 0;
            node.GetNode<AnimationPlayer>("AnimationPlayer").Play("end");
        }
    }

    public void ResetGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if(nodes[x,y] != startNode && nodes[x,y] != endNode && nodes[x,y].Open)
                {
                     nodes[x,y].ResetState();
                }
               
            }
        }
    }

    public void FullReset()
    {
        startNode = null;
        endNode = null;
        index = 0;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                nodes[x,y].Open = true;
                nodes[x,y].ResetState();
                nodes[x,y].parentNode = null;
            }
        }
    }

    public override void _Draw()
    {
        foreach(node node in nodes)
        {
            if(node.parentNode != null)
            {
                DrawLine(node.GlobalPosition,node.parentNode.GlobalPosition,Colors.Red,2);
            }
        }
    }
    
    #endregion
}
