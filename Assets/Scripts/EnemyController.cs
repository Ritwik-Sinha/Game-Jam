using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyDescription
{
    public float baseSpeed;
    public float health;
    public float damageGiven;

}


public class EnemyController : MonoBehaviour
{
    public enum enemyEnum{Ghost_Brown,Ghost_Green,Ghost_Violet,Ghost_White};

    public GameObject[] enemyPrefabs;
    [NonSerialized]
    public GameObject[] instantiatedEnemies;
    [NonSerialized]
    public int totalEnemies;

    public EnemyDescription[] enemies;
    public static EnemyController instance;

    void Awake()
    {
        instance = this;
        instantiatedEnemies=new GameObject[1000];
        totalEnemies = 4;
    }

    void Start()
    {
        enemies = new EnemyDescription[totalEnemies];
        InitializeEnemyDescription();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeEnemyDescription()
    {
        enemies[0].baseSpeed = 1.3f;
        enemies[0].health = 100;
        enemies[0].damageGiven = 50;

        enemies[1].baseSpeed = 1.5f;
        enemies[1].health = 120;
        enemies[1].damageGiven = 70;

        enemies[2].baseSpeed = 1.0f;
        enemies[2].health = 150;
        enemies[2].damageGiven = 100;

        enemies[3].baseSpeed = 1.5f;
        enemies[3].health = 120;
        enemies[3].damageGiven = 70;

    }

}

