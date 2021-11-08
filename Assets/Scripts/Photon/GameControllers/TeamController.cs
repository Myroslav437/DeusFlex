using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TeamController : MonoBehaviour
{
    public class PlayerData
    {
        public string team = "Unknown";
        public string avatarType = "Unknown";
        public string nickName = "Unknown";
        public int playerPVID;
    };


    public static TeamController TC;
    private PhotonView PV;

    public SortedDictionary<int, PlayerData> playersData;

    private void Awake()
    {
        if (TeamController.TC == null) {
            TeamController.TC = this;
        }
        else if (TeamController.TC != this) {
            Destroy(TeamController.TC.gameObject);
            TeamController.TC = this;
        }

        DontDestroyOnLoad(TC.gameObject);
        TC.PV = GetComponent<PhotonView>();
        TC.playersData = new SortedDictionary<int, PlayerData>();
    }

    public void AddPlayerInfo(int actorNum, int PVID, string nickName, string team) 
    {
        PlayerData pd = new PlayerData();
        pd.team = team;
        pd.playerPVID = PVID;
        pd.nickName = nickName;
        playersData.Add(actorNum, pd);
    }

    public void RemovePlayerInfo(int actorNum)
    {
        playersData.Remove(actorNum);
    }

    [PunRPC]
    public void RPC_AddPlayerInfo(int ActorID, int PVID, string nickName, string teamName)
    {
        Debug.Log("RPC_AddPlayerInfo Exectued");
        TC.AddPlayerInfo(ActorID, PVID, nickName, teamName);
    }

    [PunRPC]
    public void RPC_RemovePayerInfo(int ActorID)
    {
        Debug.Log("RPC_RemovePayerInfo Exectued");
        TC.RemovePlayerInfo(ActorID);
    }

    [PunRPC]
    public void RPC_SendMasterPlayerInfo() {
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

            PV.RPC("RPC_SetPlayerInfo", RpcTarget.OthersBuffered,
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

    public void AssignTypesRandomly() {
        int countRed = 0, countBlue = 0;
        int[] keysRed = new int[playersData.Count];
        int[] keysBlue = new int[playersData.Count];

        foreach (KeyValuePair<int, PlayerData> p in playersData) {
            if (p.Value.team == "Red") {
                keysRed[countRed++] = p.Key;
            }
            else if (p.Value.team == "Blue") {
                keysBlue[countBlue++] = p.Key;
            }
            p.Value.avatarType = "Player";
        }

        playersData[keysRed[Random.Range(0, countRed)]].avatarType = "God";
        playersData[keysBlue[Random.Range(0, countBlue)]].avatarType = "God";
    }
}
