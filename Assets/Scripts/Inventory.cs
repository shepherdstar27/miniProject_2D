using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 가방의 최대 크기 칸 수 고정
    [SerializeField] private int Int_MaxSlotCount = 20;

    // 실제 실시간으로 아이템 ID와 수량을 보관하는 인벤토리 내부 그릇 리스트
    [SerializeField] private List<InventorySlot> List_Slots = new List<InventorySlot>();

    private void Awake()
    {
        InitEmptySlots();
    }

    // 빈 가방 틀을 미리 20칸 확보하여 인덱스 에러와 UGUI 정렬 뒤틀림을 원천 방지합니다.
    private void InitEmptySlots()
    {
        List_Slots.Clear();
        for (int i = 0; i < Int_MaxSlotCount; i++)
        {
            List_Slots.Add(new InventorySlot(0, 0)); // ID가 0이면 빈 슬롯으로 규정합니다.
        }
    }

    public List<InventorySlot> GetSlots()
    {
        return List_Slots;
    }

    // ⭕ [아이템 획득 기능]: 외부(보물상자 상호작용 등)에서 호출할 정석 함수
    public bool AddItemToInventory(int itemId, int count)
    {
        ItemData masterData = GameDataManager.Instance.GetItemData(itemId);
        if (masterData == null)
        {
            return false;
        }

        // 1. 이미 똑같은 아이템이 가방에 있는지 확인하고, 최대 보유량(MaxStack) 안에서 겹칠 수 있는지 봅니다.
        foreach (InventorySlot slot in List_Slots)
        {
            if (slot.itemID == itemId)
            {
                if (slot.itemCount < masterData.MaxStack)
                {
                    slot.itemCount += count;
                    Debug.Log($"[가방 데이터] 기존 슬롯에 [{masterData.Name}] 수량 추가. 현재: {slot.itemCount}개");
                    return true;
                }
            }
        }

        // 2. 겹칠 칸이 없다면 새로운 빈 칸(ID가 0인 칸)을 찾아서 새로 입주 시킵니다.
        foreach (InventorySlot slot in List_Slots)
        {
            if (slot.itemID == 0)
            {
                slot.itemID = itemId;
                slot.itemCount = count;
                Debug.Log($"[가방 데이터] 빈 슬롯에 [{masterData.Name}] 새롭게 획득 시전.");
                return true;
            }
        }

        Debug.LogWarning("[가방 데이터] 가방 공간이 가득 차서 아이템을 더 넣을 수 없습니다!");
        return false;
    }
}