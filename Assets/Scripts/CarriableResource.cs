using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public enum resourceType
{
    wood = 0,
    stone = 1,
    gold = 2,
    emeralds = 3
}

public class CarriableResource : MonoBehaviour
{

    public resourceType resource;

    [SerializeField]
    float resourceAmout = 50f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public resourceType getResType()
    {
        return resource;
    }
    public float getResAmount()
    {
        return resourceAmout;
    }
}
