using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }

    [Header("Battle Game States")]
    [SerializeField] private bool Bool_IsGameOver = false;

    private void Awake()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {
        Instance = this;
        if (transform.parent != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // 실시간 게임오버 여부를 판단하는 외교용 함수
    public bool GetIsGameOver()
    {
        return Bool_IsGameOver;
    }

    // 플레이어 함선이 침몰했을 때 호출되는 함수
    public void CallPlayerDefeat()
    {
        if (Bool_IsGameOver == true) return;

        Bool_IsGameOver = true;
        Debug.Log(" [배틀 매니저] 플레이어 함선 파괴! 게임오버(패배) 프로세스를 가동합니다.");

        // TODO: UImanager.Inst.ShowPopupUI("DefeatPopupUI") 연동 가능
    }

    // 적 함선이 침몰했을 때 호출되는 함수
    public void CallPlayerVictory()
    {
        if (Bool_IsGameOver == true) return;

        Bool_IsGameOver = true;
        Debug.Log(" [배틀 매니저] 적 함선 격침 완료! 승리 프로세스를 가동합니다.");

        // TODO: UImanager.Inst.ShowPopupUI("VictoryPopupUI") 연동 가능
    }

    public void ResetBattleState()
    {
        Bool_IsGameOver = false;
        Time.timeScale = 1f;
    }
}