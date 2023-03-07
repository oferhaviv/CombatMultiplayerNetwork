using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HUD : MonoBehaviour
{
    public static HUD Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text roomText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text safeMessage;

    [Header("Debug")]
    [SerializeField] GameObject debugPanel;
    [SerializeField] GameObject debugListItemPrefab;
    [SerializeField] Transform debugListContent;

    public Canvas AreYouSure;
    public Canvas YouLoose;
    //[SerializeField] HealthBarManager Healthbar;
    int cureentScore = -1;
    public int currentHealth = 5;
    Canvas _hud_canvas;
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

    public void Start()
    {
         updateScore();
        if (!Debug.isDebugBuild) {
            Destroy(debugPanel.gameObject);
        }
        _hud_canvas = gameObject.GetComponent<Canvas>();
    }
    public void updateRoom(string roomName, int players, int maxPlayers)
    {
        roomText.text = $"Players {players.ToString()}/{maxPlayers.ToString()}\n{roomName}";

    }

    public void UpdatePlayerColor(Color c)
    {
        Debug.Log($"Color update to {c}");
        scoreText.color = c;
        //Healthbar.initColors(c);
      
    }
    public void updateScore()
    {
        cureentScore++;
        scoreText.text = cureentScore.ToString();
    }

    public void updateHealthWithKill()
    {
        currentHealth--;
        healthText.text = currentHealth.ToString();
        

    }
    public void AskForLeaving()
    {
        gameObject.SetActive(false);
        AreYouSure.gameObject.SetActive(true);
    }
    public void leaveRoom()
    {
        //ask for sure
        
        //check if you are the only one if so close the room
        if (PhotonNetwork.InRoom)
        {
            Room r = PhotonNetwork.CurrentRoom;
            //if (PhotonNetwork.IsMasterClient && r.PlayerCount > 1) MigrateMaster();
            //else

            //_playerController.Die();
            //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);

            PhotonNetwork.LeaveRoom();
            

            
        }

    }

    public void Log(string message)
    {
        if (Debug.isDebugBuild) {
            
            if (debugListContent.childCount >= 15)
            {
                Transform c = debugListContent.GetChild(0);
                Destroy(c.gameObject);
            }
            
            //Instantiate(_message, debugListContent).GetComponent<DebugListItem>().SetUp(message);
            Instantiate(debugListItemPrefab, debugListContent).GetComponent<DebugListItem>().SetUp(message); 
        }
    }

    internal void showSafeMessage(bool v)
    {
        safeMessage.enabled = v;
    }
    public void showYouLoose()
    {
        gameObject.SetActive(false);
        YouLoose.gameObject.SetActive(true);
    }
}
