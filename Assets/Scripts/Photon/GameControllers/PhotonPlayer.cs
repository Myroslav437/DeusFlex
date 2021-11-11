using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;
    public GameObject myAvatar;

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine) {
            //Debug.Log("My number in room is " + PhotonRoom.room.myNumberInRoom.ToString());
            Debug.Log("My number in room is " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
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
        
        // -------------
        myAvatar.transform.Find("Camera").gameObject.SetActive(true);

        return newAvatar;
    }



    public void AddMeToATeam(string teamName) 
    {
        if(PV.IsMine)  {
            // add me to the master
            Debug.Log("Adding me to the master client info");
            TeamController.TC.GetComponent<PhotonView>().RPC("RPC_AddPlayerInfo", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, PV.ViewID, LocalPlayerInfo.LPI.myNickName, teamName);
            TeamController.TC.GetComponent<PhotonView>().RPC("RPC_SendMasterPlayerInfo", RpcTarget.MasterClient);
        }
    }

    public void RemoveMeFromATeam() 
    {
        if (PV.IsMine) {
            // add me to the master
            Debug.Log("Removing me from the master client info");
            TeamController.TC.GetComponent<PhotonView>().RPC("RPC_RemovePayerInfo", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
            TeamController.TC.GetComponent<PhotonView>().RPC("RPC_SendMasterPlayerInfo", RpcTarget.MasterClient);
        }
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
