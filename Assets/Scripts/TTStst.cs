using System.Collections;
using UnityEngine;

public class TTStst : MonoBehaviour
{
    private MagicRoomTextToSpeachManager man;
    private bool callme = true;
    public bool startcallign = false;

    // Start is called before the first frame update
    private void Start()
    {
        man = MagicRoomManager.instance.MagicRoomTextToSpeachManager;
        man.EndSpeak += Man_EndSpeak;
    }

    private void Update()
    {
        if (startcallign)
        {
            if (callme)
            {
                callPlayer();
            }
        }
    }

    private void Man_EndSpeak()
    {
        callme = true;
    }

    private IEnumerator call()
    {
        yield return new WaitForSeconds(0);
        man.GenerateAudioFromText("Nel mezzo del cammin di nostra vita mi ritrovai per una selva oscura, ché la diritta via era smarrita. ");
    }

    // Update is called once per frame
    private void callPlayer()
    {
        callme = false;
        StartCoroutine(call());
    }
}