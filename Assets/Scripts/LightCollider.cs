using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightCollider : MonoBehaviour
{
    public enum objectsEnum{Box,PrismLeft,PrismRight};
    public enum enemyEnum{Ghost_Brown,Ghost_Green,Ghost_Violet,Ghost_White};

    [NonSerialized]
    public GameObject enemy;
    [NonSerialized]
    public float speed,time,startTime,gameBeginTime; 
    [NonSerialized]
    public int myIndex;

    [NonSerialized]
    public string enemyName;

    [NonSerialized]
    public bool hit,exited,isBeginning,playOverride,isObjectInBetween;
    
    void Awake()
    {
        myIndex=-1;
        isBeginning=true;
        playOverride=true;
        enemyName="";
    }
    void Start()
    {
        hit=false;   
        exited=false;
        isObjectInBetween = false;
        time=0.5f;

        gameBeginTime=Time.unscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(playOverride)
        {
            speed=EnemyController.instance.enemies[myIndex].baseSpeed*GameController.instance.levels[GameController.instance.currentLevel-1].speedFactor*(GameController.instance.smallTorch.activeInHierarchy==false?1.5f:1f);

            if(hit)
                this.gameObject.transform.position=Vector3.MoveTowards(this.gameObject.transform.position,GameController.instance.player.transform.position,speed*Time.deltaTime);

            if(exited && Time.unscaledTime-startTime < time && !isObjectInBetween)
            {
                this.gameObject.transform.position=Vector3.MoveTowards(this.gameObject.transform.position,GameController.instance.player.transform.position,speed*Time.deltaTime);
                //Debug.Log("Time Duration : "+(Time.unscaledTime-startTime));
            }
            else if ( exited && Time.unscaledTime-startTime > time )
            {
                exited=false;
                enemy=null;
            }
        }

        //if (enemyName!="" && ( enemyName == enemyEnum.Ghost_Brown.ToString() || enemyName == enemyEnum.Ghost_Green.ToString() || enemyName == enemyEnum.Ghost_White.ToString() || enemyName == enemyEnum.Ghost_Violet.ToString()))
        this.transform.LookAt(GameController.instance.player.transform, Vector3.up);

        
    }

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DelayedTriggerEnter(other));
    }

    private void OnTriggerStay(Collider other)
    {
        Ray ray = new Ray(new Vector3(this.transform.position.x,0.2f, this.transform.position.z), GameController.instance.player.transform.position - this.transform.position);
        RaycastHit cleanHit;
        if (Physics.Raycast(ray, out cleanHit))
        {
            Debug.Log(cleanHit.collider.transform.tag);
            Debug.Log(cleanHit.collider.transform.name);
            if (cleanHit.collider.transform.tag == "barrier")
            {
                Debug.Log("Box in between");
                hit = false;
                isObjectInBetween = true;
            }
        }
        else
        {
            if (isObjectInBetween)
                isObjectInBetween = false;
        }
    }
    public IEnumerator DelayedTriggerEnter(Collider other)
    {
        while(myIndex==-1 && isBeginning)
            yield return new WaitForSeconds(0.1f);
        isBeginning=false;

        Debug.Log("My Index : "+myIndex);
        // Debug.Log("Enter Hit");
        
        speed=EnemyController.instance.enemies[myIndex].baseSpeed*GameController.instance.levels[GameController.instance.currentLevel-1].speedFactor*(GameController.instance.smallTorch==false?1.5f:1f);
        // Debug.Log("Speed : "+speed);
        
        hit = true;
        time = GameController.instance.levels[GameController.instance.currentLevel - 1].enemyFollowTime;

        enemyName = this.transform.name;
        if (enemyName == enemyEnum.Ghost_Brown.ToString() || enemyName == enemyEnum.Ghost_Green.ToString() || enemyName == enemyEnum.Ghost_White.ToString() || enemyName == enemyEnum.Ghost_Violet.ToString())
            this.transform.LookAt(GameController.instance.player.transform, Vector3.up);
            
    }

        
    

    public void OnTriggerExit(Collider other)
    {
        // Debug.Log("Exit Hit");
        startTime=Time.unscaledTime;
        hit=false;
        exited=true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if(collision.collider.name=="Player Variant(Clone)" )
        {
            playOverride=false;
            StartCoroutine(GameLost());
        }
    }

    public IEnumerator GameLost()
    {
        GameController.instance.gameLostScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        GameController.instance.gameLostScreen.SetActive(false);
        
        // isBeginning=true;
        SceneManager.LoadScene("GGJam");
        startTime=Time.unscaledTime;
        isBeginning=true;
        myIndex=-1;
    }
}
