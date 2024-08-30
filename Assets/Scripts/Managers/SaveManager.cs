using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string _savePath;
    
    private void SaveUserData(UserData userData)
    {
        _savePath = Application.persistentDataPath + "/SaveData.json";
        string jsonData = JsonUtility.ToJson(userData);
        File.WriteAllText(_savePath, jsonData);
    }

    public void LoadUserData()
    {
        _savePath = Application.persistentDataPath + "/SaveData.json";
        if (!File.Exists(_savePath))
        {
            GameManager.Instance.datamanager.SaveData = new UserData();
            return;
        }
        string jsonData = File.ReadAllText(_savePath);
        GameManager.Instance.datamanager.SaveData = JsonUtility.FromJson<UserData>(jsonData);
        Debug.Log("Complete");
    }

    public void GetSaveUserData(UserData userData)
    {
        SaveUserData(userData);
    }
}
