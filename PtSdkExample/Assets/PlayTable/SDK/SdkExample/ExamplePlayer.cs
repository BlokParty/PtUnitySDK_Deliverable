using PlayTable;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExamplePlayer : MonoBehaviour{
    public float updateTimer = 1;
    public Text textName;
    private PTPlayer ptPlayer;

    private void Awake()
    {
        ptPlayer = GetComponent<PTPlayer>();
        ptPlayer.OnReceivedText += (type, msg) =>
        {
            switch (type)
            {
                case MyMsgType.Message:
                    FindObjectOfType<GameManagerTT>().AddMessage(true, name, msg);
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
            textName.text = ptPlayer.name;
            yield return new WaitForSeconds(updateTimer);
        }
    }
}
