using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ResourceSource : MonoBehaviourPun
{
    // Start is called before the first frame update

    [SerializeField]
    int resourceAmount=3;

    [SerializeField]
    double maxResourceHealth=100;
    double resourceHealth;

    public string resourceToSpawnName;
    public string resourceLeftoverName;

    void Start()
    {
        resourceHealth = maxResourceHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(double damageAmout)
    {
        photonView.RequestOwnership();

        resourceHealth -= damageAmout;

        Debug.Log("Remaining health: "+resourceHealth);

        if (resourceHealth <= 0)
        {
            destroySelf();
        }
    }

    void destroySelf()
    {

        Debug.Log("Imded");

        // Instantiate resource leftover
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceLeftoverName), transform.position, Quaternion.identity, 0);

        // Instantiate carriable resources
        spawnResources(resourceToSpawnName, resourceAmount);

        PhotonNetwork.Destroy(this.gameObject);
    }


    void spawnResources(string resourceType, int num)
    { 
        Vector3 spawnPosition = transform.position;

        int offset = 1;

        spawnPosition.x += offset;
        spawnPosition.z = 0;

        for(int i = 0; i < num; i++)
        { 
            spawnPosition.y += offset * (i+0.6f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceToSpawnName), spawnPosition, Quaternion.identity, 0);
        }
 
    }


}
