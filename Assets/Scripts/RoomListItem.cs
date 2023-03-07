using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = $"{_info.Name} ( {_info.PlayerCount.ToString()} / {Launcher.Instance.maxPlayersInRoom.ToString()} )";
        this.GetComponent<Button>().enabled = !IsRoomFull();
        if (_info.PlayerCount <= 0)
        {
            Destroy(gameObject);
        }


    }
    public void OnClick() {

        Launcher.Instance.JoinRoom(info);
    }
    public bool IsRoomFull()
    {
        if (info.PlayerCount >= Launcher.Instance.maxPlayersInRoom)
        {
            return true;
        }
        return false;
    }
}
