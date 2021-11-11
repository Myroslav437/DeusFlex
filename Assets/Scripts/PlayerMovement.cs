using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    private PhotonView PV;
    private Rigidbody2D rb;
    private DistanceJoint2D carriableJoint;
    private Animator anim;
    public Camera playerCam;

    GameObject deityReference;

    public LayerMask resourcesMask;

    public float movementSpeed;

    public double playerDamage = 10;

    public float playerHealth=100;
    float playerMaxHealth = 100;



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
        if (Input.GetKeyDown(KeyCode.G) && PV.IsMine)
        {
            deityReference = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "basicDeity"), transform.position, Quaternion.identity, 0);
            deityReference.GetComponent<GodController>().playerReference = gameObject;

            photonView.RPC("disableSelf", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.R) && PV.IsMine)
        {
            transform.position= new Vector2(1, 1);
        }

        if (Input.GetMouseButtonDown(0) && PV.IsMine)
        {
            photonView.RPC("attack", RpcTarget.MasterClient);
        }
    }

    private void FixedUpdate()
    {
        if (PV.IsMine) {
            BasicMovement();
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
            Collider2D hitObject = Physics2D.OverlapCircle(mousePos, 1, resourcesMask);

            if(hitObject.tag.Equals("ResourceSource"))
            hitObject.GetComponent<ResourceSource>().takeDamage(playerDamage);

            if (hitObject.tag.Equals("CarriableResource"))
            {
                hitObject.GetComponent<PhotonView>().RequestOwnership();
                carriableJoint.enabled = true;
                carriableJoint.connectedBody = hitObject.GetComponent<Rigidbody2D>();
            }

        }
        else
        {
            carriableJoint.enabled = false;
        }
    }

    [PunRPC]
    void disableSelf()
    {
        gameObject.SetActive(false);

    }
}
