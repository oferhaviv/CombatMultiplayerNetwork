using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingTheGame : MonoBehaviour
{
    public Canvas onGame;
    public void DontLeave()
    {
        gameObject.SetActive(false);
        onGame.gameObject.SetActive(true);
    }
    public void onLeave()
    {
        RoomManager.Instance.LeavingTheRoom();
    }
}
