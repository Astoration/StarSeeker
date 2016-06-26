using UnityEngine;
using Facebook.Unity;
using System.Collections;
using System;
using System.Collections.Generic;

public class FaceBookLogin : MonoBehaviour{
    public void OnLoginButtonClicked()
    {
        Login();
    }

    void Login()
    {
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallBack);
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            Debug.Log(aToken.UserId);
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
