using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEditor.VersionControl;

public class FireBaseManager : Singleton<FireBaseManager>
{
    private string _authCode;
    private FirebaseAuth _auth;
    private FirebaseApp app;
    
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
                app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                GoogleLogin();
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependecies: " + task.Result);
            }
        })
    }

    private void GoogleLogin()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(SignInInteractivity.CanPromptOnce, result =>
        {
            if (result == SignInStatus.Success)
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
                Debug.LogError("LogIn failed: " + result);
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