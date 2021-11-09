using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class basicSpawnAbility : abstractAbility
{
    public string resourceToSpawnName;

    public override void activate(GameObject parent)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceToSpawnName), parent.transform.position, Quaternion.identity, 0);
    }
}
