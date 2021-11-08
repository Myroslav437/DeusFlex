using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{

    double[,] resourcesNeeded = { { 200, 0, 0, 0 }, { 400, 200, 0, 0 }, { 800, 400, 50, 0 }, { 1600, 800, 100, 5 } };
    double[] currentResources = {0,0,0,0};

    int currentLevel = 0;
    int levelCap = 3;

    public Sprite[] levelSprites;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = levelSprites[currentLevel];    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.GetComponent<CarriableResource>() != null)
        {

            addResource(collidedObject);
            TryTolevelUp();

            PhotonNetwork.Destroy(collidedObject.gameObject);
           
        }
    }

    void addResource(GameObject collectedResource)
    {
        CarriableResource carriadge = collectedResource.GetComponent<CarriableResource>();

        currentResources[(int) carriadge.getResType()] += carriadge.getResAmount();
        Debug.Log("Resources: w-" + currentResources[0] + " s-" + currentResources[1] + " g-"+ currentResources[2] + " e-"+ currentResources[3]);
        Debug.Log("Current requirement: w-" + resourcesNeeded[currentLevel,0] + " s-" + resourcesNeeded[currentLevel, 1] + " g-" + resourcesNeeded[currentLevel, 2] + " e-" + resourcesNeeded[currentLevel, 3]);
    }

    void TryTolevelUp()
    {
        if (isAbleToLevelUp()&&currentLevel<levelCap)
        {
            currentLevel++;
            GetComponent<SpriteRenderer>().sprite = levelSprites[currentLevel];

        }
    }

    bool isAbleToLevelUp()
    {
        bool state = true;

        for(int i = 0; i < 4; i++)
        {
            if(currentResources[i]+0.1f<= resourcesNeeded[currentLevel, i])
            state = false;
            
        }
        return state;
    }

}
