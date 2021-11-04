using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviourPun
{

    public float movementSpeed;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            BasicMovement();
        }
    }

    void BasicMovement()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(movementVector * movementSpeed * Time.deltaTime);
    }

}
