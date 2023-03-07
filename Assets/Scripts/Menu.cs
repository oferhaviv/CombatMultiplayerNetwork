using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
        
    }
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }

    public int getLevel()
    {
        if (menuName == "createRoom" )
        {
            ToggleGroup tg = gameObject.GetComponentInChildren<ToggleGroup>();
            Toggle selectedToggle = tg.ActiveToggles().FirstOrDefault();
            Debug.LogError($"Toggle is {selectedToggle.name}");
            switch (selectedToggle.name)
            {
                case "A":
                    return 1;
                case "B":
                    return 2;
                case "C":
                    return 3;
                default:
                    return 1;
            }
        }
        else { return 1; }
    }

}
