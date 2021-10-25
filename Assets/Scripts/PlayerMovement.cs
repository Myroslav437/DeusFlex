using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController myCC;
    public float movementSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>();
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
        if (Input.GetKey(KeyCode.W)) {
            myCC.Move(transform.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            myCC.Move(-transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            myCC.Move(-transform.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            myCC.Move(transform.right * movementSpeed * Time.deltaTime);
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
    }
}
