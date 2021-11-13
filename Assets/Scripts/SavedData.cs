using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavedData : MonoBehaviour
{
    public Color firstColor,secondColor;
    public string playerName;
    public int playerNumber;

    private int difficulty;

    public static SavedData Instance;
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        LoadData();
        // end of new code
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetData()
    {

    }
    public void SetData(Color folor, Color solor, string name, int number)
    {
        if (folor != Color.clear)
        {
            firstColor = folor;
        }
        if (solor != Color.clear)
        {
            secondColor = solor;
        }
        if(name != null)
        {
            playerName = name;
        }
        if(number != -1)
        {
            playerNumber = number;
        }
    }
    [System.Serializable]
    class SaveData
    {
        public Color firstColor, secondColor;
        public string playerName;
        public int playerNumber;
    }
    public void SaveDate()
    {
        SaveData data = new SaveData();
        data.firstColor = firstColor;
        data.secondColor = secondColor;
        data.playerName = playerName;
        data.playerNumber = playerNumber;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/data.json", json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/data.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            firstColor = data.firstColor;
            secondColor = data.secondColor;
            playerName = data.playerName;
            playerNumber = data.playerNumber;
        }
    }
}
