using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTable;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class GameManagerHH : MonoBehaviour {
    public Transform contentMsg;
    public MyMessage msgIn;
    public MyMessage msgOut;
    public InputField inputfieldSendMessage;

    public InputField inputEmail;
    public InputField inputPassword;
    public Text textLog;

    private void Awake()
    {
        PTHandheld.Initialize();
        PTHandheld.OnReceived += (PTMessage msg) =>
        {
            switch (msg.type)
            {
                case MyMsgType.Message:
                    AddMessage(true, "Myself on TT", msg.text);
                    break;
            }
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

    public void Send()
    {
        PTHandheld.Send(MyMsgType.Message, inputfieldSendMessage.text);
        AddMessage(false, "Me", inputfieldSendMessage.text);
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        MyMessage newMsg = Instantiate(isIn ? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<MyMessage>();
        newMsg.UpdateContent(senderName, content);
    }

    public void Login()
    {
        StartCoroutine(PTDevice.Login(inputEmail.text, inputPassword.text));
    }
    
    public void Register()
    {
        StartCoroutine(PTDevice.Register(inputEmail.text, inputPassword.text, 3));
    }
}
