using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // 규칙: 프로퍼티 자동 구현 사용
    public static GameManager Inst { get; set; }

    // 규칙: 멤버변수는 _aaa 소문자 시작
    // 규칙: 실시간 변하는 인스턴스 데이터는 뒤에 Model을 붙여 GameManager가 소유
    private PlayerModel _playerModel = new PlayerModel();

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSaveData();
    }

    // 규칙: 함수는 동사로 시작할 것, 클래스 밖에서 부르는 경우는 public
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

    // 규칙: private 필드인 _playerModel 데이터에 접근하기 위한 public Get 함수 제공
    public List<ItemModel> GetPlayerItemList()
    {
        return _playerModel.ItemList;
    }
}