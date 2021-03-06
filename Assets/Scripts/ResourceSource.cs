using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ResourceSource : MonoBehaviourPun,IPunObservable
{
    // Start is called before the first frame update

    [SerializeField]
    int resourceAmount=3;

    [SerializeField]
    double maxResourceHealth=100;
    double resourceHealth;

    public string resourceToSpawnName;
    public string resourceLeftoverName;

    //SpriteRenderer sR;

    void Start()
    {
        resourceHealth = maxResourceHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(double damageAmout)
    {
        photonView.RequestOwnership();

        //shake();

        changeColor();

        resourceHealth -= damageAmout;

        Debug.Log("Remaining health: "+resourceHealth);

        if (resourceHealth <= 0)
        {
            destroySelf();
        }
       
    }

    void destroySelf()
    {

        Debug.Log("Imded");

        // Instantiate resource leftover
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceLeftoverName), transform.position, Quaternion.identity, 0);

        // Instantiate carriable resources
        spawnResources(resourceToSpawnName, resourceAmount);

        PhotonNetwork.Destroy(this.gameObject);
    }

    IEnumerator changeColor()
    {
        SpriteRenderer sR = GetComponent<SpriteRenderer>();
        Color origin = sR.color;
        Color change = new Color(origin.r+128,origin.g + 128, origin.b + 128);

        sR.color = change;

        yield return new WaitForSeconds(2f);

        sR.color = origin;
    }

    IEnumerator shake()
    {
        float offset = 1f;
        float dec = 0.02f;
        Vector3 origin = transform.position;

        for(int i = 0; i < 20; i++)
        {
            transform.Translate((new Vector3(transform.position.x + offset, origin.y,origin.z))); 
            offset -= dec;
            offset *= -1;
            dec *= -1;
            yield return new WaitForSeconds(0.3f);
        }

        transform.position = origin;
    }

    void spawnResources(string resourceType, int num)
    { 
        Vector3 spawnPosition = transform.position;

        int offset = 1;

        spawnPosition.x += offset;
        spawnPosition.z = 0;

        for(int i = 0; i < num; i++)
        { 
            spawnPosition.y += offset * (i+0.6f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resourceToSpawnName), spawnPosition, Quaternion.identity, 0);
        }
 
    }
    //RPC would suit this better
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(resourceHealth);
        }
        else
        {
            this.resourceHealth = (double)stream.ReceiveNext();
        }
    }
}
