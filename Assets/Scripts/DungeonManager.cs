using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour {
    public GameObject Dialog;
    public GameObject removeDialog;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
    public void SetView()
    {
        Dialog.SetActive(true);
        if (removeDialog) removeDialog.SetActive(false);
    }
    public void EntranceDungeon()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
