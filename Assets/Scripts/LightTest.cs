using UnityEngine;

public class LightTest : MonoBehaviour
{
    private MagicRoomTextToSpeachManager stt;

    private void Start()
    {
        stt = MagicRoomManager.instance.MagicRoomTextToSpeachManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            stt.GenerateAudioFromText("Prova", stt.ListOfVoice[0]);
        }
    }
}