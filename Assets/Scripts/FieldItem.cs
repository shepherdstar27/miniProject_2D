using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private int Int_ItemId = 1001; // 주울 아이템의 JSON 마스터 ID
    [SerializeField] private int Int_ItemCount = 1; // 주울 개수

    // 배가 아이템 영역에 닿았을 때 물리 엔진이 자동으로 호출해 주는 센서 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 물체가 "Player" 태그를 가진 플레이어의 배인지 확인합니다.
        if (collision.CompareTag("Player") == true)
        {
            // 플레이어 오브젝트에 붙어있는 가방(Inventory) 컴포넌트를 가져옵니다.
            Inventory playerInventory = collision.GetComponent<Inventory>();

            if (playerInventory != null)
            {
                TryAcquireItem(playerInventory);
            }
        }
    }

    // 실제로 아이템 획득을 시도하고 사후 처리를 하는 정석 메서드
    private void TryAcquireItem(Inventory playerInventory)
    {
        // 1. 플레이어의 가방에 아이템을 넣어봅니다. (자리가 없으면 false 반환됨)
        bool isSuccess = playerInventory.AddItemToInventory(Int_ItemId, Int_ItemCount);

        if (isSuccess == true)
        {
            Debug.Log($"[필드 아이템] ID {Int_ItemId} ({Int_ItemCount}개) 습득 완료!");

            // 2. 만약 화면에 인벤토리 창이 열려있는 상태로 먹었다면, 
            // 열려있는 UI를 찾아서 실시간으로 새로고침 해줍니다.
            InventoryUI openedUI = UnityEngine.Object.FindFirstObjectByType<InventoryUI>();
            if (openedUI != null)
            {
                openedUI.UpdateInventoryUI();
            }

            // 3. 습득에 성공했으므로 필드에 떨어져 있던 이 오브젝트는 파괴하여 없앱니다.
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[필드 아이템] 가방이 꽉 차서 아이템을 먹을 수 없습니다.");
        }
    }
}