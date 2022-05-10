using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
//do i make this -10 or is this right?
private const int MOVE_STRAIGHT_COST = 10;
private const int MOVE_DIAGONAL_COST = 140;

private Node[,] graph;
public Node[,] Graph 
    {
        get { return graph; }
        set { graph = value; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = xDistance - yDistance;
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Node GetLowestFCostNode(List<Node> pathNodeList)
    {
        Node lowestFCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++)
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathNodeList[i];
                        
        return lowestFCostNode;
    }

    private List<Node> GetNeighbourList(Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();

        if(currentNode.x - 1 >= 0)
        {
            neighbourList.Add(graph[currentNode.x - 1,currentNode.y]);

            if(currentNode.y - 1 >= 0)
                neighbourList.Add(graph[currentNode.x - 1, currentNode.y - 1]);
            if(currentNode.y + 1 < graph.GetLength(1))
                neighbourList.Add(graph[currentNode.x - 1, currentNode.y + 1]);
        }

        if(currentNode.x + 1 < graph.GetLength(0))
        {
            neighbourList.Add(graph[currentNode.x + 1, currentNode.y]);
                
            if(currentNode.y - 1 >= 0) 
                neighbourList.Add(graph[currentNode.x + 1, currentNode.y - 1]);
            if(currentNode.y + 1 < graph.GetLength(1)) 
                neighbourList.Add(graph[currentNode.x + 1, currentNode.y + 1]);
        }

        if(currentNode.y - 1 >= 0) 
            neighbourList.Add(graph[currentNode.x, currentNode.y - 1]);
        if(currentNode.y + 1 < graph.GetLength(1)) 
            neighbourList.Add(graph[currentNode.x, currentNode.y + 1]);
            
        return neighbourList;
    }

    private List<Node> CalculatePath(Node endNode)
        {
            List<Node> path = new List<Node>();
            path.Add(endNode);
            Node currentNode = endNode;
            while(currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

}
