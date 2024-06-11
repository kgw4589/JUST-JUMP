using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class FireBaseManager : Singleton<FireBaseManager>
{
    private string _authCode;
    private FirebaseAuth _auth;
    
    protected override void Init()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        ChectFirebaseDepencies();
    }

    private void ChectFirebaseDepencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependecies: " + task.Result);
            }
        });
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Login Success");
        }
        else
        {
            Debug.LogError("Login failed: " + status);
        }
    }

    private void FireBaseLogin()
    {
        Credential credential = PlayGamesAuthProvider.GetCredential(_authCode);
        _auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignIn was canceled");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignIn encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });

    }
}