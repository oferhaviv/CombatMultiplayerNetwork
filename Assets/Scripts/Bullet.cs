using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Bullet : MonoBehaviourPunCallbacks
{

    [HideInInspector]
    public GameObject _Tank;
    PhotonView PV;

    Transform Tank;
    public float movespeed;
    public KeyCode fireKey = KeyCode.Space;

    public Vector3 bulletOffset = new Vector3(0f, .2174f, 0f);
    

    bool bulet;
    Collider col;
    Rigidbody rb;
    Renderer rend;
    
    //Collider coliderofb;
    float hits;
    Quaternion q;
    Vector3 startpos;
    float ypaus;

    //audio sources
    AudioSource shoot;
    AudioSource bounce;

    public int PVID;
    public int playerID;
    /*
    public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
    {
        Debug.Log($"Instat data arraived");
        object[] inData = info.photonView.InstantiationData;
        updateTankObject((int)inData[0]);
        
    }*/

    private void Awake()
    {
        Debug.Log($"Bullet Awaked called");
        

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        List<AudioSource> allAudios = new List<AudioSource>();
        GetComponents(allAudios);
        foreach (AudioSource audio in allAudios)
        {
            if (audio.clip.name == "tankshoot") shoot = audio;
            if (audio.clip.name == "bounce") bounce = audio;
        }
        

    }


    
    public void updateTankObject(GameObject go)
    {
        _Tank = go;
        Tank = _Tank.transform;
        PV = _Tank.GetComponent<PlayerController>().PV;
        if (PV.IsMine)
        {
            //Hashtable hash = new Hashtable();
            //hash.Add("tankID", ID);
            //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            Debug.Log($"Tank GO photon id is: {go.GetPhotonView().ViewID}");
            photonView.RPC("updateTankObject", RpcTarget.OthersBuffered, go.GetPhotonView().ViewID);
        }

    }

    [PunRPC]
    public void updateTankObject(int ID)
    {
        Debug.Log($"Tank view id is {ID}");
        _Tank = PhotonView.Find(ID).gameObject;
        if (_Tank == null) Debug.Log($"Failed to create to ");
        Tank = _Tank.transform;
        PV = _Tank.GetComponent<PlayerController>().PV;
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(rb);
        }
        q = transform.localRotation;
        startpos = transform.localPosition;
        transform.localPosition = Tank.localPosition + new Vector3(0, -999, 0);



    }

    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (_Tank.GetComponent<PlayerController>().isSpintout) return;

        if (!bulet)
        {
            

            rend.enabled = false;
            rb.isKinematic = true;
            col.enabled = false;
            hits = 0;

            if (Input.GetKey(fireKey))
            {
                
                transform.rotation = Tank.rotation;
                transform.localRotation *= Quaternion.Euler(0, 90, 0);


                transform.localPosition = Tank.localPosition + Tank.right + bulletOffset;

                rb.constraints = RigidbodyConstraints.None;

                shoot.Play();
                ypaus = transform.position.y;

                rend.enabled = true;
                rb.isKinematic = false;
                
                col.enabled = true;
                hits = 0;
                bulet = true;

                rb.velocity = transform.forward * movespeed;
                col.enabled = true;
                StartCoroutine("shootle");
            }
        }
        else { 
            transform.position = new Vector3(transform.position.x, ypaus, transform.position.z);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        PVID = PV.ViewID;
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        if (collision.gameObject.tag == "Untagged")
        {
            Vector2 direction = collision.GetContact(0).normal;
            if((direction.y == 1)==false)
                {
                hits = hits + 1;
            bounce.pitch = (hits/2f);
            bounce.Play();
            }

            if (hits > 6)
            {                
                StopCoroutine("shootle");
                StartCoroutine("resetbulley");
            }
        }
        
        if (collision.gameObject.tag == "tank")
        {
            StopCoroutine("shootle");
            StartCoroutine("resetbulley");
            //you hit a tank
        }
    }

 
    IEnumerator shootle()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine("resetbulley");
    }

    IEnumerator resetbulley()
    {
        if (!PV.IsMine) StopCoroutine("resetbulley");
        
        transform.localPosition = new Vector3(-999, -999, -999);

        yield return new WaitUntil(() => (Input.GetKey(fireKey) == false));
                rb.constraints = RigidbodyConstraints.FreezePositionY;

        rb.isKinematic = true;

        ypaus = .26f;
        rend.enabled = false;
        col.enabled = false;


        yield return new WaitUntil(() => (Input.GetKey(fireKey) == false));

        transform.localRotation = q;
        rend.enabled = false;


        bulet = false;

        //transform.localPosition = startpos;
        yield return null;

    }


    
    [PunRPC]
    public void updateColor(int playerColorIndex)
    {
        HUD.Instance.Log($"Player color index is: {playerColorIndex}");
        Color playerColor = _Tank.GetComponent<PlayerController>().colorArray[playerColorIndex];
        Renderer bullet_rend = GetComponent<Renderer>();
        bullet_rend.material.color = playerColor;
        if (PV.IsMine)
        {
            photonView.RPC("updateColor", RpcTarget.OthersBuffered, playerColorIndex);
        }

    }

}
