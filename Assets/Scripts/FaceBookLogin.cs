using UnityEngine;
using Facebook.Unity;
using System.Collections;
using System;
using Facebook.MiniJSON;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaceBookLogin : MonoBehaviour{
    public GameObject Dialog;
    public GameObject NameDialog;
    public GameObject MessageDialog;
    public Text field;
    public Text ConsoleLog;
    private AccessToken token;

    public delegate void Callback(WWW www);

    public void OnLoginButtonClicked()
    {
        Login();
    }

    public void MessageConfirm()
    {
        MessageDialog.SetActive(false);
    }

    void Start()
    {
        NameDialog.SetActive(false);
        MessageDialog.SetActive(false);
        if (PlayerPrefs.GetString("userId","undefined") != "undefined"&&PlayerPrefs.GetString("token","undefined")!="undefined")
        {
            string userId = PlayerPrefs.GetString("userId");
            string tokenString = PlayerPrefs.GetString("token");
            REST.instance.POST("http://koibito.moe/StarSeeker/login",new Dictionary<string, string>()
            {
                {"userId",userId},
                { "token",tokenString}
            },delegate(WWW www)
            {
                int code = int.Parse(www.text);
                ConsoleLog.text = code + "";
                if (code == 200) SceneManager.LoadScene("MainScene");
            });
        }
    }

    void Update()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Dialog.SetActive(true);
        }
        else
        {
            Dialog.SetActive(false);
        }
    }

    void Login()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallBack);
        }
    }

    public void OnRegister()
    {
        REST.instance.GET("http://koibito.moe/StarSeeker/users/" + field.text.ToString(), delegate (WWW www)
         {
             if (www.text == "undefined")
             {
                REST.instance.POST("http://koibito.moe/StarSeeker/users/", new Dictionary<string, string>{
                {"token", token.TokenString},
                { "userId", token.UserId },
                { "name", field.text}
                }, delegate (WWW result){
                    int code = int.Parse(result.text);
                    if (code == 200)
                    {
                        SceneManager.LoadScene("storyScene");
                    }
                    else
                    {
                        MessageDialog.SetActive(true);
                    }
                });
             }
         });
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (FB.IsLoggedIn)
            {
                token = Facebook.Unity.AccessToken.CurrentAccessToken;
                PlayerPrefs.SetString("userId", token.UserId);
                PlayerPrefs.SetString("token", token.TokenString);
                //FB.API("me?fields=name", Facebook.Unity.HttpMethod.REST.instance.GET,LoginCallback);
                REST.instance.GET("http://koibito.moe/StarSeeker/users/" + token.UserId,delegate(WWW www){
                    if (www.text == "undefined")
                            {
                        NameDialog.SetActive(true);
                    }
                    else
                    {
                        string userId = PlayerPrefs.GetString("userId");
                        string tokenString = PlayerPrefs.GetString("token");
                        REST.instance.POST("http://koibito.moe/StarSeeker/login", new Dictionary<string, string>()
                        {
                            {"userId",userId},
                            { "token",tokenString}
                        }, delegate (WWW Data)
                        {
                            int code = int.Parse(Data.text);
                            if (code == 200) SceneManager.LoadScene("InGameScene");
                        });
                    }
                });
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }
    }
    void LoginCallback(IGraphResult result)
    {
        ConsoleLog.text = result.ResultDictionary["name"].ToString();
        PlayerPrefs.SetString("userName", result.ResultDictionary["name"].ToString());
    }
}
