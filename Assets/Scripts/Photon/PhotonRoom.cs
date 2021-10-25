using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;

    public int multiplayerScene;
    public int currentScene;

    Player[] photonPlayers;
    public int playersInGame;
    public int playersInRoom;
    public int myNumberInRoom;

    private void Awake()
    {
        if (PhotonRoom.room == null) {
            PhotonRoom.room = this;
        }
        else if (PhotonRoom.room != this) {
            Destroy(PhotonRoom.room.gameObject);
            PhotonRoom.room = this;
        }

        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }


    public override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the " + PhotonNetwork.CurrentRoom.Name);
        base.OnJoinedRoom();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if (!PhotonNetwork.IsMasterClient)  {
            return;
        }

        StartGame();
    }

    void StartGame() 
    {
        Debug.Log("Loading Level " + multiplayerScene.ToString());
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) 
    {
        currentScene = scene.buildIndex;

        if (currentScene == multiplayerScene) {
            CreatePlayer();
        }
    }

    private void CreatePlayer() {
        PhotonNetwork.Instantiate(
            Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), 
            transform.position, Quaternion.identity, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
    }
}
