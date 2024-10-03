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

    private string _token;
    private DatabaseReference reference;

    protected override void Init()
    {
        
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Sign In Success");
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                _token = code;
            });
        }
        else
        {
            Debug.Log("Sign In failed : " + status);
        }
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
