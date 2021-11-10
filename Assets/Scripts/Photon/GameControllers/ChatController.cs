using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    public GameObject sendButton;
    public InputField messageInput;
    public Canvas chatCanvas;
    public Text chatText;

    private PhotonView PV;
    private string chatHistory;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        chatCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            if (chatCanvas.gameObject.activeSelf) {
                if (messageInput.isFocused) {
                    // freeze player;
                    return;
                }
                else {
                    chatCanvas.gameObject.SetActive(false);
                }
            }
            else {
                chatCanvas.gameObject.SetActive(true);
            }
        }
    }

    public void onSendButtonPressed() 
    {
        string message = messageInput.text;
        PV.RPC("RPC_AddMessage", RpcTarget.MasterClient, message, LocalPlayerInfo.LPI.myNickName);
    }

    [PunRPC]
    void RPC_AddMessage(string message, string authorNick) 
    {
        chatHistory += authorNick + ": " + message + System.Environment.NewLine;
        PV.RPC("RPC_SetChatHistory", RpcTarget.AllBufferedViaServer, chatHistory);
    }

    [PunRPC]
    void RPC_SetChatHistory(string Newchat) 
    {
        chatHistory = Newchat;
        chatText.text = chatHistory;
    }
}
