using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string _savePath;    
    
    private void SaveUserData(UserData userData)
    {
        string jsonData = JsonUtility.ToJson(userData);
        _savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        File.WriteAllText(_savePath, jsonData);
    }

    public void LoadUserData()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        if (!File.Exists(_savePath))
        {
            DataManager.Instance.SaveData = new UserData();
            return;
        }
        string jsonData = File.ReadAllText(_savePath);
        DataManager.Instance.SaveData = JsonUtility.FromJson<UserData>(jsonData);
    }

    public void GetSaveUserData(UserData userData)
    {
        SaveUserData(userData);
    }
}
