using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {
    public Image DrawManager;
    public Text Name;
	// Use this for initialization
	void Start () {
        Name.text = PlayerPrefs.GetString("userName")+"쨩, ㅎㅇ";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowGuideLine()
    {
        if (DrawManager.color.a != 0)
        {
            DrawManager.color = new Color(255, 255, 255, 0);
        }
        else
        {
            DrawManager.color = new Color(255, 255, 255, 255);
        }
    }
}
