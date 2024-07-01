using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{
    public TMP_Text nameText;

    private void Awake()
    {
        FB.Init(SetInit, onHidenUnity);

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    print("Couldn't initialize");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is Login!");
            string s = "client token" + FB.ClientToken + "User Id" + AccessToken.CurrentAccessToken.UserId + "token string" + AccessToken.CurrentAccessToken.TokenString;
        }
        else
        {
            Debug.Log("Facebook is not Logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    void onHidenUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            //FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            print("Not logged in");
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
            if (nameText != null) nameText.text = name;
            nameText.text = name;
            Debug.Log("" + name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void Facebook_LogIn()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        //permissions.Add("user_friends");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            //AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;

            print(aToken.UserId);

            foreach (string perm in aToken.Permissions)
            {
                print(perm);
            }
        }
        else
        {
            print("Failed to log in");
        }

    }

    public void Facebook_LogOut()
    {
        StartCoroutine(LogOut());
    }
    IEnumerator LogOut()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            print("Logging Out");
            yield return null;
        }
        print("Logout Successful");
        // if (FB_profilePic != null) FB_profilePic.sprite = null;
        if (nameText != null) nameText.text = "";
        //if (rawImg != null) rawImg.texture = null;
    }

}
