using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ObjectDescription
{
    public string tag;
    public float damage;
    public bool left;
    public float intensityFactor;

}


public class ObjectController : MonoBehaviour
{
    //public enum objectsEnum{Box};
    public enum enemyEnum{Ghost_Brown,Ghost_Green,Ghost_Violet,Ghost_White};

    public GameObject[] objectPrefab;
    [NonSerialized]
    public GameObject[] instantiatedPrefab;
    [NonSerialized]
    public GameObject[] instantiatedPrefabIndex;
    [NonSerialized]
    public int totalInstantiatedObjects;
    // Start is called before the first frame update
    [NonSerialized]
    public ObjectDescription[] objects;
    [NonSerialized]
    public int totalObjects;

    public static ObjectController instance;

    void Awake()
    {
        instance = this;
        instantiatedPrefab=new GameObject[1000];
        totalObjects = 3;
        totalInstantiatedObjects=0;
    }

    void Start()
    {
        objects = new ObjectDescription[totalObjects];
        InitializeObjectDescription();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeObjectDescription()
    {   
        objects[0].tag= "Box";
        objects[0].damage = 0;
        objects[0].intensityFactor = 0.0f;

        objects[1].tag = "PrismLeft";
        objects[1].damage = 0;
        objects[1].intensityFactor = 0.9f;
        objects[1].left = true;

        objects[2].tag = "PrismRight";
        objects[2].damage = 0;
        objects[2].intensityFactor = 0.9f;
        objects[2].left = false;

    }

}

