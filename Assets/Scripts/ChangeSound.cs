using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSound : MonoBehaviour
{
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    public Button button;
    private bool isOn = true;


    // Start is called before the first frame update
    void Start()
    {
        soundOnImage = button.image.sprite;
    }

    public void ButtonClicked()
    {
        if (isOn)
        {
            button.image.sprite = soundOffImage;

        }
        else
        {
            button.image.sprite = soundOnImage;
        }
        isOn = !isOn;
        AudioListener.volume = isOn ? 1 : 0;
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
