using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour {
    public GameObject story1, story2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            if (story2.active)
                SceneManager.LoadScene("MainScene");
            story1.SetActive(false);
            story2.SetActive(true);
        }
	}
}
