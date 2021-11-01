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

        // set the spawn point
        if (PV.IsMine) {
            Debug.Log("My number in room is " + PhotonRoom.room.myNumberInRoom.ToString());

            int spIdx = PhotonRoom.room.myNumberInRoom % GameSetup.GS.spawnPoints.Length;
            Transform sp = GameSetup.GS.spawnPoints[spIdx];
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), sp.position, sp.rotation, 0);

            myAvatar.transform.Find("Camera").gameObject.SetActive(true);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
