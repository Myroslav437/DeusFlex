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

    private PhotonView PV;

    [SerializeField]
    float resourceAmout = 50f;

    void Start()
    {
        PV = GetComponent<PhotonView>();
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    transferOwnershipOnTouch(collision);
    //}
    //
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    transferOwnershipOnTouch(collision);
    //}

    //private void transferOwnershipOnTouch(Collision2D collision)
    //{
    //    if (!PV.IsMine)
    //    {
    //        Transform collisionObjectRoot = collision.transform.root;
    //        if (collisionObjectRoot.CompareTag("Player"))
    //        {
    //            Debug.Log("TransferedOwnership");
    //            PV.TransferOwnership(PhotonNetwork.LocalPlayer);
    //        }
    //    }
    //}
}
