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
    public GameObject body;
    public GameObject leg1;
    public GameObject leg2;

    GameObject deityReference;
    public float jumpResetTime = 0.8f;
    private float jumpResetCount = 0;

    public LayerMask resourcesMask;

    public float movementSpeed;
    public float jumpForce;
    public bool allowedToJump = true;

    public double playerDamage = 10;

    public float playerHealth = 100;
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
        jumpResetCount -= Time.deltaTime;
        if (jumpResetCount <= 0) {
            jumpResetCount = jumpResetTime;
            allowedToJump = true;
        }
        /*
        if (Input.GetKeyDown(KeyCode.G) && PV.IsMine)
        {
            deityReference = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "basicDeity"), transform.position, Quaternion.identity, 0);
            deityReference.GetComponent<GodController>().playerReference = gameObject;

            photonView.RPC("disableSelf", RpcTarget.All);
        }
        */

        if (Input.GetKeyDown(KeyCode.R) && PV.IsMine)
        {
            transform.position = new Vector2(1, 1);
        }
        if (Input.GetMouseButtonDown(0) && PV.IsMine)
        {
            // attack();
            // photonView.RPC("attack", RpcTarget.MasterClient);
        }
        if (PV.IsMine) {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 bodyScale = body.GetComponent<Transform>().localScale;
                Vector3 leg1Scale = leg1.GetComponent<Transform>().localScale;
                Vector3 leg2Scale = leg2.GetComponent<Transform>().localScale;
                if (bodyScale.x < 0)
                {
                    PV.RPC("PRC_RotateLeft", RpcTarget.AllViaServer, PV.ViewID);
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 bodyScale = body.GetComponent<Transform>().localScale;
                if (bodyScale.x > 0)
                {
                    PV.RPC("PRC_RotateRight", RpcTarget.AllViaServer, PV.ViewID);
                }
            }
          if (Input.GetKeyDown(KeyCode.Space))
          {
              if (allowedToJump) {
                  Vector2 movementVector = new Vector2(0, 1);
                   rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                  // rb.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
                  allowedToJump = false;
              }
          }
        }
    }

    [PunRPC]
    public void PRC_RotateLeft(int PID) 
    {
        PlayerMovement otherPlayerMovement = PhotonHelper.FindObjectViaPVID(PID).GetComponent<PlayerMovement>();
        Vector3 bodyScale = otherPlayerMovement.body.GetComponent<Transform>().localScale;
        Vector3 leg1Scale = otherPlayerMovement.leg1.GetComponent<Transform>().localScale;
        Vector3 leg2Scale = otherPlayerMovement.leg2.GetComponent<Transform>().localScale;
        if (bodyScale.x < 0)
        {
            bodyScale.x = bodyScale.x * -1;
            leg1Scale.x = leg1Scale.x * -1;
            leg2Scale.x = leg2Scale.x * -1;
        }
        otherPlayerMovement.body.GetComponent<Transform>().localScale = bodyScale;
        otherPlayerMovement.leg1.GetComponent<Transform>().localScale = leg1Scale;
        otherPlayerMovement.leg2.GetComponent<Transform>().localScale = leg2Scale;
    }

    [PunRPC]
    public void PRC_RotateRight(int PID)
    {
        PlayerMovement otherPlayerMovement = PhotonHelper.FindObjectViaPVID(PID).GetComponent<PlayerMovement>();
        Vector3 bodyScale = otherPlayerMovement.body.GetComponent<Transform>().localScale;
        Vector3 leg1Scale = otherPlayerMovement.leg1.GetComponent<Transform>().localScale;
        Vector3 leg2Scale = otherPlayerMovement.leg2.GetComponent<Transform>().localScale;
        if (bodyScale.x > 0)
        {
            bodyScale.x = bodyScale.x * -1;
            leg1Scale.x = leg1Scale.x * -1;
            leg2Scale.x = leg2Scale.x * -1;
        }
        otherPlayerMovement.body.GetComponent<Transform>().localScale = bodyScale;
        otherPlayerMovement.leg1.GetComponent<Transform>().localScale = leg1Scale;
        otherPlayerMovement.leg2.GetComponent<Transform>().localScale = leg2Scale;
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

        // if (rb.velocity.y == 0f)
        //{
        // rb.MovePosition(rb.position + movementVector * movementSpeed * Time.deltaTime);
        rb.AddForce( movementVector * movementSpeed * Time.deltaTime,ForceMode2D.Impulse);
            anim.SetFloat("HorSpeed", Mathf.Abs(horSpeed));
        //}

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
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "CarriableResource" ||
           collider.tag == "Ground" ||
           collider.tag == "Player" ||
           collider.tag == "CarriableResource")
        {
            allowedToJump = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "CarriableResource" ||
            collision.collider.tag == "Ground" ||
            collision.collider.tag == "Player" ||
            collision.collider.tag == "CarriableResource") 
        {
            allowedToJump = true;
        }
    }
}
