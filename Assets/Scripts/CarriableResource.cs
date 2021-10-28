using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableResource : MonoBehaviour
{
    [SerializeField]
    float resourceAmout = 50f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getResourceAmout()
    {
        return resourceAmout;
    }
}
