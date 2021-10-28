using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalPlayerInfo : MonoBehaviour
{
    public static LocalPlayerInfo LPI;
    public GameObject networkPlayer;
    public string myNickName;
    public int mySelectedCharacter;
    public GameObject[] allCharacters;

    private void OnEnable()
    {
        if (LocalPlayerInfo.LPI == null) {
            LocalPlayerInfo.LPI = this;
        }
        else if(LocalPlayerInfo.LPI != this) {
            Destroy(LocalPlayerInfo.LPI.gameObject);
            LocalPlayerInfo.LPI = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter")) {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }

        if (PlayerPrefs.HasKey("MyNickName")) {
            myNickName = PlayerPrefs.GetString("MyNickName");
        }
        else {
            myNickName = "";
            PlayerPrefs.SetString("MyNickName", myNickName);
        }
    }

    public void UpdateNickName() 
    {
        if (networkPlayer != null)
        {
            PhotonPlayer pl = networkPlayer.GetComponent<PhotonPlayer>();
            if (pl.myAvatar != null) { 
                pl.myAvatar.GetComponent<NicknameLabel>().nickName = myNickName;
            }
        }
    }
}
