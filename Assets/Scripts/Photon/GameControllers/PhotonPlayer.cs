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
        PV = GetComponent<PhotonView>();

        if (PV.IsMine) {
            Debug.Log("My number in room is " + PhotonRoom.room.myNumberInRoom.ToString());
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

        myAvatar = PhotonNetwork.Instantiate(prefabPath, pos, rot, 0);
 
        return myAvatar;
    }

    public void SetNickName(string nn) 
    {
        GetComponent<NicknameLabel>().nickName = nn;
    }
}
