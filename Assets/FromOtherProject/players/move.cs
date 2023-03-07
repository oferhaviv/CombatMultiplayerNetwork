using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    public Rigidbody rb;
    public float movespeed;
    public float movedirecspeed;
    public float maxspeed;
    public bool spinout;

    public float timer;

    public List<string> controlss;


    public float scoree;
    public Text score;
    public AudioSource ded;

    public AudioSource mmove;



    float spin;


    // Update is called once per frame
    void FixedUpdate()
    {

        mmove.pitch = rb.velocity.magnitude + 1;
        if (Input.GetKey(controlss[0]))
        {
            if( rb.velocity.magnitude < maxspeed)
            {
            rb.AddRelativeForce(((movespeed) * Time.deltaTime * -1), 0,0);

                rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);
            }
        }
        if (Input.GetKey(controlss[1]))
        {
            switch (spinout)
            {
                case false:
                    rb.angularVelocity = new Vector3(0, -movedirecspeed * Time.deltaTime, 0);

                    break;
            }
        }

        if (Input.GetKey(controlss[2]))
        {
            if (rb.velocity.magnitude < maxspeed)
            {
                rb.AddRelativeForce(((movespeed) * Time.deltaTime), 0, 0);

            }
        }
        if (Input.GetKey(controlss[3]))
        {
            switch (spinout)
            {
                case false:
                    rb.angularVelocity = new Vector3(0, movedirecspeed * Time.deltaTime, 0);

                    break;
            }
        }
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            rb.angularVelocity = new Vector3(0, 90000000, 0);
            Debug.Log("spin");
            timer = 10;
            StartCoroutine("speen");
            ded.Play();
            scoree = scoree + 1;
            score.text = scoree.ToString();
        }
        if (collision.gameObject.name == "teleback")
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.material.name);

        if (other.tag == "bouncepad")
        {
            Debug.Log("bounce");

            rb.velocity = new Vector3(0, 0,0);
            rb.AddRelativeForce((movespeed*-5) * Time.deltaTime, 20000 * Time.deltaTime, 0);
        //fix this number later
        }
    }

    IEnumerator speen()
    {

        while (timer > 0)
        {
            spinout = true;
	
            rb.angularVelocity = new Vector3(0, 90000 * Time.deltaTime, 0);
            
            timer = timer - Random.Range(4, 5) * Time.deltaTime;
	
        yield return null;//Reminder to replace this with  WaitForFixedUpdate and fix the numbers

        }
        spinout = false;

        yield return null;


    }
}
