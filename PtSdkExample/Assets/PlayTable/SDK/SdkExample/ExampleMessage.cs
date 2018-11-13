using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleMessage : MonoBehaviour {
    public Text textPlayerName;
    public Text textMsg;

    public void UpdateContent(string playerName, string msg)
    {
        textPlayerName.text = playerName;
        textMsg.text = msg;
        textMsg.text = textMsg.text;
    }
}
