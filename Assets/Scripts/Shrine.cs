using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{

    double levelCap = 100;
    double currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.GetComponent<CarriableResource>() != null)
        {
            CarriableResource carriadge = collidedObject.GetComponent<CarriableResource>();
            currentLevel += carriadge.getResourceAmout();

           Destroy(collidedObject.gameObject);
           
        }
    }

}
