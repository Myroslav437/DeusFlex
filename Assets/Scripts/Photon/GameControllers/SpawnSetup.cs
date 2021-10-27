using UnityEngine;
using Photon.Pun;

public class SpawnSetup : MonoBehaviour 
{
    public static SpawnSetup SSetup;
    public Transform[] spawnPoints;

    public PhotonView PV;

    private void OnEnable()
    {
        if (SSetup == null)
        {
            SSetup = this;
        }
        PV = GetComponent<PhotonView>();
    }
}