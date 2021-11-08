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

        Debug.Log(System.Environment.NewLine);
        foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData) {
            Debug.Log(p.Value.nickName + " " + p.Value.team + " " + p.Value.avatarType + System.Environment.NewLine);
        }
    }

    [PunRPC]
    void RPC_SetNickName(int playerViewID, string nickName) {
        Debug.Log("Executing RPC_SetNickName for " + nickName);

        PhotonView[] plArr = FindObjectsOfType<PhotonView>();
        GameObject obj = null;
        foreach(PhotonView pl in plArr) {
            if (pl.ViewID == playerViewID) {
                obj = pl.gameObject;
                break;
            }
        }

        NicknameLabel nl = obj.GetComponent<NicknameLabel>();
        nl.nickName = nickName;
        nl.UpdateText();
    }


}