using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _inst;
    public static GameManager Inst
    {
        get { return _inst; }
        set { _inst = value; }
    }

    private PlayerModel _playerModel = new PlayerModel();

    // 외부에서 데이터 원본에 접근하기 위한 프로퍼티
    public PlayerModel PlayerModel
    {
        get { return _playerModel; }
        set { _playerModel = value; }
    }

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    private void Start()
    {
        LoadSaveData();
    }

    public void SaveData()
    {
        NetworkManager.Inst.RequestSaveData(_playerModel);
    }

    public void SaveAndEndGame()
    {
        SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void LoadSaveData()
    {
        _playerModel = NetworkManager.Inst.RequestLoadSaveData();
    }
}