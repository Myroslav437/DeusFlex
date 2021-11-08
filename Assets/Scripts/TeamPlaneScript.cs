using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPlaneScript : MonoBehaviour
{
    public string[] tagsToDetect;
    public string teamName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in tagsToDetect) {
            if (collision.gameObject.tag == tag) {
                GameObject avatar = collision.gameObject;
                PhotonPlayer player = PhotonHelper.GetPhotonPlayerFromAvatar(avatar);

                player.AddMeToATeam(teamName);
                break;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in tagsToDetect) {
            if (collision.gameObject.tag == tag) {
                GameObject avatar = collision.gameObject;
                PhotonPlayer player = PhotonHelper.GetPhotonPlayerFromAvatar(avatar);

                // if player == null than it was disconnected while matchmaking countdown:
                if (player != null) { 
                    player.RemoveMeFromATeam();
                }
                break;
            }
        }
    }
}
