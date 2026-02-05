using System.Collections.Generic;
using UnityEngine;

public class KinTest : MonoBehaviour
{
    private void Start()
    {
        MagicRoomManager.instance.MagicRoomKinectV2Manager.Skeletons += ManageSkeleton;
    }

    private void ManageSkeleton(Dictionary<ulong, Skeleton> skel)
    {
        Debug.Log(skel.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MagicRoomManager.instance.MagicRoomKinectV2Manager.StartStreamingSkeletons(1000);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MagicRoomManager.instance.MagicRoomKinectV2Manager.StopStreamingSkeletons();
        }
    }

    public void printdebug() {
        Debug.Log("Grasp hand");
    }
    public void printdebug2()
    {
        Debug.Log("Release hand");
    }

}