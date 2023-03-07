using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimation : MonoBehaviourPunCallbacks
{
    public float frame;
    public float speed;

    public Renderer frameone;
    public Renderer frametwo;
    public Rigidbody rb;
    public PhotonView PV;

    
    bool last_update = true;

    private void Start()
    {
        updateFrames(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        // Tank is moving
        if (rb.velocity.magnitude != 0)
        {
            bool default_frame = true;

            //calc value base the frame choosing 
            frame = frame + speed * rb.velocity.magnitude * Time.deltaTime;

            //make frame selection
            if (frame < 1) default_frame = true;
            else
            {
                if (frame > 2) frame = 0;
                else default_frame = false;
            }

            //apply frame is needed
            if (last_update != default_frame)
            {
                last_update = default_frame;
                PV.RPC("updateFrames", RpcTarget.All, default_frame);
            }
            

        }
    }

    [PunRPC]
    public void updateFrames(bool default_frame)
    {
        frameone.enabled = default_frame;
        frametwo.enabled = !default_frame;
        
    }

}
