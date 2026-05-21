using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }

    [Header("Battle Game States")]
    [SerializeField] private bool Bool_IsGameOver = false;

    [Header("Drop Item Resources Settings")]
    [SerializeField] private GameObject GameObject_BaseLootBoxPrefab; // 드랍 연출로 사용할 부유하는 기본 전리품 상자 프리팹

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
    public void CallPlayerVictory(string dropTableId, Vector3 enemyPosition)
    {
        if (Bool_IsGameOver == true) return;

        Bool_IsGameOver = true;
        Debug.Log(" [배틀 매니저] 적 함선 격침 완료! 승리 프로세스 및 보상 연산을 가동합니다.");

        // 1. JSON 마스터 테이블에서 해당하는 드랍테이블 행 데이터를 조회합니다.
        if (GameDataManager.Instance != null)
        {
            // 유저님이 구축하신 데이터 매니저 조회 규격에 맞춰 연동하십시오.
            DropTableData targetDropData = GameDataManager.Instance.GetDropItemData(dropTableId);

            if (targetDropData != null)
            {
                // 2. 확보된 데이터를 바탕으로 독립 주사위 분기 공정을 집행합니다.
                ExecuteDropLogic(targetDropData, enemyPosition);
            }
            else
            {
                Debug.LogWarning($"[배틀 경고] 입력된 드랍 ID [{dropTableId}]를 JSON 시트에서 찾을 수 없습니다.");
            }
        }
        // TODO: UImanager.Inst.ShowPopupUI("VictoryPopupUI") 연동 가능
    }


    // 독립 주사위 분기 공정
    private void ExecuteDropLogic(DropTableData targetData, Vector3 dropPos)
    {
        // 내장 유틸리티 함수들을 호출해 순정 2D 기하 배열로 복원 추출
        string[] items = targetData.GetItemArray();
        float[] chances = targetData.GetChanceArray();
        int[] minCounts = targetData.GetMinCountArray();
        int[] maxCounts = targetData.GetMaxCountArray();

        // [동시 주사위 가동]: 쪼개진 배열의 크기만큼 루프를 돌며 연산을 일괄 집행
        for (int i = 0; i < items.Length; i++)
        {
            float diceRoll = UnityEngine.Random.Range(0f, 100f);

            // 해당 칸 인덱스에 대응하는 개별 확률 제원과 대조
            if (diceRoll <= chances[i])
            {
                int finalCount = UnityEngine.Random.Range(minCounts[i], maxCounts[i] + 1);

                // 3. [최종 물리 방출]: 월드 좌표계에 아이템 오브젝트 실물 스폰 지시
                SpawnDroppedItemPrefab(items[i], finalCount, dropPos);
            }
        }
    }

    // 실질적으로 아이템 상자나 루팅 프리팹을 월드에 복사해 주는 연출 함수
    private void SpawnDroppedItemPrefab(string itemId, int count, Vector3 position)
    {
        if (GameObject_BaseLootBoxPrefab == null)
        {
            Debug.LogWarning($"[드랍 연출 경고] GameObject_BaseLootBoxPrefab이 비어있어 실물 복사를 생략합니다. (당첨 아이템: {itemId} / {count}개)");
            return;
        }

        // 1. 뱃머리가 터진 바다 좌표 평면(Z=0)에 기본 보물상자 프리팹을 복사 생성합니다.
        GameObject spawnedBoxObj = Instantiate(GameObject_BaseLootBoxPrefab, position, Quaternion.identity);

        // 2. 💡 [매우 중요]: 새로 태어난 보물상자 기물에 붙어있을 루팅용 컴포넌트(예: LootBox.cs)를 서칭합니다.
        // 나중에 인벤토리에 들어갈 실제 알맹이 정보인 '아이템 ID'와 '수량'을 상자 데이터 그릇에 원격 주입합니다.
        LootBox lootBoxScript = spawnedBoxObj.GetComponent<LootBox>();
        if (lootBoxScript != null)
        {
            // 상자 스크립트에 아이템 정보 심어두기
            lootBoxScript.SetupLootBoxContents(itemId, count);
        }

        Debug.Log($" [물리 방출 성공] 월드 씬 전리품 보상 상자 스폰 완료! 내장 품목 ID: {itemId} / 수량: {count}개");
    }



    public void ResetBattleState()
    {
        Bool_IsGameOver = false;
        Time.timeScale = 1f;
    }
}