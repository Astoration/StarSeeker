using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour {
    public Text Name, Level;
	// Use this for initialization
    public class UserData
    {
        string userId;
        string userName;
        string masterCharacter;
        string registerDate;
        string lastloginDate;
        int userLevel;
        string[] friendRequest;
        string[] clanRequest;
        string userClan;
        string[] userFriend;
        int[] unlockedStage;
        int golds;
        int exp;
        int magicStone;
    }
	void Start () {
        //string id = PlayerPrefs.GetString("userId");
        //REST.instance.GET("http://koibito.moe/StarSeeker/users/" + id, delegate (WWW w) {
        //    UserData user = JsonUtility.FromJson(w.text, UserData);
        //});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
