using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class FireBaseManager : Singleton<FireBaseManager>
{
    private string _googlePlayId;

    private DatabaseReference reference;

    protected override void Init()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        GooglePlaySignIn();
    }

    private void GooglePlaySignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(status =>
        {
            if (status == SignInStatus.Success)
            {
                _googlePlayId = PlayGamesPlatform.Instance.GetUserId();
                Debug.Log("Sign In Success");
                GetUserData();
            }
            else
            {
                Debug.Log("Sign in Failed: " + status);
            }
        });
    }

    private void GetUserData()
    {
        reference.Child("user").Child(_googlePlayId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (!task.Result.Exists)
                {
                    Debug.Log("New User");
                    UpdateUserData(new UserData());
                    DataManager.Instance.SaveData = new UserData();
                    return;
                }
                DataManager.Instance.SaveData = JsonUtility.FromJson<UserData>(task.Result.GetRawJsonValue());
                
            }
            else
            {
                Debug.Log("User Data Load failed");
            }
        });
    }

    public void UpdateUserData(UserData userData)
    {
        string json = JsonUtility.ToJson(userData);
        reference.Child("user").Child(_googlePlayId).SetRawJsonValueAsync(json);
    }

}
