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

    public void StartAI()
    {
        startRow = graph.GetUpperBound(0) - 1;
        startCol = graph.GetUpperBound(1) - 1;            
    }

    void Update()
    {
        if(startRow != -1 && startCol != -1)
        {            
            int playerCol = (int)Mathf.Round(player.transform.position.x / hallWidth);
            int playerRow = (int)Mathf.Round(player.transform.position.z / hallWidth);
            
            List<Node> path = FindPath(startRow, startCol, playerRow, playerCol);

            if(path != null && path.Count > 1)
                {
                    Node nextNode = path[1];
                    float nextX = nextNode.y * hallWidth;
                    float nextZ = nextNode.x * hallWidth;
                    Vector3 endPosition = new Vector3(nextX, 0f, nextZ);
                    float step =  monsterSpeed * Time.deltaTime;
                    monster.transform.position = Vector3.MoveTowards(monster.transform.position, endPosition, step);
                    Vector3 targetDirection = endPosition - monster.transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(monster.transform.forward, targetDirection, step, 0.0f);
                    monster.transform.rotation = Quaternion.LookRotation(newDirection);
                    if(monster.transform.position == endPosition)
                        {
                            startRow = nextNode.x;
                            startCol = nextNode.y;
                        }
                }
        }   
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

    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = graph[startX,startY];
        Node endNode = graph[endX, endY];

        List<Node> openList = new List<Node> { startNode };
        List<Node> closedList = new List<Node>();

        int graphWidth = graph.GetLength(0);
        int graphHeight = graph.GetLength(1);

        for(int x = 0; x < graphWidth; x++)
            for(int y = 0; y < graphHeight; y++)
            {
                Node pathNode = graph[x, y];
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)            
                return CalculatePath(endNode);            

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(Node neighbourNode in GetNeighbourList(currentNode)){
                if(closedList.Contains(neighbourNode)) continue;

                if(!neighbourNode.isWalkable){
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost){
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);  
                    neighbourNode.CalculateFCost();                  

                    if(!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }

        //out of nodes on the open list
        return null;
    }

    private GameObject monster;
    public GameObject Monster 
        {
            get { return monster; }
            set { monster = value; }       
        }

    private GameObject player;
    public GameObject Player
        {
            get { return player; }
            set { player = value; } 
        }

    private float hallWidth;
    public float HallWidth 
        {
            get { return hallWidth; }
            set { hallWidth = value; }
        }

    [SerializeField] private float monsterSpeed;
    private int startRow = -1;
    private int startCol = -1;


}
