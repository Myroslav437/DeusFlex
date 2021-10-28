using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NicknameLabel : MonoBehaviour
{
    public string nickName;

    void Start() 
    {
        nickName = "Unknown";
        //nickName = LocalPlayerInfo.LPI.myNickName;
    }

    public void UpdateText()
    {
        GetComponentInChildren<TextMesh>().text = nickName;
    }
}
