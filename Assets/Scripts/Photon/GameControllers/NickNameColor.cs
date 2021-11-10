using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameColor : MonoBehaviour
{
    // Update is called once per frame
    public void Update()
    {
        foreach (KeyValuePair<int, TeamController.PlayerData> pd in TeamController.TC.playersData) {
            try { 
                GameObject go = PhotonHelper.FindObjectViaPVID(pd.Value.playerPVID);
                GameObject avatar = go.GetComponent<PhotonPlayer>().myAvatar;
                if (pd.Value.team == "Red") {
                    avatar.GetComponent<NicknameLabel>().PaintRed();
                }
                else if (pd.Value.team == "Blue") {
                    avatar.GetComponent<NicknameLabel>().PaintBlue();
                }
                else {
                    avatar.GetComponent<NicknameLabel>().PaintDefault();
                }
            }
            catch(Exception e) {
                Debug.Log("UpdateColors for" + pd.Value.nickName + ", " + pd.Value.playerPVID + " failed");
            }
        }

        // wait for several seconds;
    }
}
