using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct LevelDescription
{
    public float x;
    public float y;
    public int countGrid;
    public float speedFactor;
    public int mapNumber;
    public int noOfObjects;
    public float torchDuration;
    public float waitDuration;
    public float lightDuration;

    public float enemyFollowTime;

}

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    public enum objectsEnum{Box,PrismLeft,PrismRight};
    public enum enemyEnum{Ghost_Brown,Ghost_Green,Ghost_Violet,Ghost_White};
    public GameObject playerPrefab,exitPortalPrefab,gameLostScreen,roomClearedScreen,soundObject;
    public Text levelNo,lightIntensity, timeText, torchHintText,maxLevelText;

    public Animation lightningAnim;
    [NonSerialized]
    public GameObject[] objects;
    [NonSerialized]
    public GameObject player,exitPortal,smallTorch,largeTorch;
    public LevelDescription[] levels;

    public AudioClip lightningSound,backgrounSound;

    [NonSerialized]
    public AudioSource audio,backgroundAudio;
    [NonSerialized]
    public int currentLevel;
    [NonSerialized]
    public int totalLevels,maxLevelReached;
    [NonSerialized]
    public float startTime,highTorchOnTime, waitTorchTime, lightningStartTime;
    [NonSerialized]
    public bool isWaiting;
    public GameObject groundPlane;

    public static GameController instance;

    void Awake()
    {
        instance = this;
        totalLevels = 30;
        currentLevel = 1;
        isWaiting=false;
        maxLevelReached=PlayerPrefs.GetInt("MaxLevel",1);
        maxLevelText.text=PlayerPrefs.GetInt("MaxLevel",1).ToString();
        levelNo.text=1.ToString();
        lightningStartTime=Time.unscaledTime;
    }

    void Start()
    {      
        gameLostScreen.SetActive(false);
        roomClearedScreen.SetActive(false);
        audio= lightningAnim.gameObject.GetComponent<AudioSource>();
        backgroundAudio= soundObject.GetComponent<AudioSource>(); 
        backgroundAudio.clip=backgrounSound;
        backgroundAudio.Play();

        levels = new LevelDescription[totalLevels];
        objects=new GameObject[1000];
        InitializeLevelDescription();
        MakeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Time Left : "+timeLeft);
        
        if(Time.unscaledTime-lightningStartTime > levels[currentLevel-1].lightDuration)
        {
            audio.clip=lightningSound;
            audio.Play();
            lightningAnim.Blend("Lightning",1.0f,0);
            lightningStartTime=Time.unscaledTime;
        }

        if(Input.GetKeyDown("space"))
        {
            if(smallTorch.activeInHierarchy)
            {
                startTime=Time.unscaledTime;
                smallTorch.SetActive(false);
                largeTorch.SetActive(true);
                torchHintText.text="";
                lightIntensity.text="High";
                timeText.text=0.ToString();
            }
            else
            {
                smallTorch.SetActive(true);
                largeTorch.SetActive(false);
                lightIntensity.text="Low";
            }
        }

        if(Time.unscaledTime-startTime> highTorchOnTime && largeTorch.activeInHierarchy)
        {
            smallTorch.SetActive(true);
            largeTorch.SetActive(false);
            lightIntensity.text="Low";
            torchHintText.text="Torch Recharging";
            timeText.text=((int)waitTorchTime).ToString();
            isWaiting=true;
            startTime=Time.unscaledTime;
        }

        if(isWaiting)
        {
            if((Time.unscaledTime-startTime)<waitTorchTime)
                timeText.text=((int)(waitTorchTime-(Time.unscaledTime-startTime))).ToString();
            else
            {
                isWaiting=false;
                smallTorch.SetActive(true);
                largeTorch.SetActive(false);
                lightIntensity.text="Low";
                startTime=Time.unscaledTime;
                torchHintText.text="High beam torch recharged";
            }
        }
        

    // if(timeLeft<=0)
        // {
        //     smallTorch.SetActive(true);
        //     largeTorch.SetActive(false);
        //     timeLeft=waitTorchTime;
        //     lightIntensity.text="Low";
        //     torchHintText.text="Use Space for High Beam";
        // }
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        System.Random random = new System.Random();
        return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }


    public void InitializeLevelDescription()
    {
        levels[0].x = 50;
        levels[0].y = 50;
        levels[0].countGrid = 80;
        levels[0].noOfObjects = 30;
        levels[0].speedFactor = 1;
        levels[0].mapNumber = 0;
        levels[0].lightDuration = 9;
        levels[0].waitDuration = 5;
        levels[0].torchDuration = 5;
        levels[0].enemyFollowTime = 1;


        levels[1].x = 60;
        levels[1].y = 60;
        levels[1].countGrid = 90;
        levels[1].noOfObjects = 16;
        levels[1].speedFactor = 1.1f;
        levels[1].mapNumber = 1;
        levels[1].lightDuration = 12;
        levels[1].waitDuration = 7;
        levels[1].torchDuration = 3;
        levels[1].enemyFollowTime = 2;

        levels[2].x = 70;
        levels[2].y = 70;
        levels[2].countGrid = 100;
        levels[2].noOfObjects = 18;
        levels[2].speedFactor = 1.3f;
        levels[2].mapNumber = 2;
        levels[2].torchDuration = 3;
        levels[2].waitDuration = 9;
        levels[2].lightDuration = 14;
        levels[2].enemyFollowTime = 4;

        for(int i=3;i<=29;i++)
        {
            levels[i].x=levels[i].y=70+(i-2)*5;
            levels[i].countGrid=100+(i-2)*10;
            levels[i].speedFactor = 1.3f+(i-2)*0.2f;
            levels[i].torchDuration = 3;
            levels[i].waitDuration = 10;
            levels[i].lightDuration = 16;
            levels[i].enemyFollowTime = 4;

        }
        //levels[3].x = 10;
        //levels[3].y = 10;
        //levels[3].countGrid = 8;
        //levels[3].speedFactor = 1;
        //levels[3].mapNumber = 0;
        //levels[3].lightDuration = 5;

        //levels[4].x = 10;
        //levels[4].y = 10;
        //levels[4].countGrid = 8;
        //levels[4].speedFactor = 1;
        //levels[4].mapNumber = 0;
        //levels[4].lightDuration = 5;
    }

    public void MakeLevel()
    {
        float finalX = levels[currentLevel - 1].x;
        float finalY = levels[currentLevel - 1].y;
        //Debug.Log(finalX);
        //Debug.Log(finalY);

        if(currentLevel>1)
        {
            Vector3 scale=new Vector3(finalX- levels[currentLevel - 2].x, 0.01f, finalY- levels[currentLevel - 2].y);
            groundPlane.transform.localScale += scale;
            Debug.Log("Scale : "+groundPlane.transform.localScale);
        }
        levelNo.text=currentLevel.ToString();
        
        highTorchOnTime=GameController.instance.levels[currentLevel-1].torchDuration;
        waitTorchTime=GameController.instance.levels[currentLevel-1].waitDuration;
        
        timeText.text=0.ToString();
        
        torchHintText.text="Use Space for High Beam";

        int count = -1,objectCount = 0;

        for (float pos = -finalY / 2, posx=-finalX / 2; pos< finalY / 2;pos+= finalY / (levels[currentLevel-1].countGrid))
        {
            posx+=finalY / (levels[currentLevel-1].countGrid);
            float fixedy=finalX/2;
            float fixedx=finalY/2;

            objects[objectCount++]=Instantiate(ObjectController.instance.objectPrefab[3],new Vector3(pos,0,fixedy+0.6f) ,Quaternion.EulerAngles(UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90)));
            objects[objectCount++]=Instantiate(ObjectController.instance.objectPrefab[3],new Vector3(pos,0,-fixedy-0.6f) ,Quaternion.EulerAngles(UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90)));
            objects[objectCount++]=Instantiate(ObjectController.instance.objectPrefab[3],new Vector3(fixedx+0.6f,0,posx) ,Quaternion.EulerAngles(UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90)));
            objects[objectCount++]=Instantiate(ObjectController.instance.objectPrefab[3],new Vector3(-fixedx-0.6f,0,posx) ,Quaternion.EulerAngles(UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90),UnityEngine.Random.Range(-90,90)));

            count++;
        
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-finalX/2+ finalX / 10, finalX / 2 - finalX / 10), 0 , UnityEngine.Random.Range(pos,pos+finalY / (levels[currentLevel-1].countGrid)));
            int randomIndex= (int)UnityEngine.Random.Range(0,Enum.GetNames(typeof(enemyEnum)).Length-0.01f);
            //Debug.Log("Random Index : "+randomIndex);
            if(count==0)
            {
                player=Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0,0,0));
            }
            else if(count==GameController.instance.levels[GameController.instance.currentLevel-1].countGrid-1) 
            {
                exitPortal=Instantiate(exitPortalPrefab, spawnPosition+new Vector3(0,1.5f,0), Quaternion.Euler(0,0,0));
            }
            else if(count%6==0)
            {
                EnemyController.instance.instantiatedEnemies[count]=Instantiate(EnemyController.instance.enemyPrefabs[randomIndex], spawnPosition, Quaternion.Euler(0,0,0));
                StartCoroutine(DelayInitialiseColliderScript(randomIndex, EnemyController.instance.instantiatedEnemies[count]));
            }
            else
            {
                int randInt=(int)UnityEngine.Random.Range(0,8);
                ObjectController.instance.instantiatedPrefab[ObjectController.instance.totalInstantiatedObjects++]=Instantiate(ObjectController.instance.objectPrefab[randInt], spawnPosition+new Vector3(0,ObjectController.instance.objectPrefab[randInt].transform.position.y,0), ObjectController.instance.objectPrefab[randInt].transform.rotation);
            }
            
        }

        smallTorch=player.GetComponentsInChildren<Light>()[0].gameObject;
        largeTorch=player.GetComponentsInChildren<Light>()[1].gameObject;
        smallTorch.SetActive(true);
        largeTorch.SetActive(false);
        lightIntensity.text="Low";

    }

    public IEnumerator DelayInitialiseColliderScript(int randomIndex,GameObject currentGO)
    {
        while(currentGO.GetComponent<LightCollider>() == null)
            yield return new WaitForSeconds(0.1f);
        currentGO.GetComponent<LightCollider>().myIndex=randomIndex;
    }


}

