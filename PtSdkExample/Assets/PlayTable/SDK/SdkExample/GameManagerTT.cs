using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTable;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManagerTT : MonoBehaviour {
    public float updateTimer = 1;
    public PTPlayer player;
    public Transform playerParent;
    public InputField inputfieldSendMessage;
    public Dropdown dropdownPlayer;
    public Text textSmartPiece;
    public Text textStatus;

    public Transform contentMsg;
    public ExampleMessage msgIn;
    public ExampleMessage msgOut;

    public InputField inputEmail;
    public InputField inputPassword;
    public Text textLog;

    private void Awake()
    {

        PTTableTop.OnLinked += (PTPlayer player) =>
        {
            Debug.Log("Linked: " + player.name);
            UpdateDropdownPlayer();
        };
        PTTableTop.OnUnlinked += (PTPlayer player) =>
        {
            Debug.Log("Unlinked: " + player.name);
        };
        PTTableTop.OnDisconnected += (int id) =>
        {
            Debug.Log("Disconnected: " + player.name);
            UpdateDropdownPlayer();
        };
        PTTableTop.OnConnected += (int id) =>
        {
            Debug.Log("Connected: connectionId=" + id);
        };
        PTTableTop.OnSmartPiece += (sp)=>
        {
            string spText = "x = " + sp.x + ", y = " + sp.y + "\n ID = " + sp.id;
            textSmartPiece.text = spText;
        };
        PTPlatform.OnResultLogIn += (PTResult<PTKey> result) =>
        {
            if (result.succeeded)
            {
                textLog.text = "Logged In:\n\n" + result.content[0];
            }
            else
            {
                textLog.text = "Login Failed:\n\n" + result.echo;

            }
        };
        PTPlatform.OnResultRegistration += (PTResult<PTKey> result) =>
        {
            if (result.succeeded)
            {
                textLog.text = "Registerd:\n\n" + result.content[0];
            }
            else
            {
                textLog.text = "Registration Failed:\n\n" + result.echo;

            }
        };

    }
    private void Start()
    {

        /*StartCoroutine(PTDevice.PullGames(null));
        StartCoroutine(PTDevice.PullPreDefinedAvators());
        StartCoroutine(PTDevice.PullProfile());
        StartCoroutine(PTDevice.PushProfile(
            PTSelfEvaluation.geek
            , "Wang"
            , "Jacky"
            , 3
            , "jacky"
            , "An awesome guy @ Blok Party"));*/
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

    void UpdateDropdownPlayer()
    {
        List<string> listPlayerName = new List<string>();
        foreach (PTPlayer player in PTTableTop.players)
        {
            if (player.isLinked)
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
            player.SendText(MyMsgType.Message, inputfieldSendMessage.text);
            AddMessage(false, "Me", inputfieldSendMessage.text);
        }
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        ExampleMessage newMsg = Instantiate(isIn? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<ExampleMessage>();
        newMsg.UpdateContent(senderName, content);
    }

    public void Login()
    {
        PTPlatform.Login(inputEmail.text, inputPassword.text);
    }

    public void Register()
    {
        PTPlatform.Register(inputEmail.text, inputPassword.text, 3);
    }

    
}

public class MyMsgType
{
    public const int Message = 0;
}