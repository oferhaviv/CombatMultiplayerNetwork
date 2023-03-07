using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMap : MonoBehaviour
{
    public RectTransform minimapImage;

    [Header("Small Size")]
    public Vector2 smallSize = new Vector2(150f, 150f);
    public Vector3 smallSizePos = new Vector3(0, 0,0);
    public Color smallMapColor = Color.gray;

    [Header("Big Size")]
    public Vector2 bigSize = new Vector2(300f, 300f);
    public Vector3 bigSizePos = new Vector3(75, -75,0);
    public Color bigMapColor = Color.white;

    bool isSmallSize = true;
    Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.image.color = smallMapColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonClicked()
    {
        if (isSmallSize)
        {
            minimapImage.sizeDelta = bigSize;
            minimapImage.localPosition = bigSizePos;
            _button.image.color = bigMapColor;
        }
        else
        {
            minimapImage.sizeDelta = smallSize;
            minimapImage.localPosition = smallSizePos;
            _button.image.color = smallMapColor; 
        }
        isSmallSize = !isSmallSize;
    }

}
