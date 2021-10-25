using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public PhotonView PV;

    public Transform[] spawnPoints;


    private void OnEnable()
    {
        Debug.Log("GameSetup enabeled");

        if (GameSetup.GS == null) {
            GameSetup.GS = this;
        }
        PV = GetComponent<PhotonView>();
    }
}
