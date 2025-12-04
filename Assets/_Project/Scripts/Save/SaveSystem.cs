using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string fileName = "saveData.json";

    private static string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public static void SaveData(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(GetFilePath(), json);

        Debug.Log($"Game Saved to: {GetFilePath()}");
    }

    public static SaveData LoadData()
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            return data;
        }
        else
            return new SaveData();
    }

}

[Serializable]
public class SaveData
{
    public int highScore;

    public SaveData()
    {
        highScore = 0;
    }

    public SaveData(int highScore)
    {
        this.highScore = highScore;
    }
}