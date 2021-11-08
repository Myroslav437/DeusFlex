using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PhotonHelper : MonoBehaviourPunCallbacks
{
    public static PhotonHelper PH;
    private PhotonView PV;

    private void Awake()
    {
        if (PhotonHelper.PH == null) {
            PhotonHelper.PH = this;
        }
        else if (PhotonHelper.PH != this) {
            Destroy(PhotonHelper.PH.gameObject);
            PhotonHelper.PH = this;
        }

        PhotonHelper.PH.PV = GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameObject FindObjectViaPVID(int PVID) 
    {
        PhotonView[] plArr = FindObjectsOfType<PhotonView>();
        foreach (PhotonView p in plArr) {
            if (p.ViewID == PVID) {
                return p.gameObject;
            }
        }

        return null;
    }

    public static PhotonPlayer GetPhotonPlayerFromAvatar(GameObject avatar)
    {
        PhotonPlayer[] arr = GameObject.FindObjectsOfType<PhotonPlayer>();
        foreach (PhotonPlayer player in arr) {
            if (player.myAvatar == avatar) {
                return player;
            }
        }

        return null;
    }
}
