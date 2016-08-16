using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class MenuManager : MonoBehaviour {
    public Text Name, Level, Gold, Magicstone;
	
	void Start () {
        string id = PlayerPrefs.GetString("userId","1805781019651285");
        REST.instance.GET("http://koibito.moe/StarSeeker/users/" + id, delegate (WWW w)
        {
            JsonData user = JsonMapper.ToObject(w.text);
            Name.text = user["userName"].ToString();
            Level.text = user["userLevel"].ToString();
            Gold.text = user["golds"].ToString();
            Magicstone.text = user["magicStones"].ToString();
        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
