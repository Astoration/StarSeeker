using UnityEngine;
using Facebook.Unity;
using System.Collections;
using System;
using Facebook.MiniJSON;
using System.Collections.Generic;
using UnityEngine.UI;

public class FaceBookLogin : MonoBehaviour{
    public GameObject Dialog;
    public Text ConsoleLog;
    public void OnLoginButtonClicked()
    {
        Login();
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

    private void AuthCallBack(ILoginResult result)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (FB.IsLoggedIn)
            {
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                Debug.Log(aToken.UserId);
                FB.API("me?fields=name", Facebook.Unity.HttpMethod.GET,LoginCallback);
                PlayerPrefs.SetString("userId", aToken.UserId);
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
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
        Application.LoadLevel("InGameScene");
    }
}
