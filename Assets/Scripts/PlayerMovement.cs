using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private Rigidbody2D rb;
    public float movementSpeed;
    public float rotationSpeed;
    public Animator anim;
    public Transform modelRef;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        anim = GameObject.Find("model").GetComponent<Animator>();
        modelRef = GameObject.Find("model").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (PV.IsMine) {
            BasicMovement();
            BasicRotation();
        }
    }

    void BasicMovement() 
    {
       //Needs formatting

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        float horSpeed = movementVector.x * movementSpeed * Time.deltaTime;


        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.deltaTime);
        anim.SetFloat("HorSpeed", Mathf.Abs(horSpeed));
        //This is a bad way of implementing sprite mirroring on direction

        if (rb.velocity.x > 0)
        {
            modelRef.transform.localScale = new Vector3(1, 1, -1);

        }
        else
        {
            modelRef.transform.localScale = new Vector3(1, 1, 1);
            //rendRef.flipX = false;
        }
    }

    void BasicRotation()
    {
        /*
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
        */
        //test comment to test git
        //git status on my laptop is up to date , even though i changed the branch and file on other computer

    }
}
