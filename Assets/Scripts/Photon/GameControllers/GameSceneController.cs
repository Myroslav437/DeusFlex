using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameSceneController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    private static PhotonView PV;

    public override void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameSceneController started");
        if (PV.IsMine) { 
            //int playerid = PhotonRoom.room.myNetworkPlayer.GetComponent<PhotonView>().ViewID;
            //PV.RPC("RPC_SetNickName", RpcTarget.OthersBuffered, playerid, PhotonNetwork.NickName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void RPC_SetNickName(string nickName) {
        LocalPlayerInfo.LPI.myNickName = nickName;
        /*
        Debug.Log("RPC_SetNickName for" + nickName);

        PhotonView[] plArr = FindObjectsOfType<PhotonView>();
        GameObject obj = null;
        foreach(PhotonView pl in plArr) {
            if (pl.ViewID == playerViewID) {
                Debug.Log("Objects with ViewID = " + playerViewID + " found");
                obj = pl.gameObject;
                break;
            }
        }
        if (obj == null) {
            Debug.Log("No objects found");
        }
        */

        //PhotonPlayer tmp = obj.GetComponent<PhotonPlayer>();
        //PhotonRoom.room.myNetworkPlayer.GetComponent<PhotonPlayer>().SetNickName(nickName);
    }


}
