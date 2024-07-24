using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class PlayGameManager : MonoBehaviour
{
    public TMP_Text nameText;

    void Start()
    {
        //SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void SignInManually()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            nameText.text = PlayGamesPlatform.Instance.GetUserDisplayName();
            GameController.instance.playerName = PlayGamesPlatform.Instance.GetUserDisplayName();
        }
        else
        {
            nameText.text = "Failed!";
            GameController.instance.playerName = "Player";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}
