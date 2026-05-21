using UnityEngine;

public class LootBox : MonoBehaviour
{
    [Header("Loot Box Component Cache")]
    [SerializeField] private SpriteRenderer SpriteRenderer_InsideIcon; //  아이템 아이콘 이미지를 그려줄 컴포넌트

    [Header("Loot Box Inside Contents")]
    [SerializeField] private string String_ContainedItemId;
    [SerializeField] private int Int_ContainedItemCount;

    private void Awake()
    {
        CacheLootBoxComponents();
    }

    private void CacheLootBoxComponents()
    {
        // 만약 인스펙터에서 깜빡하고 할당을 안 했다면, 자기 자신이나 자식 기물에서 자동으로 렌더러를 찾아옵니다.
        if (SpriteRenderer_InsideIcon == null)
        {
            SpriteRenderer_InsideIcon = GetComponent<SpriteRenderer>();

            if (SpriteRenderer_InsideIcon == null)
            {
                SpriteRenderer_InsideIcon = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    //  [핵심 데이터 주입구]: BattleManager가 상자를 Instantiate한 직후 이 함수를 호출하여 내용을 심습니다.
    public void SetupLootBoxContents(string itemId, int count)
    {
        String_ContainedItemId = itemId;
        Int_ContainedItemCount = count;

        //  [실시간 데이터 드리븐 외형 동기화]: 알맹이가 정해진 즉시 JSON 이미지 경로를 추적합니다.
        ApplyItemIconFromTable();
    }

    // JSON 마스터 데이터를 조회하여 아이콘 스프라이트를 실시간 교체하는 전담 함수
    private void ApplyItemIconFromTable()
    {
        if (GameDataManager.Instance == null || SpriteRenderer_InsideIcon == null)
        {
            return;
        }

        // 1. 유저님이 개설해 두신 공용 아이템 마스터 테이블에서 ID를 기점으로 로드합니다.
        // (아이템 데이터 클래스 명칭이 ItemData 라고 가정하고 규격을 세팅합니다)
        ItemData itemMaster = GameDataManager.Instance.GetItemData(String_ContainedItemId);

        if (itemMaster == null)
        {
            Debug.LogWarning($"[루팅박스 경고] 아이템 ID [{String_ContainedItemId}]를 마스터 테이블에서 찾을 수 없습니다.");
            return;
        }

        // 2.  [경로 추적 및 로드]: JSON에 기록된 "IconPath" 텍스트 정보(예: "Icons/Gold")를 파싱합니다.
        //  조건: 해당 이미지 에셋은 반드시 프로젝트 창의 'Assets/Resources/' 폴더 내부에 물리적으로 존재해야 합니다.
        string targetResourcePath = itemMaster.IconPath;

        if (string.IsNullOrEmpty(targetResourcePath) == false)
        {
            // Resources.Load API를 가동하여 텍스트 주소지에 매핑된 스프라이트 원본 리소스를 메모리에 낚아챕니다.
            Sprite loadedIconSprite = Resources.Load<Sprite>(targetResourcePath);

            if (loadedIconSprite != null)
            {
                // 3. 내 몸뚱이에 붙은 SpriteRenderer의 이미지를 실시간으로 교체 배정합니다.
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
        if (collision.CompareTag("Player") == true)
        {
            Debug.Log($"[루팅 청취] 플레이어 함선과 드랍 보물상자 충돌 확인.");

            if (Inventory.Instance != null)
            {
                // [최종 수송]: 상자가 간직했던 아이템 마스터 정보 ID와 수량을 
                // 인벤토리 백엔드 매니저의 필터 적재 연산 함수인 AddItemToInventory로 토스 배달합니다.
                Inventory.Instance.AddItemToInventory(String_ContainedItemId, Int_ContainedItemCount);
            }
            // 상자 기물 파쇄 철거
            Destroy(gameObject);
        }
    }
}