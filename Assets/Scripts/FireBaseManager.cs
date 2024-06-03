using System.Collections;
using System.Collections.Generic;
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

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;
        });
        
        GoogleLogin();
    }

    private void GoogleLogin()
    {
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("LogIn success");
                PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
                {
                    _authCode = code;
                    FireBaseLogin();
                });
            }
            else
            {
                Debug.LogError("LogIn failed");
            }
        });
    }

    private void FireBaseLogin()
    {
        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(_authCode);
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

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });

    }
}