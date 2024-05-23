using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class DataManager : Singleton<DataManager>
{
    
    protected override void Init()
    {
        if (!File.Exists(Application.persistentDataPath+"/SaveData.json"))
        {
            CreateJson(new JsonData(0));
        }
    }

    private void CreateJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", 
            Application.persistentDataPath, "SaveData"), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    private void ChangeJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        FileStream fileStream = new FileStream(
            Application.persistentDataPath+"/SaveData.json", FileMode.Open, FileAccess.Write);
        byte[] data = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    
    public T LoadJson<T>()
    {
        FileStream fileStream = new FileStream(
            Application.persistentDataPath+"/SaveData.json", FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }


    public void GetCreateJson(JsonData jsonData)
    {
        CreateJson(jsonData);
    }

    public void GetChangeJson(JsonData jsonData)
    {
        ChangeJson(jsonData);
    }

}
