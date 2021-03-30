using Godot;
using System;
using System.Collections.Generic;

public class Pathfinding : Node2D
{
    public List<node> path = new List<node>();
    Grid grid;

    public override void _Ready()
    {
        grid = GetParent().GetNode<Grid>("Grid");
    }

    public override void _Process(float delta)
    {
        
    }

    //has to be on frames of godot? maybe 
    public async void FindPath()
    {
        grid.ResetGrid();
        List<node> openList = new List<node>();
        List<node> closedList = new List<node>();

        openList.Add(grid.startNode);


        //while openlist is not empty
        while(openList.Count > 0)
        {
            //find the node with the least f cost
            node q = MinCost(openList);
            openList.Remove(q);
            closedList.Add(q);

            List<node> neighbors = FindNeighbors(q);
            
            foreach(node neighbour in neighbors)
            {
                if(neighbour.posGrid == grid.endNode.posGrid) 
                {
                    grid.endNode.parentNode = q;
                    GD.Print("found path");
                    RetracePath();
                    return;
                }
                

                

                //GD.Print(currNode.fCost + " at: "+ currNode.posGrid + " (" + grid.endNode.posGrid + ")");

                bool con = false; 

                foreach(node nodeInClosed in closedList)
                {
                    if(nodeInClosed.posGrid == neighbour.posGrid) //&& nodeInClosed.fCost < currNode.fCost)
                    {
                        con = true;
                        break;
                    }
                }

                if(con)
                {
                    continue;
                }

                //if node is already in openList with lower fCost > skip
                // con = false; 
                // foreach(node nodeInOpen in openList)
                // {
                //     if(nodeInOpen.posGrid == neighbour.posGrid)
                //     {
                //         if(nodeInOpen.fCost < fCost)
                //         {
                //             con = true;
                //             break;
                //         }
                //     }
                // }

                // if(con)
                // {
                //     await ToSignal(GetTree(), "idle_frame");
                //     continue;
                // }

                int gCost = q.gCost + DistanceToNode(neighbour,q);
                int hCost = DistanceToNode(neighbour,grid.endNode);
                int fCost = gCost+hCost;

                if(neighbour.fCost < fCost && openList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.hCost = hCost;
                neighbour.gCost = gCost;
                neighbour.parentNode = q;

                if(!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }

            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
        }
    }

    List<node> FindNeighbors(node a)
    {
        List<node> neighbors = new List<node>();
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                int xGrid = (int)a.posGrid.x + x; 
                int yGrid = (int)a.posGrid.y + y;

                if(x == 0 && y ==0)
                {
                    continue; 
                }

                if(xGrid < 0 || xGrid >= grid.gridWidth || yGrid < 0 || yGrid >= grid.gridHeight)
                {
                    continue;
                }
                
                node currNode = grid.nodes[xGrid,yGrid];

                if(currNode.Open)
                {
                    neighbors.Add(currNode);

                    if(currNode.posGrid != grid.endNode.posGrid && currNode.posGrid != grid.startNode.posGrid)
                    {
                        currNode.SetAsSearching();
                    }
                }
            }
        }

        return neighbors;
    }

    int DistanceToNode(node a, node b)
    {
        int distanceX = (int)Math.Abs(a.posGrid.x - b.posGrid.x);
        int distanceY = (int)Math.Abs(a.posGrid.y - b.posGrid.y);
        if(distanceY < distanceX)
        {
            return 14*distanceY + 10*(distanceX-distanceY);
        }
        return 14*distanceX + 10*(distanceY-distanceX);
    }


    node MinCost(List<node> list)
    {
        int minCost = list[0].fCost;
        int index = 0;

        for (int i = 0; i < list.Count; i++)
        {
            int currF = list[i].fCost;
            if(currF < minCost)
            {
                minCost = currF;
                index = i;
            }
        }

        return list[index];
    }

    async void RetracePath()
    {
        grid.ResetGrid();
        path = new List<node>();
        GD.Print("retrace path");
        

        path.Add(grid.endNode);

        node currNode = grid.endNode;

        while(currNode != grid.startNode)
        {
            if(currNode != grid.endNode)
            {
                currNode.SetAsPathNode();

            }
            path.Add(currNode);
            currNode = currNode.parentNode;
            
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
        }
        GD.Print("path finished");
    }   
}

