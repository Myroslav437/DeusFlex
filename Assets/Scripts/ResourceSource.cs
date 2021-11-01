using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ResourceSource : MonoBehaviour
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

        Vector3 spawnPosition = this.transform.position;

        int offset = 1;
        int increment = 1;

        spawnPosition.x += offset;
        spawnPosition.z = 0;

        resourceHealth -= damageAmout;
        Debug.Log("ouch");


        if (resourceHealth <= 0)
        {
            Debug.Log("Imded");
            //Instantiate(resourceLeftover).transform.position = transform.position;
            // Instantiate resource leftover
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceLeftoverName), transform.position, Quaternion.identity, 0);

            foreach(GameObject resource in spawnResources(resourceToSpawnName, resourceAmount))
            {
                increment++;
                spawnPosition.y += offset*increment;
                resource.transform.position=spawnPosition;
            }

            //Destroy(this.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
        }
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
