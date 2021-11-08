using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    private PhotonView PV;
    private Rigidbody2D rb;
    private DistanceJoint2D carriableJoint;
    private Animator anim;
    public Camera playerCam;

    public LayerMask resourcesMask;

    public float movementSpeed;

    public double playerDamage = 10;

    public float playerHealth=100;
    public float playerMaxHealth = 100;



    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        anim = GameObject.Find("model").GetComponent<Animator>();
        carriableJoint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& PV.IsMine)
        {
            //  photonView.RPC("attack", RpcTarget.All);
            attack();
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


        if (Physics2D.OverlapCircle(mousePos, 1, resourcesMask) != null)
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
}
