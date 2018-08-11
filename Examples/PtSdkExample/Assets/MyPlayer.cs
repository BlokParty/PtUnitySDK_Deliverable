using PlayTable;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : PTPlayer{
    public float updateTimer = 1;
    public Text textName;

    private void Awake()
    {
        OnCommanded += (msg) =>
        {
            print("OnNotified: " + msg);
            switch (msg.type)
            {
                case MyMsgType.Message:
                    FindObjectOfType<GameManagerTT>().AddMessage(true, name, msg.value);
                    break;
            }
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
            textName.text = ConnectionId > 0 ? "Player " + ConnectionId : "Non-Phone";
            yield return new WaitForSeconds(updateTimer);
        }
    }
}
