﻿using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int StageNumber = 0;
}

public class JsonStageSelect : SingletonMonoBehaviour<JsonStageSelect>
{
    [SerializeField]
    [Header("セーブデータ")]
    private SaveData _saveData;

    [SerializeField]
    [Header("ファイルの場所")]
    private string _filePath;

    [SerializeField]
    [Header("ステージのボタン")]
    private GameObject[] _stageButton;

     protected override void Awake()
    {   
        base.Awake();
        _filePath = Application.persistentDataPath + "/" + ".savedata.json";
        if (!File.Exists(_filePath))
        {
            Debug.Log("ファイルが存在する");
        }
        else
        {
            new SaveData();
            Debug.Log("ファイルが存在しないため作成");
        }

        foreach (var chr in _stageButton)
        {
            chr.SetActive(false);
        }
        
        Load();
    }

    ///<summary>
    ///ステージクリア時に呼ばれる
    ///
    ///<summary>
    public void Save(int nowStageNumber)
    {
        if (!File.Exists(_filePath))
        {
            _filePath = Application.persistentDataPath + "/" + ".savedata.json";
        }
        else
        {
            new SaveData();
        }
        _saveData.StageNumber = nowStageNumber;

        var json = JsonUtility.ToJson(_saveData);
        StreamWriter streamWriter = new StreamWriter(_filePath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }

    /// <summary>
    ///ロード
    ///シーンが読み込まれたときに呼ぶ
 　/// </summary>
    public void Load()
    {
        if (File.Exists(_filePath))
        {                
            var streamReader = new StreamReader(_filePath);
            string _data = streamReader.ReadToEnd();
            streamReader.Close();

            _saveData = JsonUtility.FromJson<SaveData>(_data);

            for (int i = 0; i <= _saveData.StageNumber; i++)
            {
                _stageButton[i].SetActive(true);
                Debug.Log(i + 1 + "ステージを開放");
            }
        }
    }

    public void ResetSaveData()
    {
        _saveData.StageNumber = 0;
        Debug.Log("リセット");
    }
}
