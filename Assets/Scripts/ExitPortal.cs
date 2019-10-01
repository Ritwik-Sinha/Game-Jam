using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name=="Player Variant(Clone)" )
        {
            StartCoroutine(NextLevel());
        }
    }

    public IEnumerator NextLevel()
    {
        GameController.instance.roomClearedScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        GameController.instance.roomClearedScreen.SetActive(false);
        
        DestroyEnemies();
        //TODO Destroy objects as well            
        GameController.instance.currentLevel++;
        GameController.instance.MakeLevel();
    
}

    public void DestroyEnemies()
    {
        if(GameController.instance.currentLevel>PlayerPrefs.GetInt("MaxLevel",1))
        {
            PlayerPrefs.SetInt("MaxLevel",GameController.instance.currentLevel);
            GameController.instance.maxLevelText.text=PlayerPrefs.GetInt("MaxLevel",1).ToString();
        }
        for(int i=0;i<1000;i++)
        {
            if(EnemyController.instance.instantiatedEnemies[i]!=null)
                Destroy(EnemyController.instance.instantiatedEnemies[i]);
            if(ObjectController.instance.instantiatedPrefab[i]!=null)
                Destroy(ObjectController.instance.instantiatedPrefab[i]);
            if(GameController.instance.objects[i]!=null)
                Destroy(GameController.instance.objects[i]);


            Destroy(GameController.instance.player);
            Destroy(GameController.instance.exitPortal);
        }
    }
}
