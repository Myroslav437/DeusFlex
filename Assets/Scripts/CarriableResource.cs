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

    [SerializeField]
    float resourceAmout = 50f;

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
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

   // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
   // {
   //     if (stream.IsWriting)
   //     {
   //         stream.SendNext(rigidbody2.position);
   //         stream.SendNext(rigidbody2.velocity);
   //     }
   //
   //     if (stream.IsReading)
   //     {
   //         rigidbody2.position = (Vector2) stream.ReceiveNext();
   //         rigidbody2.velocity = (Vector3) stream.ReceiveNext();
   //
   //         float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
   //         rigidbody2.position += rigidbody2.velocity * lag;
   //     }
   // }
}
