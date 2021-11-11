using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class sceneCall : abstractAbility
{
    public GameObject redCanvas;
    public GameObject blueCanvas;

    PhotonView PV;

    public override void activate(GameObject parent)
    {
        PV = parent.GetComponent<PhotonView>();

        //PV.RPC("setEndGameCanvas", RpcTarget.AllBufferedViaServer);
    }

    /*
    public void SetRedWinner() 
    {
        PV.RPC("RPC_SetRedWinner", RpcTarget.AllBufferedViaServer);
    }

    public void SetBlueWinner()
    {
        PV.RPC("RPC_SetBlueWinner", RpcTarget.AllBufferedViaServer);
    }

    public void RPC_SetRedWinner() 
    {
        GameObject posEx = GameObject.FindGameObjectWithTag("PositionCanvas");
        GameObject Instantiate(redCanvas, posEx.GetComponent<RectTransform>().position, Quaternion.identity);
    }

    public void RPC_SetBlueWinner() 
    {
        GameObject posEx = GameObject.FindGameObjectWithTag("PositionCanvas");
        Instantiate(blueCanvas, posEx.GetComponent<RectTransform>().position, Quaternion.identity);
    }
    */

}
