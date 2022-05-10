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

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
    }
    
    void Start()
    {
    constructor.GenerateNewMaze(rows, cols);
    CreatePlayer();
    CreateMonster();
    }

    private void CreatePlayer()
    {
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";
    }

    private void CreateMonster()
    {
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";    
    }
}