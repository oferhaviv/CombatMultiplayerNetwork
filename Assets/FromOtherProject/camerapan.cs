using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerapan : MonoBehaviour
{


    public Quaternion startpo;

    public Quaternion startro;

    public float rbyv;


    Vector3 eulers;
    // Start is called before the first frame update
    void Start()
    {
        eulers = transform.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //I will fix this later.

        Rigidbody rb = gameObject.GetComponentInParent(typeof(Rigidbody)) as Rigidbody;
        rbyv = Mathf.Pow(rb.velocity.y,2);

        if (rb.velocity.y < 0.5)
        {
            transform.eulerAngles = new Vector3((transform.eulerAngles.x + 15), transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (rb.velocity.y > 0.5)
        {
            transform.eulerAngles = new Vector3((transform.eulerAngles.x - 1), transform.eulerAngles.y, transform.eulerAngles.z);
        }




        //  transform.localPosition = new Vector3(-1.92999995f - rbyv * 40, 1.49000001f - rbyv * 100, 0.0799999982f);
        // transform.eulerAngles = new Vector3((20 - rbyv ), transform.eulerAngles.y, 0);



    }
}
