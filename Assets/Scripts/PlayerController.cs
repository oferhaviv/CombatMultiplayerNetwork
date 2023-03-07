using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviourPunCallbacks
{
    //color array 
    public Color[] colorArray = { Color.red, Color.green, Color.yellow, Color.blue, Color.magenta, Color.black, Color.white, Color.gray };

    [Header("Movment")]
    public float rotationSpeed = 16f;
    public float moveSpeed = 620f;
    public float maxSpeed = 2f;

    [Header("Hit")]
    public bool isSpintout;
    public float timer;
    public Vector3 rotationEulerAngleVelocity = new Vector3(0, 800, 0);
    public int rotationSpeedDec = 12;
    public int safeTimeAfterShoot = 5;

    [Header("Animation")]
    public Renderer frameone;
    public Renderer frametwo;
    
    [HideInInspector]
    public PhotonView PV;
    private Rigidbody rb;
    private bool isSafe =false;
    private HUD hud;

    //audio sources
    AudioSource deadSound;
    AudioSource moveSound;
    private bool isMute = false;
    bool youDead = false;
    [HideInInspector]
    public int _playerColorIndex = -1;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        hud = HUD.Instance;
        List<AudioSource> allAudios = new List<AudioSource>();
        GetComponents(allAudios);
        foreach (AudioSource audio in allAudios)
        {
            if (audio.clip.name == "tankded") deadSound = audio;
            if (audio.clip.name == "tankmove") moveSound = audio;
        }
    }


    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);

        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gameStarted"))
            {
                if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["gameStarted"])
                {
                    HUD.Instance.updateRoom(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (isSpintout) return;
        if (youDead) return;
        //sides do a turn base
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }

        // forward and backward drive
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(moveSpeed * Time.deltaTime * transform.right);
                if (!moveSound.isPlaying) moveSound.Play();
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(-moveSpeed * Time.deltaTime * transform.right);
                if (!moveSound.isPlaying) moveSound.Play();
            }
        }

        // Stop moving sound on stop
        if (rb.velocity.magnitude == 0)
            if (moveSound.isPlaying) moveSound.Stop(); 

    }

    [PunRPC]
    public void updateColor(int playerColorIndex)
    {

        HUD.Instance.Log($"Player color index is: {playerColorIndex}");
        Color playerColor = colorArray[playerColorIndex];
        _playerColorIndex = playerColorIndex;

        frameone.material.color = playerColor;
        frametwo.material.color = playerColor;

        if (PV.IsMine)
        {
            photonView.RPC("updateColor",RpcTarget.OthersBuffered , playerColorIndex);
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            GameObject shootingTank = collision.gameObject.GetComponent<Bullet>()._Tank;
            int shootingTankViewID = shootingTank.GetPhotonView().ViewID;
            shootingTank.GetComponent<PlayerController>().hitByTank(shootingTankViewID, PV.ViewID);

            //HUD.Instance.Log($"Tank {PV.ViewID} hit by {collision.gameObject.GetComponent<Bullet>().PVID}");
            HUD.Instance.Log($"Tank {PV.ViewID} hit by {shootingTankViewID}");
            HUD.Instance.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} hit by {collision.gameObject.GetComponent<Bullet>().playerID}");

            //hitByTank(collision.gameObject.GetComponent<Bullet>().PVID, PV.ViewID);
            //PV.RPC( "hitByTank", RpcTarget.Others, collision.gameObject.GetComponent<Bullet>().PVID, PV.ViewID);

            //PV.RPC("hitByTank", RpcTarget.All, collision.gameObject.GetComponent<Bullet>().PVID, PV.ViewID);
            PV.RPC("hitByTank", RpcTarget.Others, shootingTankViewID, PV.ViewID);

        }
    }

    [PunRPC]
    public void hitByTank(int shootingTankViewID, int gotHitTankViewID)
    {
        
        if (!PV.IsMine) return;
        
        HUD.Instance.Log($"Shooter {shootingTankViewID} got hit {gotHitTankViewID} me {PV.ViewID}");

        if (PV.ViewID == gotHitTankViewID)  
        {
            if (!PV.IsMine) return;
            if (isSafe)  return;

            hud.updateHealthWithKill();
            if (hud.currentHealth <= 0)
            {
                hud.showYouLoose();
                frameone.enabled = false;
                frametwo.enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;

                youDead = true;
            }
            else
            {
                //update player data
                ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
                data.Add("safe", true);
                data.Add("ViewID", gotHitTankViewID);
                PhotonNetwork.SetPlayerCustomProperties(data);
                //HUD.Instance.Log($"custom property updated with: {data}");
                timer = 10;
                StartCoroutine("spin");
            }
            deadSound.Play();
        }
        if (PV.ViewID == shootingTankViewID) //shoot yourself
        {
            if (!PV.IsMine) return;
            if (safeTanks.Contains(gotHitTankViewID)) { return; }
            
            
            //ExitGames.Client.Photon.Hashtable data = PhotonNetwork.LocalPlayer.CustomProperties;
            //if (data.ContainsKey("safe"))
            //if the other one not spining you desrve a point
            //HUD.Instance.Log($"You are player ID: {PhotonNetwork.LocalPlayer.ActorNumber}");
            HUD.Instance.Log($"You are the shooter {PV.ViewID}");
            hud.updateScore();
        }
    }
    IEnumerator spin()
    {
        while (timer > 0)
        {
            isSpintout = true;
            Quaternion deltaRotation = Quaternion.Euler(rotationEulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            timer = timer -rotationSpeedDec * Time.deltaTime;

            yield return null;

        }
        isSpintout = false;

        StartCoroutine(safeTime());
        yield return null;


    }
    IEnumerator safeTime()
    {
        isSafe = true;
        HUD.Instance.showSafeMessage(true);
        HUD.Instance.Log("you are safe now");
        yield return new WaitForSeconds(safeTimeAfterShoot);
        
        isSafe = false;
        HUD.Instance.showSafeMessage(false);
        HUD.Instance.Log("you no loger safe");
        //update player data
        ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
        data.Add("safe", false);
        data.Add("ViewID", PV.ViewID);
        PhotonNetwork.SetPlayerCustomProperties(data);
    }

    private List<int> safeTanks = new List<int>();
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PV.IsMine) HUD.Instance.Log($"player {targetPlayer.ActorNumber} update properties");


        if (changedProps.ContainsKey("safe"))
        {
            if (PV.IsMine) HUD.Instance.Log($"player {targetPlayer.ActorNumber} update properties with {changedProps["safe"]}");
            if ((bool)changedProps["safe"])
            {
                safeTanks.Add((int)changedProps["ViewID"]);
            }
            else
            {
                safeTanks.Remove((int)changedProps["ViewID"]);
            }

        }
    }
    public void Mute()
    {
        isMute = !isMute;
        AudioListener.volume = isMute ? 0 : 1;

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("updateRoomHUD", RpcTarget.All);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("updateRoomHUD", RpcTarget.All);
    }
    [PunRPC]
    public void updateRoomHUD()
    {
        HUD.Instance.updateRoom(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }
}
