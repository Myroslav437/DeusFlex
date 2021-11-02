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

    bool isDestroyed = false;

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
        Debug.Log("ouch");


        if (resourceHealth <= 0)
        {
            destroySelf();
        }
    }

    void destroySelf()
    {
       

        Vector3 spawnPosition = transform.position;

        int increment = 1;
        int offset = 1;

        spawnPosition.x += offset;
        spawnPosition.z = 0;

        Debug.Log("Imded");

        // Instantiate resource leftover
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceLeftoverName), transform.position, Quaternion.identity, 0);

        // Instantiate carriable resources
        foreach (GameObject resource in spawnResources(resourceToSpawnName, resourceAmount))
        {
            increment++;
            spawnPosition.y += offset * increment;
            resource.transform.position = spawnPosition;
        }


        PhotonNetwork.Destroy(this.gameObject);
    }


    List<GameObject> spawnResources(string resourceType, int num)
    {
        List<GameObject> spawnedObjects = new List<GameObject>(num);
        for(int i = 0; i < num; i++)
        {
            spawnedObjects.Add(PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceToSpawnName), transform.position, Quaternion.identity, 0));
        }
        return spawnedObjects;
    }


}
