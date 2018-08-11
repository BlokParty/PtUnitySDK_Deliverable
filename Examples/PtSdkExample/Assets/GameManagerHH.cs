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

    private void Awake()
    {
        PTHandheld.Initialize();
        PTHandheld.OnNotified += (msg) =>
        {
            switch (msg.type)
            {
                case MyMsgType.Message:
                    AddMessage(true, "Myself on TT", msg.value);
                    break;
            }
        };
    }

    public void Send()
    {
        PTHandheld.Command(MyMsgType.Message, inputfieldSendMessage.text);
        AddMessage(false, "Me", inputfieldSendMessage.text);
    }

    public void AddMessage(bool isIn, string senderName, string content)
    {
        MyMessage newMsg = Instantiate(isIn ? msgIn.gameObject : msgOut.gameObject, contentMsg).GetComponent<MyMessage>();
        newMsg.UpdateContent(senderName, content);
    }
}
