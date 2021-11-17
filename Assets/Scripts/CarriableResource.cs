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



public class CarriableResource : MonoBehaviour //, IPunObservable
{
    Rigidbody2D rigidbody2;

    public resourceType resource;
    public bool isCarried = false;

    public PhotonView PV;
    public Sprite ordinary;
    public Sprite selected;

    SpriteRenderer sR;
    
    public float resourceAmout = 50f;

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
        sR = GetComponent<SpriteRenderer>();
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

    public void setOrdinarySprite()
    {
        sR.sprite = ordinary;
    }

    public void setSelectedSprite()
    {
        sR.sprite = selected;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!PV.IsMine)
        {
            Transform collisionObjectRoot = collision.transform.root;
            if (collisionObjectRoot.CompareTag("Player")&&(!isCarried))
            {
                Debug.Log("TransferedOwnership");
                PV.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }
    }
}
