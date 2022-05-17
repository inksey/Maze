using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    
    private MazeConstructor constructor;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    private AIController aIController;

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
        aIController = GetComponent<AIController>(); 
    }
    
    void Start()
    {
    constructor.GenerateNewMaze(rows, cols);
    aIController.Graph = constructor.graph;
    aIController.Player = CreatePlayer();
    aIController.Monster = CreateMonster(); 
    aIController.HallWidth = constructor.hallWidth;         
    aIController.StartAI();
    }

    private GameObject CreatePlayer()
    {
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";
        
        return player;
    }

    private GameObject CreateMonster()
    {
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";  

        return monster;  
    }
}