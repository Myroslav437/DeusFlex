using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resource : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject resourceToSpawn;
    [SerializeField]
    int resourceAmount=3;

    [SerializeField]
    double maxResourceHealth=100;
    double resourceHealth;
    
    public GameObject trunk;

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

        int offset = 3;
        int increment = 1;

        spawnPosition.y += offset;
        spawnPosition.z = 0;

        resourceHealth -= damageAmout;
        Debug.Log("ouch");


        if (resourceHealth <= 0)
        {
            Debug.Log("Imded");
            Instantiate(trunk).transform.position = transform.position;

            foreach(GameObject resource in spawnResources(resourceToSpawn, resourceAmount))
            {
                increment++;
                spawnPosition.x += offset*increment;
                resource.transform.position=spawnPosition;
            }
            Destroy(this.gameObject);
        }
    }


    List<GameObject> spawnResources(GameObject resourceType, int num)
    {
        List<GameObject> spawnedObjects = new List<GameObject>(num);
        for(int i = 0; i < num; i++)
        {
            spawnedObjects.Add( Instantiate(resourceType));
        }
        return spawnedObjects;
    }


}
