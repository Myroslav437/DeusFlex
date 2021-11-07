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

        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
        playersData = new SortedDictionary<int, PlayerData>();
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
        if (PhotonNetwork.IsMasterClient) {
            playersData.Remove(actorNum);
        }
    }
}
