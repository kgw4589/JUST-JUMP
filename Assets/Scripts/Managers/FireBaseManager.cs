using System;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class FireBaseManager : Singleton<FireBaseManager>
{
    private string _authCode;
    private FirebaseAuth _auth;
    private FirebaseApp _app;
    private DatabaseReference _dbReference;
    private DatabaseReference _userReference;
    private FirebaseUser _user;
    
    protected override void Init()
    {
        GameManager.Instance.firebaseManager = this;
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            }
            else
            {
                Debug.Log("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

        _app = FirebaseApp.DefaultInstance;
        _auth = FirebaseAuth.DefaultInstance;
        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        
        FireBaseLogin();
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Login Success");
            PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
            {
                _authCode = code;
            });
        }
        else
        {
            Debug.LogError("Login failed: " + status);
        }
    }

    private void FireBaseLogin()
    {
        Debug.Log("FireBase Login try ...");
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
            _user = result.User;
            _userReference = _dbReference.Child("users").Child(_user.UserId);
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            LoadSaveData();
        });

    }
    public void LoadSaveData()
    {
        DataSnapshot snapshot = null;
        _userReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("Load Success");
            }
            else
            {
                Debug.Log("Load failed : " + task.Exception);
            }

            GameManager.Instance.dataManager.SaveData = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        });
    }

    private void SaveInDB(string value)
    {
        _userReference.SetRawJsonValueAsync(value).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Save Completed");
            }
            else
            {
                Debug.Log("Save failed: " + task);
            }
        });
    }

    public void ShowLeaderBoard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
    }

    public void GetSaveInDB(string value)
    {
        SaveInDB(value);
    }
}