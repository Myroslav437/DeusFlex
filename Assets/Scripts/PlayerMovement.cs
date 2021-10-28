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
    private Animator anim;
    public LayerMask resourcesMask;
    public double playerDamage=10;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        anim = GameObject.Find("model").GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
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

    void attack()
    {
        //states it's null despite the check
        if (Physics2D.OverlapCircle(transform.position, 1, resourcesMask) != null)
        {
            Collider2D hitObject = Physics2D.OverlapCircle(transform.position, 1, resourcesMask);

            hitObject.GetComponent<ResourceSource>().takeDamage(playerDamage);
        }
           
    }
}
