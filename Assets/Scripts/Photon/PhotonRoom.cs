using System.IO;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView PV;
    public InputField nicknameInput;

    public int waitScene;
    public int gameScene;
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
        PhotonNetwork.NickName = GetNickName();

        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        StartGame();
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (currentScene == waitScene) {
            OnWaitSceneLoaded();
        }
        else if (currentScene == gameScene) {
            OnGameSceneLoaded();
        }
    }

    private GameObject CreatePlayer()
    {
        return PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"),
                transform.position, Quaternion.identity, 0);
    }

    void OnWaitSceneLoaded()
    {
        GameObject player = CreatePlayer();
        player.GetComponent<PhotonPlayer>().InstantiateAvatar(Path.Combine("PhotonPrefabs", "PlayerWaitAvatar"));
    }

    void OnGameSceneLoaded()
    {
        GameObject player = CreatePlayer();
        player.GetComponent<PhotonPlayer>().InstantiateAvatar(Path.Combine("PhotonPrefabs", "PlayerGameAvatar"));
    }

    void StartGame()
    {
        Debug.Log("Loading Level " + waitScene.ToString());
        PhotonNetwork.LoadLevel(waitScene);
    }

    string GetNickName() 
    {
        string nick = nicknameInput.text;
        if (!CheckNickname(nick)) {
            nick = "Player" + Random.Range(0, 1000).ToString();
        }

        return nick;
    }

    public bool CheckNickname(string nick)
    {
        if (nick.Length == 0) {
            return false;
        }
        else {
            return true;
        }
    }
}
