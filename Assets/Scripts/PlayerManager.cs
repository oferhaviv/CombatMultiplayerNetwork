using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    //color array 
    Color[] colorArray = { Color.red, Color.green, Color.yellow, Color.blue, Color.magenta, Color.black, Color.white, Color.gray };
    Vector3[] possibleLocations = { 
        new Vector3(-6.5f, 0, 1),
        new Vector3(0, 0, -6.5f),
        new Vector3(0,0,6.5f),
        new Vector3(6.5f, 0, 0),
        new Vector3(6.5f, 0, -6.5f),
        new Vector3(-6.5f, 0, 6.5f),
        new Vector3(-6.5f, 0, -6.5f),
        new Vector3(6.5f, 0, 6.5f)

    };
    PhotonView PV;
    GameObject go;
    GameObject go_bullet;
    

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            //am i the owner?
            CreateController();
        }
        
    }


    void CreateController()
    {
        MenuManager.Instance.CloseAllMenus(true);

        bool checkIfNumberExists = false;
        int randomValueForPlayer = Random.Range(0, colorArray.Length); 

        checkIfNumberExists = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(randomValueForPlayer.ToString());

        while (checkIfNumberExists)
        {
            randomValueForPlayer = Random.Range(0, colorArray.Length);
            checkIfNumberExists = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(randomValueForPlayer.ToString());
        }
        
        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add(randomValueForPlayer.ToString(),true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);

        // Create Tank
        go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerTank"), possibleLocations[randomValueForPlayer], Quaternion.identity, 0, new object[] { PV.ViewID }); 
        // Create bullet related to tank
        go_bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), possibleLocations[randomValueForPlayer], Quaternion.identity);
        
        //Call initiate Methods (RPC-inside)
        go_bullet.GetComponent<Bullet>().updateTankObject(go);
        go.GetComponent<PlayerController>().updateColor(randomValueForPlayer);
        go_bullet.GetComponent<Bullet>().updateColor(randomValueForPlayer);
    }

    


    public void Die(bool returnToGame=false)
    {
        Debug.Log("remove value from customer prop");
        PhotonNetwork.CurrentRoom.CustomProperties.Remove(go.GetComponent<PlayerController>()._playerColorIndex.ToString());
        PhotonNetwork.Destroy(go);
        PhotonNetwork.Destroy(go_bullet);


        if (returnToGame)
            CreateController();

    }

    
}
