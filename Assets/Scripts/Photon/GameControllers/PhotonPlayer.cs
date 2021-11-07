using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine) {
            //Debug.Log("My number in room is " + PhotonRoom.room.myNumberInRoom.ToString());
            Debug.Log("My number in room is " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    public GameObject InstantiateAvatar(string prefabPath) 
    {
        // set the spawn point
        Vector3 pos = new Vector3(0,0, 0);
        Quaternion rot = Quaternion.identity;

        try {
            if (SpawnSetup.SSetup.spawnPoints.Length == 0) {
                throw new System.NullReferenceException();
            }

            int spIdx = PhotonRoom.room.myNumberInRoom % SpawnSetup.SSetup.spawnPoints.Length;
            Transform sp = SpawnSetup.SSetup.spawnPoints[spIdx];

            pos = sp.position;
            rot = transform.rotation;
        }
        catch (System.NullReferenceException) {
        }

        GameObject newAvatar = PhotonNetwork.Instantiate(prefabPath, pos, rot, 0); // assign avatar using rpc (via photonViewId)
        myAvatar = newAvatar;

        int myPVID = PV.ViewID;
        int avatarPVID = newAvatar.GetComponent<PhotonView>().ViewID;

        PV.RPC("RPC_ConnectAvatar", RpcTarget.AllBufferedViaServer, myPVID, avatarPVID);
 
        return newAvatar;
    }



    public void AddMeToATeam(string teamName) 
    {
        if(PV.IsMine)  {
            // add me to the master
            Debug.Log("Adding me to the master client info");
            PV.RPC("RPC_AddPlayerInfo", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, PV.ViewID, LocalPlayerInfo.LPI.myNickName, teamName);
        }
    }

    public void RemoveMeFromATeam() 
    {
        if (PV.IsMine) {
            // add me to the master
            Debug.Log("Removing me from the master client info");
            PV.RPC("RPC_RemovePayerInfo", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    public void RPC_AddPlayerInfo(int ActorID, int PVID, string nickName, string teamName)
    {
        Debug.Log("RPC_AddPlayerInfo Exectued");
        TeamController.TC.AddPlayerInfo(ActorID, PVID, nickName, teamName);

        // update team info on all cleints
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Updating info on the all cilents");
            int size = TeamController.TC.playersData.Count;
            int[] TC_ActorNumbers = new int[size];
            int[] TC_PlayerPVIDs = new int[size];
            string[] TC_TeamNames = new string[size];
            string[] TC_AvatarTypes = new string[size];
            string[] TC_NickNames = new string[size];

            int i = 0;
            foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData)
            {
                TC_ActorNumbers[i] = p.Key;
                TC_PlayerPVIDs[i] = p.Value.playerPVID;
                TC_TeamNames[i] = p.Value.team;
                TC_AvatarTypes[i] = p.Value.avatarType;
                TC_NickNames[i] = p.Value.nickName;
                ++i;
            }

            PV.RPC("RPC_SetPlayerInfo", RpcTarget.OthersBuffered,
                (int[])TC_ActorNumbers, (int[])TC_PlayerPVIDs, (string[])TC_TeamNames, (string[])TC_AvatarTypes, (string[])TC_NickNames);
        }
    }

    [PunRPC]
    public void RPC_RemovePayerInfo(int ActorID)
    {
        Debug.Log("RPC_RemovePayerInfo Exectued");
        TeamController.TC.RemovePlayerInfo(ActorID);

        // update team info on all cleints
        if (PhotonNetwork.IsMasterClient)  {
            Debug.Log("Updating info on the all cilents");

            int size = TeamController.TC.playersData.Count;
            int[] TC_ActorNumbers = new int[size];
            int[] TC_PlayerPVIDs = new int[size];
            string[] TC_TeamNames = new string[size];
            string[] TC_AvatarTypes = new string[size];
            string[] TC_NickNames = new string[size];

            int i = 0;
            foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData)
            {
                TC_ActorNumbers[i] = p.Key;
                TC_PlayerPVIDs[i] = p.Value.playerPVID;
                TC_TeamNames[i] = p.Value.team;
                TC_AvatarTypes[i] = p.Value.avatarType;
                TC_NickNames[i] = p.Value.nickName;
                ++i;
            }

            PV.RPC("RPC_SetPlayerInfo", RpcTarget.AllBufferedViaServer,
                (int[])TC_ActorNumbers, (int[])TC_PlayerPVIDs, (string[])TC_TeamNames, (string[])TC_AvatarTypes, (string[])TC_NickNames);
        }
    }

    [PunRPC]
    void RPC_SetPlayerInfo(int[] TC_ActorNumbers, int[] TC_PlayerPVIDs, string[] TC_TeamNames, string[] TC_AvatarTypes, string[] TC_NickNames)
    {
        Debug.Log("RPC_PlayerInfoUpdate Executed");

        int size = TC_ActorNumbers.Length;

        SortedDictionary<int, TeamController.PlayerData> tmp = new SortedDictionary<int, TeamController.PlayerData>();
        for (int i = 0; i < size; ++i)
        {
            TeamController.PlayerData tmpData = new TeamController.PlayerData();
            tmpData.playerPVID = TC_PlayerPVIDs[i];
            tmpData.team = TC_TeamNames[i];
            tmpData.nickName = TC_NickNames[i];
            tmpData.avatarType = TC_AvatarTypes[i];
            tmp.Add(TC_ActorNumbers[i], tmpData);
        }
        TeamController.TC.playersData = tmp;
    }

    [PunRPC]
    void RPC_ConnectAvatar(int netPlayerPVID, int avatarPVID)
    {
        GameObject netPl = PhotonHelper.FindObjectViaPVID(netPlayerPVID);
        GameObject avatar = PhotonHelper.FindObjectViaPVID(avatarPVID);

        PhotonPlayer pPl = netPl.GetComponent<PhotonPlayer>();
        pPl.myAvatar = avatar;
    }
}
