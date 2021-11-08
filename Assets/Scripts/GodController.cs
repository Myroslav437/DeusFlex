using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviourPun
{



    private PhotonView PV;
    public Camera godCam;

    public GameObject[] level1Skills;
    public GameObject[] level2Skills;
    public GameObject[] level3Skills;
    public GameObject[] level4Skills;

    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        disableAllSkills();

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            BasicMovement();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            enableFirstLevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            enableSecondLevel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            enableThirdLevel();
            enableFourthLevel();
        }
    }

    void BasicMovement()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(movementVector * movementSpeed * Time.deltaTime);
    }

    void enableFirstLevel()
    {
        foreach(GameObject skill in level1Skills)
        {
            skill.SetActive(true);
        }
    }

    void enableSecondLevel()
    {
        foreach (GameObject skill in level2Skills)
        {
            skill.SetActive(true);
        }
    }

    void enableThirdLevel()
    {
        foreach (GameObject skill in level3Skills)
        {
            skill.SetActive(true);
        }
    }

    void enableFourthLevel()
    {
        foreach (GameObject skill in level4Skills)
        {
            skill.SetActive(true);
        }
    }

    void disableSkills(GameObject[] skills)
    {
        foreach(GameObject skill in skills)
        {
            skill.SetActive(false);
        }
    }

    void disableAllSkills()
    {
        disableSkills(level1Skills);
        disableSkills(level2Skills);
        disableSkills(level3Skills);
        disableSkills(level4Skills);
    }
}
