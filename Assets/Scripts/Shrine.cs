using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour, IPunObservable
{

    double[,] resourcesNeeded = { { 200, 0, 0, 0 }, { 400, 200, 0, 0 }, { 800, 400, 50, 0 }, { 1600, 800, 100, 5 } };
    double[] currentResources = {0,0,0,0};

    int currentLevel = 0;
    int levelCap = 3;
    private PhotonView PV;

    public string shrineTeam = "unknown";
    public Sprite[] levelSprites;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        GetComponent<SpriteRenderer>().sprite = levelSprites[currentLevel];    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        try
        {
            if ((collidedObject.GetComponent<CarriableResource>() != null) && (checkSameTeam()))
            {

                addResource(collidedObject);
                TryTolevelUp();

                PhotonNetwork.Destroy(collidedObject.gameObject);

            }
        }catch(Exception)
        {

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
            PV.RPC("RPC_SetShrimeLevel", RpcTarget.AllBufferedViaServer, PV.ViewID, currentLevel);
        }
    }

    [PunRPC]
    void RPC_SetShrimeLevel(int ShrimePID, int newLevel) 
    {
        Shrine shr = PhotonHelper.FindObjectViaPVID(ShrimePID).GetComponent<Shrine>();
        shr.currentLevel = newLevel;
        shr.GetComponent<SpriteRenderer>().sprite = levelSprites[currentLevel];
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

    public int getShrineLevel()
    {
        return currentLevel;
    }

    //RPC would suit this better
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentResources);
        }
        else
        {
            this.currentResources = (double[])stream.ReceiveNext();
        }
    }

    bool checkSameTeam()
    {
           return  TeamController.TC.playersData[PhotonNetwork.LocalPlayer.ActorNumber].team == shrineTeam;
    }
}
