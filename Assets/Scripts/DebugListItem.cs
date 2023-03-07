using TMPro;
using UnityEngine;

public class DebugListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public void SetUp(string message)
    {
        string _time = System.DateTime.Now.ToString("hh:mm:ss");
        text.text = $"{_time} {message}";
    }


}
