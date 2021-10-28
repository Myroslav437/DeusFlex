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
        Debug.Log(LocalPlayerInfo.LPI.myNickName + " started RPC_SetNickName");
        GameObject avatar = LocalPlayerInfo.LPI.networkPlayer.GetComponent<PhotonPlayer>().myAvatar;
        int avatarID = avatar.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPC_SetNickName", RpcTarget.AllBufferedViaServer, avatarID, LocalPlayerInfo.LPI.myNickName);
    }

    [PunRPC]
    void RPC_SetNickName(int playerViewID, string nickName) {
        Debug.Log("Executing RPC_SetNickName for " + nickName);

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

        NicknameLabel nl = obj.GetComponent<NicknameLabel>();
        nl.nickName = nickName;
        nl.UpdateText();
    }


}
