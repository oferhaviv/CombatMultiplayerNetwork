using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levela : MonoBehaviour
{

    public pause lev;


    public GameObject level2;
    public GameObject level3;
    public GameObject level4;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -0.67f, 0);

        StartCoroutine("level");
    }

    private void Update()
    {
        if (lev.canpause == 1)
        {
        StartCoroutine("level");

        }

    }

    // Update is called once per frame

    public void level()
    {
        level2.SetActive(false);
        level3.SetActive(false);
        level4.SetActive(false);


        switch (lev.level)
        {
            case 2:
                        level2.SetActive(true);

                break;
            case 3:
                level3.SetActive(true);

                break;
            case 4:
                level4.SetActive(true);

                break;
        }
        
        
        



    }
}
