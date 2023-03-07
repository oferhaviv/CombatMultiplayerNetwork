using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Linq;
using TMPro;


public class pause : MonoBehaviour
{
    public AudioSource puas;
    public Transform pausae;
    public float canpause;
    public float level;


    public Text bullethit;
    public Text leveltxt;
    public Text boundtxt;

    public bool bullethits;

    public float maxlevel;
    public bool bounds;

    public Transform boundtr;

    public Camera map;
    public bool mapaaaaa;
    public Text maptxt;


    public TextMeshPro op;



    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 9999999999999, 0);
        canpause = 0;

        List<string> oplines = File.ReadAllLines(Application.persistentDataPath + "/op.txt").ToList();
        if(oplines[1] == "False")
        {
            bullethits = false;
        }
        else
        {
            bullethits = true;
        }
        level = float.Parse(oplines[2]);
        if (oplines[3] == "False")
        {
            bounds = false;
        }
        else
        {
            bounds = true;
        }
        if (oplines[4] == "False")
        {
            mapaaaaa = false;
        }
        else
        {
            mapaaaaa = true;
        }

    }

    // Update is called once per frame



    void Update()
    {
        if(canpause == 1)
        {

 op.text = "Hello! This is just the options for the tank game saved because it is annoying changing them every time. \n" + bullethits.ToString() + "\n".ToString()  + level.ToString() + "\n".ToString() + bounds.ToString() + "\n" +  mapaaaaa.ToString() ;
        Directory.CreateDirectory(Application.streamingAssetsPath + "/tankoptions/");
        CreateTexFile();
            CreateTexFile();
        }
       

        leveltxt.text = ("level[" + level.ToString() + "]");


        

            switch (bullethits)
            {
                case false:
                    bullethit.text = "Bullet destroyed after hitting a tank [ ]";
                    break;
                case true:
                    bullethit.text = "Bullet destroyed after hitting a tank [O]";
                    break;

            }
        switch (bounds)
        {
            case false:
                boundtxt.text = "1.5x Boundary Size [ ]";
                boundtr.localScale = new Vector3(14, 1, 14);
                map.orthographicSize = 7.5f;
                break;
            case true:
                boundtxt.text = "1.5x Boundary Size [O]";
                boundtr.localScale = new Vector3(14*1.5f, 1, 14 * 1.5f);
                map.orthographicSize = 10f;

                break;


        }
        switch (mapaaaaa)
        {
            case false:
                maptxt.text = "Map [ ]";
                map.enabled = false;
                break;
            case true:
                maptxt.text = "Map [O]";
                map.enabled = true;



                break;


        }




        if (Input.GetKey("p"))
            {
                if (canpause == 0)
                {
                    Time.timeScale = 0;
                    transform.localPosition = new Vector3(0, 0, 0);
                    puas.Play();
                    canpause = 1;
                }

            }


            if (canpause == 2)
        {
            
                Time.timeScale = 1;
                transform.localPosition = new Vector3(999999, 9999999999, 0);
                puas.Play();
                canpause = 0;
           
        }
        }
    public void CreateTexFile()
    {





        
            File.WriteAllText(Application.persistentDataPath + "/op.txt", op.text);
    }
    }



