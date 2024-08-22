using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public class DataManager : Singleton<DataManager>
{
    private string SavePath()
    {
        return Path.Combine(Application.persistentDataPath, "SaveData.json");
    }
    
    private void EditJson(UserData jsonData)
    {
        string saveData = JsonUtility.ToJson(jsonData, true);
        File.WriteAllText(SavePath(), saveData, Encoding.UTF8);
    }
    
    public T LoadJson<T>()
    {
        string jsonData = File.ReadAllText(SavePath(), Encoding.UTF8);
        return JsonUtility.FromJson<T>(jsonData);
    }


    public void GetEditJson(UserData jsonData)
    {
        EditJson(jsonData);
    }
}
