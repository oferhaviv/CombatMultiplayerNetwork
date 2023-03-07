using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] InputField roomNameInputField;
    [SerializeField] InputField userNickName;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomName;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] public byte maxPlayersInRoom;
    
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
    void Start()
    {
        userNickName.text = "Player " + Random.Range(0, 1000).ToString("0000");
        roomNameInputField.text = "Room " + Random.Range(0, 100).ToString("000");



    }
    public void EnterLobby()
    {
        MenuManager.Instance.OpenMenu("loading");
        if (string.IsNullOrEmpty(userNickName.text))
        {
            return;
        }
        PhotonNetwork.NickName = userNickName.text;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to  Master");

    }

    internal void quit()
    {
        PhotonNetwork.DestroyAll();
        MenuManager.Instance.OpenMenu("title");
    }

    public void StartGame()
    {

        int level = (int)PhotonNetwork.CurrentRoom.CustomProperties["level"];
        Hashtable hash = new Hashtable();
        hash.Add("gameStarted", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        //PhotonNetwork.LoadLevel(1);
        //PhotonNetwork.LoadLevel(2);
        PhotonNetwork.LoadLevel(level);

    }


    /// <summary>
    /// First connection to PUN
    /// After PhotonNetwork.ConnectUsingSettings();
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// After first connection completed
    /// OnConnectedToMaster() is done
    /// </summary>
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");


        
    }

    // Update is called once per frame
    public void CreateRoom()
    {

        //get option from menu
        int level = MenuManager.Instance.getOpenedMenu().getLevel();
        Debug.LogError($"Selected LeveL is {level}");
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayersInRoom,
            BroadcastPropsChangeToAll = false,
            CleanupCacheOnLeave = true
        };


        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add("level", level);

        PhotonNetwork.CreateRoom(roomNameInputField.text,roomOptions,null);
        MenuManager.Instance.OpenMenu("loading");


    }

    
    public override void OnJoinedRoom()
    {

        Debug.Log("Player Joined a room");
        Room info = PhotonNetwork.CurrentRoom;
        if (info.CustomProperties.ContainsKey("gameStarted"))
        {
            
            if ((bool)info.CustomProperties["gameStarted"])
            {
                Debug.Log("Game Already started");
                MenuManager.Instance.CloseAllMenus(true);
                return;
            }

        }
        
        MenuManager.Instance.OpenMenu("roomMenu");
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        foreach (Transform t in playerListContent)
        {
            Destroy(t.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player p in players)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(p);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("errorMenu");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        RoomInfo roomInfo = PhotonNetwork.CurrentRoom;
        if (roomInfo.PlayerCount <= 0 )
        {
            roomInfo.RemovedFromList = true;
        }
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = $"Room joining failed: (E{returnCode}) {message}";
        MenuManager.Instance.OpenMenu("errorMenu");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom called");
        PhotonNetwork.LoadLevel(0);
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate( List<RoomInfo> roomList)
    {

        foreach (Transform t in roomListContent)
        {
            Destroy(t.gameObject);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomInfo);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gameStarted"))
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["gameStarted"])
            {
                
                HUD.Instance.updateRoom(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, maxPlayersInRoom);
            }
        }

    }




}
