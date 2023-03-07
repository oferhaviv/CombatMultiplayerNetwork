using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class bullietuii : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public pause main;

     public bool sele;



    string ob;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sele == true)
        {

            ob = gameObject.name;

            if (ob == "bullet hit")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (main.bullethits == false)
                    {
                        main.bullethits = true;


                    }
                    else if (main.bullethits == true)
                    {
                        main.bullethits = false;
                    }

                }
            }

            else if (ob == "resume")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    main.canpause = 2;

                }
            }
            else if (ob == "level det")
            {

                if (Input.GetMouseButtonDown(0))
                {
                    main.level = main.level + 1;
                    if (main.level > main.maxlevel)
                    {
                        main.level = 1;
                    }



                }
            }
            else if (ob == "bounds")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (main.bounds == false)
                    {
                        main.bounds = true;


                    }
                    else if (main.bounds == true)
                    {
                        main.bounds = false;
                    }
                }
            }
            else if (ob == "mapaaaaaa")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (main.mapaaaaa == false)
                    {
                        main.mapaaaaa = true;


                    }
                    else if (main.mapaaaaa == true)
                    {
                        main.mapaaaaa = false;
                    }
                }
            }
        }
        
        
        





    }
    public void OnPointerExit(PointerEventData eventData)
    {
        sele = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sele = true;

        Debug.Log(gameObject.name);
        
       
        
        
    }
}
