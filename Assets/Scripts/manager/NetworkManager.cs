using UnityEngine;
using System.IO;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Inst { get; set; }

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    public void RequestSaveData(PlayerModel data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
        Debug.Log($"저장 완료: {GetPath()}");
    }

    public PlayerModel RequestLoadSaveData()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerModel data = JsonUtility.FromJson<PlayerModel>(json);
            Debug.Log("데이터를 성공적으로 불러왔습니다.");
            return data;
        }
        else
        {
            Debug.LogWarning("세이브 파일이 없습니다. 기본 데이터를 생성합니다.");
            var defaultData = GetDefaultPlayerData();
            RequestSaveData(defaultData);
            return defaultData;
        }
    }

    private PlayerModel GetDefaultPlayerData()
    {
        var newPlayerData = new PlayerModel();
        newPlayerData.PlayerName = "NoName";
        newPlayerData.PlayerTotalExp = 0;
        return newPlayerData;
    }
}