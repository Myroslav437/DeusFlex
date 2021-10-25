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
    

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.deltaTime);

       // if (Input.GetKey(KeyCode.W)) {
       //    // rb.Move(transform.up * movementSpeed * Time.deltaTime);
       //     rb.AddForce(Vector2.up * movementSpeed * Time.deltaTime);
       // }
       // if (Input.GetKey(KeyCode.A)) {
       //    // rb.Move(-transform.right * movementSpeed * Time.deltaTime);
       //     rb.AddForce(Vector2.left * movementSpeed * Time.deltaTime);
       // }
       // if (Input.GetKey(KeyCode.S)) {
       //    //rb.Move(-transform.up * movementSpeed * Time.deltaTime);
       //     rb.AddForce(Vector2.down * movementSpeed * Time.deltaTime);
       // }
       // if (Input.GetKey(KeyCode.D)) {
       //   //  rb.Move(transform.right * movementSpeed * Time.deltaTime);
       //     rb.AddForce(Vector2.right * movementSpeed * Time.deltaTime);
       // }

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
