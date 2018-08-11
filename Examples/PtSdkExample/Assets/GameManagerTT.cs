using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTable;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManagerTT : MonoBehaviour {
    public float updateTimer = 1;
    public PTPlayer player;
    public Transform playerParent;
    public InputField inputfieldSendMessage;
    public Dropdown dropdownPlayer;
    public Text textSmartPiece;
    public Text textStatus;

    public Transform contentMsg;
    public MyMessage msgIn;
    public MyMessage msgOut;

    private void Awake()
    {
        PTTableTop.Initialize(player, playerParent);

        PTTableTop.OnConnected += (player) =>
        {
            UpdateDropdownPlayer();
        };
        PTTableTop.OnDisconnected += (player) =>
        {
            UpdateDropdownPlayer();
        };
        PTTableTop.OnSmartPiece += (sp)=>
        {
            print("ListenSmartPiece: " + sp == null);
            string spText = "x = " + sp.x + ", y = " + sp.y + "\n ID = " + sp.id;
            textSmartPiece.text = spText;
        };

    }

    /*private void Handler_OnSmartPiece(SmartPiece sp)
    {
        print("ListenSmartPiece: " + sp == null);
        string spText = "x = " + sp.x + ", y = " + sp.y + "\n ID = " + sp.id; ;
        textSmartPiece.text = spText;
    }*/

    private void OnEnable()
    {
        StartCoroutine(UpdateContent());
    }

    IEnumerator UpdateContent()
    {
        while (true)
        {
            //Status text
            if (NetworkServer.active)
                textStatus.text = "Discoverable at " + Network.player.ipAddress + ":" + NetworkServer.listenPort + "";
            else
                textStatus.text = "Inactive";

            //cool down
            yield return new WaitForSeconds(updateTimer);
        }
    }

    public void AddNonPhonePlayer()
    {
        PTTableTop.AddNonPhonePlayer();
    }

    void UpdateDropdownPlayer()
    {
        List<string> listPlayerName = new List<string>();
        foreach (PTPlayer player in PTTableTop.players)
        {
            if (player.ConnectionId > 0)
            {
                listPlayerName.Add(player.name);
            }
        }
        dropdownPlayer.ClearOptions();
        dropdownPlayer.AddOptions(listPlayerName);
    }

    public void Send()
    {
        PTPlayer player = PTTableTop.FindPlayer(dropdownPlayer.captionText.text);
        player.NotifyHandheld(MyMsgType.Message, inputfieldSendMessage.text);
        AddMessage(false, "Me", inputfieldSendMessage.text);
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        MyMessage newMsg = Instantiate(isIn? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<MyMessage>();
        newMsg.UpdateContent(senderName, content);
    }
}

public class MyMsgType
{
    public const int Message = 0;
}