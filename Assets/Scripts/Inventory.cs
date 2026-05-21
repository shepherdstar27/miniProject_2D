using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    public string String_ItemId = "";
    public int Int_Count = 0;
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }

    [Header("Special Resource Wallet Status")]
    [SerializeField] private int Int_GoldCount = 0;
    [SerializeField] private int Int_FuelCount = 0;
    [SerializeField] private int Int_SuppliesCount = 0;

    [Header("Live Cargo Slots Memory")]
    [SerializeField] private List<InventorySlotData> List_CargoSlots;

    private void Awake()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {
        Instance = this;
    }

    public void SetupInventorySlots(int cargoCapacity)
    {
        List_CargoSlots = new List<InventorySlotData>();

        // 기획된 화물칸 개수만큼 빈 공간 그릇을 순정 루프로 미리 파둡니다.
        for (int i = 0; i < cargoCapacity; i++)
        {
            InventorySlotData emptySlot = new InventorySlotData();
            List_CargoSlots.Add(emptySlot);
        }

        Debug.Log($"[인벤토리 백엔드] JSON 함선 스펙 동기화 완료. 총 {cargoCapacity}칸의 빈 화물창 가동.");

        // UI가 꺼져있으면 화면 생성 지시를 패스합니다.
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.CreateUIContainerSlots(cargoCapacity);
        }
    }

    // 아이템 습득 필터
    public void AddItemToInventory(string itemId, int count)
    {


        // 1: 특수 자원 3종 세트 검증 방화벽 (인벤토리 칸을 쓰지 않고 전용 그릇에 적재)
        if (itemId == "Gold_0001")
        {
            Int_GoldCount += count;
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.UpdateSpecialResourceText(Int_GoldCount, Int_FuelCount, Int_SuppliesCount);
            }
            return;
        }
        if (itemId == "Fuel_0002")
        {
            Int_FuelCount += count;
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.UpdateSpecialResourceText(Int_GoldCount, Int_FuelCount, Int_SuppliesCount);
            }
            return;
        }
        if (itemId == "Supplies_0003")
        {
            Int_SuppliesCount += count;
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.UpdateSpecialResourceText(Int_GoldCount, Int_FuelCount, Int_SuppliesCount);
            }
            return;
        }

        // 2: 일반 아이템 10개 상한 누적 연산 분기 공정 1단계 (이미 존재하는 같은 아이템 칸 채우기)
        for (int i = 0; i < List_CargoSlots.Count; i++)
        {
            if (List_CargoSlots[i].String_ItemId == itemId && List_CargoSlots[i].Int_Count < 10)
            {
                int currentAvailableSpace = 10 - List_CargoSlots[i].Int_Count;

                if (count <= currentAvailableSpace)
                {
                    List_CargoSlots[i].Int_Count += count;
                    count = 0;
                    break;
                }
                else
                {
                    List_CargoSlots[i].Int_Count = 10;
                    count -= currentAvailableSpace;
                }
            }
        }

        // 3: 10개 스택 오버플로우 발생 시 분기 공정 2단계 (새로운 빈 칸 찾아서 10개 단위 쪼개기 적재)
        if (count > 0)
        {
            for (int i = 0; i < List_CargoSlots.Count; i++)
            {
                if (string.IsNullOrEmpty(List_CargoSlots[i].String_ItemId) == true)
                {
                    List_CargoSlots[i].String_ItemId = itemId;

                    if (count <= 10)
                    {
                        List_CargoSlots[i].Int_Count = count;
                        count = 0;
                        break;
                    }
                    else
                    {
                        List_CargoSlots[i].Int_Count = 10;
                        count -= 10;
                    }
                }
            }
        }

        // 화물창 공간 전멸 대처 예외 처리 보험
        if (count > 0)
        {
            Debug.LogWarning($"[화물창 포화] 공간이 부족하여 아이템 [{itemId}] {count}개가 버려졌습니다.");
        }

        if (InventoryUI.Instance != null)
        {
            // 데이터 계산이 종료되었으므로 프론트엔드 UI 화면 새로고침 전송
            InventoryUI.Instance.RefreshInventoryDisplay(List_CargoSlots);
        }
    }

}