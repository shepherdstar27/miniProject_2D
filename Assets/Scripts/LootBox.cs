using UnityEngine;

public class LootBox : MonoBehaviour
{
    [Header("Loot Box Component Cache")]
    [SerializeField] private SpriteRenderer SpriteRenderer_InsideIcon;

    [Header("Loot Box Inside Contents")]
    [SerializeField] private string _containedItemId;
    [SerializeField] private int _containedItemCount;

    private void Awake()
    {
        CacheLootBoxComponents();
    }

    private void CacheLootBoxComponents()
    {
        if (SpriteRenderer_InsideIcon == null)
        {
            SpriteRenderer_InsideIcon = GetComponent<SpriteRenderer>();

            if (SpriteRenderer_InsideIcon == null)
            {
                SpriteRenderer_InsideIcon = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    // [핵심 데이터 주입구]
    public void SetupLootBoxContents(string itemId, int count)
    {
        _containedItemId = itemId;
        _containedItemCount = count;

        ApplyItemIconFromTable();
    }

    // JSON 마스터 데이터를 조회하여 아이콘 스프라이트를 실시간 교체하는 전담 함수
    private void ApplyItemIconFromTable()
    {
        if (GameDataManager.Instance == null || SpriteRenderer_InsideIcon == null)
        {
            return;
        }

        ItemData itemMaster = GameDataManager.Instance.GetItemData(_containedItemId);

        if (itemMaster == null)
        {
            Debug.LogWarning($"[루팅박스 경고] 아이템 ID [{_containedItemId}]를 마스터 테이블에서 찾을 수 없습니다.");
            return;
        }

        string targetResourcePath = itemMaster.IconPath;

        if (string.IsNullOrEmpty(targetResourcePath) == false)
        {
            Sprite loadedIconSprite = Resources.Load<Sprite>(targetResourcePath);

            if (loadedIconSprite != null)
            {
                SpriteRenderer_InsideIcon.sprite = loadedIconSprite;
                Debug.Log($" [외형 동기화 성공] 전리품 상자 그래픽 갱신 완료. 경로: {targetResourcePath}");
            }
            else
            {
                Debug.LogWarning($"[리소스 억까] Resources 폴더 내부 [{targetResourcePath}] 위치에 실제 스프라이트 파일이 누락되었습니다.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[루팅 테스트] 성공! 플레이어를 감지했습니다.");

            // 데이터 중앙 관리 원칙: GameManager의 PlayerModel 원본에 직접 접근하여 데이터 적재
            if (GameManager.Inst != null && GameManager.Inst.PlayerModel != null)
            {
                GameManager.Inst.PlayerModel.AddItem(_containedItemId, _containedItemCount);
                Debug.Log($"[강제루팅] {_containedItemId} 아이템 {_containedItemCount}개를 PlayerModel에 안전하게 저장했습니다.");

                // 데이터 적재 완료 후, UI 동기화는 브리지(Inventory)가 활성화되어 있을 때만 호출
                if (Inventory.Instance != null)
                {
                    Inventory.Instance.RefreshInventoryUI();
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("[루팅 실패] GameManager 또는 PlayerModel 인스턴스가 존재하지 않아 아이템을 저장할 수 없습니다.");
            }
        }
    }
}