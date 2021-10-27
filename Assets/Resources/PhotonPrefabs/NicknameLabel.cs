using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NicknameLabel : MonoBehaviour
{
    PhotonView PV;
    public string nickName;

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start() 
    {
        nickName = PhotonNetwork.NickName;
    }

    private void Update()
    {
        nickName = PhotonNetwork.NickName;
        GetComponent<TextMesh>().text = nickName;
    }
}
