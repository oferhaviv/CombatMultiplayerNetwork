using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Image[] images;
    int colorDistance = 100;

    public void updateHealth(int state)
    {
        state--; //zero base
        if (state < 0)
        {
            Debug.LogError($"state went less than 0, {state}");
            return;
        }
        
        Color currentColor = images[state].material.color;
        
        currentColor.a -= colorDistance;
        
        images[state].material.color = currentColor;
    }

    public void initColors(Color c)
    {
        foreach (Image i in images)
        {
            i.material.color = c;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
