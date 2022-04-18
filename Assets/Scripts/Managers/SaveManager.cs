using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータを管理するクラス
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; } = null;

    string filePath = null;
    public SaveData SaveData { get; private set; } = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Application.dataPath + FilePath.SaveData;

        Load();
        if (SaveData == null)
        {
            SaveData = new SaveData();
        }
    }

    /// <summary>
    /// セーブデータをセーブする
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(SaveData);
        StreamWriter streamWriter = new StreamWriter(filePath, false);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    /// <summary>
    /// セーブデータをロードする
    /// </summary>
    public void Load()
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        StreamReader streamReader = new StreamReader(filePath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();

        SaveData = JsonUtility.FromJson<SaveData>(data);
    }

    /// <summary>
    /// ハイスコアを記録する
    /// </summary>
    /// <param name="score">記録するスコア</param>
    public void Register(int score)
    {
        SaveData.Register(score);
    }
}
