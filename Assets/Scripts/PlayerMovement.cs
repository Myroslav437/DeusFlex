using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    private PhotonView PV;
    private Rigidbody2D rb;
    private DistanceJoint2D carriableJoint;
    private Animator anim;
    public Camera playerCam;

    GameObject deityReference;

    public LayerMask resourcesMask;

    public float movementSpeed;

    public float playerDamage = 10;

    public float playerHealth=100;
    float playerMaxHealth = 100;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        carriableJoint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G) && PV.IsMine)
        //{
        //    deityReference = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "basicDeity"), transform.position, Quaternion.identity, 0);
        //    deityReference.GetComponent<GodController>().playerReference = gameObject;
        //
        //    photonView.RPC("disableSelf", RpcTarget.MasterClient);
        //}

       // if (Input.GetKeyDown(KeyCode.R) && PV.IsMine)
       // {
       //     transform.position= new Vector2(1, 1);
       // }
    }

    private void FixedUpdate()
    {
        if (playerHealth <= 0)
            isDead = true;

        if (!isDead)
        {
            if (PV.IsMine)
            {
                BasicMovement();
            }

            if (Input.GetMouseButtonDown(0) && PV.IsMine)
            {
                //  photonView.RPC("attack", RpcTarget.MasterClient);
                attack();
            }
        }
    }

    void BasicMovement() 
    {
       //Needs formatting

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        float horSpeed = movementVector.x * movementSpeed * Time.deltaTime;

        if (rb.velocity.y == 0f)
        {
            rb.MovePosition(rb.position + movementVector * movementSpeed * Time.deltaTime);
            anim.SetFloat("HorSpeed", Mathf.Abs(horSpeed));
        }
    }


    //[PunRPC]
    void attack()
    {
        Vector3 mousePos = playerCam.ScreenToWorldPoint(Input.mousePosition);


        if (Physics2D.OverlapCircle(mousePos, 0.5f, resourcesMask) != null)
        {
            Collider2D hitObject = Physics2D.OverlapCircle(mousePos, 0.5f, resourcesMask);

            if (hitObject.tag.Equals("ResourceSource"))
            {
                hitObject.GetComponent<ResourceSource>().takeDamage(playerDamage);
            }
            

            if (hitObject.tag.Equals("CarriableResource"))
            {
                hitObject.GetComponent<PhotonView>().RequestOwnership();

                carriableJoint.enabled = true;
                carriableJoint.connectedBody = hitObject.GetComponent<Rigidbody2D>();
            }

            if (hitObject.tag.Equals("player"))
            {
                //if(hitObject)
                Debug.Log("Player hitting works");
                hitObject.GetComponent<PlayerMovement>().playerHealth -= this.playerDamage;
                Debug.Log(hitObject.GetComponent<PlayerMovement>().playerHealth);
                // Debug.Log(TeamController.TC.playersData[].team);
            }
        }
        else
        {
            carriableJoint.enabled = false;
        }

       


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHealth);
        }
        else
        {
            this.playerHealth = (float)stream.ReceiveNext();
        }
    }

    public void ressurect()
    {
        isDead = false;
    }

    // [PunRPC]
    // void connectToCarriable(GameObject carr)
    // {
    //     carriableJoint.enabled = true;
    //     carriableJoint.connectedBody = carr.GetComponent<Rigidbody2D>();
    // }
    //
    // [PunRPC]
    // void disconnectFromCarriable()
    // {
    //     carriableJoint.enabled = false;
    // }

    //[PunRPC]
    //void disableSelf()
    //{
    //    gameObject.SetActive(false);
    //
    //}
}
