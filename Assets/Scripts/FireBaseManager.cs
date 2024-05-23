using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
    
public class FireBaseManager : Singleton<FireBaseManager>
{
    private FirebaseAuth _auth;
    private string _authCode;
    
    protected override void Init()
    {
        
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        _auth = FirebaseAuth.DefaultInstance;
        Debug.Log("FireBase Starts");
        
        GoogleLogin();
    }

    private void GoogleLogin()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
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
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(_authCode, null);
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