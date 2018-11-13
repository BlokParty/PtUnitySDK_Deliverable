using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayTable;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class GameManagerHH : MonoBehaviour {
    public Transform contentMsg;
    public ExampleMessage msgIn;
    public ExampleMessage msgOut;
    public InputField inputfieldSendMessage;

    public InputField inputEmail;
    public InputField inputPassword;
    public Text textLog;

    private void Awake()
    {
        PTHandheld.OnReceivedText += (type, msg) =>
        {
            switch (type)
            {
                case MyMsgType.Message:
                    AddMessage(true, "Myself on TT", msg);
                    break;
            }
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

    public void Send()
    {
        PTHandheld.SendText(MyMsgType.Message, inputfieldSendMessage.text);
        AddMessage(false, "Me", inputfieldSendMessage.text);
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        ExampleMessage newMsg = Instantiate(isIn ? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<ExampleMessage>();
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
