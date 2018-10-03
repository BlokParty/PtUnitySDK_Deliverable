using PlayTable;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviour{
    public float updateTimer = 1;
    public Text textName;
    private PTPlayer ptPlayer;

    private void Awake()
    {
        ptPlayer = GetComponent<PTPlayer>();
        ptPlayer.OnReceived += (msg) =>
        {
            switch (msg.type)
            {
                case MyMsgType.Message:
                    FindObjectOfType<GameManagerTT>().AddMessage(true, name, msg.text);
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
            textName.text = ptPlayer.connectionId > 0 ? "Player " + ptPlayer.connectionId : "Non-Phone";
            yield return new WaitForSeconds(updateTimer);
        }
    }
}
