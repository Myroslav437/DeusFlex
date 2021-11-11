using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    private static PhotonView PV;
    public int menuScene;
    public Canvas RedTeamWin;
    public Canvas BlueTeamWin;

    public override void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(LocalPlayerInfo.LPI.myNickName + " started RPC_SetNickName");
        PhotonPlayer player = LocalPlayerInfo.LPI.networkPlayer.GetComponent<PhotonPlayer>();
        GameObject avatar = player.myAvatar;
        int avatarID = avatar.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPC_UpdatePVID", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PhotonView>().ViewID);
        PV.RPC("RPC_SetNickName", RpcTarget.AllBufferedViaServer, avatarID, LocalPlayerInfo.LPI.myNickName);

        Debug.Log(System.Environment.NewLine);
        foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData) {
            Debug.Log(p.Value.nickName + " " + p.Value.team + " " + p.Value.avatarType + System.Environment.NewLine);
        }
    }

    [PunRPC]
    void RPC_SetNickName(int playerViewID, string nickName) {
        Debug.Log("Executing RPC_SetNickName for " + nickName);

        try { 
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
        catch(Exception) {
            Debug.Log("RPC_SetNickName for " + nickName + " Failed");
        }
    }

    [PunRPC]
    void RPC_UpdatePVID(int actorID, int newPVID) {
        Debug.Log("Executing RPC_UpdatePVID for " + actorID.ToString());
        TeamController.TC.playersData[actorID].playerPVID = newPVID;
    }

    public void onReturnToMenuButtonPressed() 
    {
        Destroy(PhotonHelper.PH.gameObject);
        Destroy(PhotonRoom.room.gameObject);
        Destroy(TeamController.TC.gameObject);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuScene);
    }


}
