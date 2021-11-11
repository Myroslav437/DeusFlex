using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodController : MonoBehaviourPun
{

    // public GameObject playerReference;

    private PhotonView PV;
   // public Camera godCam;
   // public GameObject SkillsAndUI;

    public GameObject[] level1Skills;
    public GameObject[] level2Skills;
    public GameObject[] level3Skills;
    public GameObject[] level4Skills;

    public float movementSpeed;
    int currentLevel = 0;

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine) {
            SetActiveAllChildren(this.transform, true);

            disableAllSkills();
            enableFirstLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            BasicMovement();
        }

        if (Input.GetKeyDown(KeyCode.G) && PV.IsMine)
        {
            // photonView.RPC("enableSelf", RpcTarget.All);

            PhotonNetwork.Destroy(gameObject);
        }

        // this should be done by events, but i dont care
        synchLevels();

    }

    /*
    [PunRPC]
    void enableSelf() 
    {
        playerReference.SetActive(true);
    }
    */

    void synchLevels()
    {
        currentLevel = GameObject.FindGameObjectWithTag("ShrineTeam1").GetComponent<Shrine>().getShrineLevel();

        switch (currentLevel)
        {
            case (0):
                enableFirstLevel();
                break;
            case (1):
                enableSecondLevel();
                break;
            case (2):
                enableThirdLevel();
                break;
            case (3):
                enableFourthLevel();
                break;
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
        foreach (GameObject skill in level1Skills)
        {
            skill.SetActive(true);
        }

        foreach (GameObject skill in level2Skills)
        {
            skill.SetActive(true);
        }
    }

    void enableThirdLevel()
    {
        foreach (GameObject skill in level1Skills)
        {
            skill.SetActive(true);
        }

        foreach (GameObject skill in level2Skills)
        {
            skill.SetActive(true);
        }

        foreach (GameObject skill in level3Skills)
        {
            skill.SetActive(true);
        }
    }

    void enableFourthLevel()
    {
        foreach (GameObject skill in level1Skills)
        {
            skill.SetActive(true);
        }

        foreach (GameObject skill in level2Skills)
        {
            skill.SetActive(true);
        }

        foreach (GameObject skill in level3Skills)
        {
            skill.SetActive(true);
        }

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

    private void SetActiveAllChildren(Transform transform, bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);

            SetActiveAllChildren(child, value);
        }
    }
}
