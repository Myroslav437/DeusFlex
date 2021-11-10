using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameLabel : MonoBehaviour
{
    public string nickName;
    public SpriteRenderer nickNameBG;

    void Start() 
    {
        nickName = "Unknown";
        //nickName = LocalPlayerInfo.LPI.myNickName;
    }

    public void PaintRed() 
    {
        nickNameBG.color = new Color(0.6f, 0, 0, 0.35f);
    }

    public void PaintBlue() 
    {
        nickNameBG.color = new Color(0, 0, 0.6f, 0.35f);
    }

    public void PaintDefault() 
    {
        nickNameBG.color = new Color(0, 0, 0, 0.35f);
    }

    public void UpdateText()
    {
        GetComponentInChildren<TextMesh>().text = nickName;
    }
}
