using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    GameObject go;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance)
        { 
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex >=1)
        {
            MenuManager.Instance.setActiveAllMenu(false);
            go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }


    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void LeavingTheRoom()
    {
        Debug.Log("Call Die in playerMAnager");
        go.GetComponent<PlayerManager>().Die();
        //Debug.Log("destory playerMAnager");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Destroy(go);
        Debug.Log("loading level0");
        PhotonNetwork.LoadLevel(0);
        
        MenuManager.Instance.backFromGame();

        if (!PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinLobby();
        else
            MenuManager.Instance.OpenMenu("title");
        //Destroy(gameObject);
    }
    
}
