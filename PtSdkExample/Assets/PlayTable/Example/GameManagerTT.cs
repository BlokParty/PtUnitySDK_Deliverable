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

    public InputField inputEmail;
    public InputField inputPassword;
    public Text textLog;

    private void Awake()
    {
        PTTableTop.Initialize(player, playerParent, 3, 8);

        PTTableTop.OnLinked += (player) =>
        {
            UpdateDropdownPlayer();
        };
        PTTableTop.OnDisconnected += (player) =>
        {
            UpdateDropdownPlayer();
        };
        PTTableTop.OnSmartPiece += (sp)=>
        {
            string spText = "x = " + sp.x + ", y = " + sp.y + "\n ID = " + sp.id;
            textSmartPiece.text = spText;
        };
        PTDevice.OnLoggedIn += (account) =>
        {
            textLog.text = "Logged In:\n\n" + account;
        };
        PTDevice.OnRegistered += (account) =>
        {
            textLog.text = "Registered:\n\n" + account;
        };
        PTDevice.OnLogInFailed += (account, echo) =>
        {
            textLog.text = "Login Failed:\n\n" + account + "\n\necho=" + echo;
        };
        PTDevice.OnRegistrationFailed += (account, echo) =>
        {
            textLog.text = "Registration Failed:\n\n" + account + "\n\necho=" + echo;
        };
    }

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
                textStatus.text = "Discoverable at " + PTDevice.localIPAddress + ":" + NetworkServer.listenPort + "";
            else
                textStatus.text = "Inactive";

            //cool down
            yield return new WaitForSeconds(updateTimer);
        }
    }

    public void AddNonPhonePlayer()
    {
        PTTableTop.AddTabletopPlayer();
    }

    void UpdateDropdownPlayer()
    {
        List<string> listPlayerName = new List<string>();
        foreach (PTPlayer player in PTTableTop.players)
        {
            if (player.connectionId > 0)
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
        if (player)
        {
            player.Send(MyMsgType.Message, inputfieldSendMessage.text);
            AddMessage(false, "Me", inputfieldSendMessage.text);
        }
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        MyMessage newMsg = Instantiate(isIn? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<MyMessage>();
        newMsg.UpdateContent(senderName, content);
    }

    public void Login()
    {
        print("Login");
        StartCoroutine(PTDevice.Login(inputEmail.text, inputPassword.text));
    }

    public void Register()
    {
        print("Register");
        StartCoroutine(PTDevice.Register(inputEmail.text, inputPassword.text, 3));
    }
}

public class MyMsgType
{
    public const int Message = 0;
}