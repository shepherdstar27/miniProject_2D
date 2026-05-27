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
    [SerializeField] public List<InventorySlotData> List_CargoSlots;

    private void Awake()
    {
        // 이미 인스턴스가 있다면 파괴 (중복 방지)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지

        Debug.Log($"[인벤토리] Awake 호출됨! 내 오브젝트 이름: {gameObject.name}, 인스턴스 ID: {GetInstanceID()}");
    }

    public List<InventorySlotData> GetCargoSlots()
    {
        return List_CargoSlots;
    }

    public void SetupInventorySlots(int cargoCapacity)
    {
        List_CargoSlots = new List<InventorySlotData>();
        for (int i = 0; i < cargoCapacity; i++)
        {
            // 빈 슬롯 데이터를 확실하게 생성하여 리스트에 넣습니다.
            InventorySlotData slot = new InventorySlotData();
            slot.String_ItemId = ""; // 명시적으로 빈 값 설정
            slot.Int_Count = 0;
            List_CargoSlots.Add(slot);
        }
        Debug.Log($"[인벤토리] {cargoCapacity}칸의 화물창이 깨끗하게 초기화되었습니다.");
    }

    public void AddItemToInventory(string itemId, int count)
    {
        Debug.Log($"[인벤토리] AddItemToInventory 호출됨! 내 오브젝트 이름: {gameObject.name}, 인스턴스 ID: {GetInstanceID()}, 리스트 개수: {List_CargoSlots?.Count}");
        Debug.Log($"[인벤토리] 데이터 저장 중인 나 자신 ID: {GetInstanceID()}");
        // 1. 초기화 안전장치
        if (List_CargoSlots == null || List_CargoSlots.Count == 0)
        {
            SetupInventorySlots(200);
        }

        // 2. 특수 자원 처리
        if (itemId == "Gold_0001") { Int_GoldCount += count; UpdateSpecialUI(); return; }
        if (itemId == "Fuel_0002") { Int_FuelCount += count; UpdateSpecialUI(); return; }
        if (itemId == "Supplies_0003") { Int_SuppliesCount += count; UpdateSpecialUI(); return; }

        // 3. 일반 아이템 로직 (데이터 적재)
        int remainingCount = count;

        // 이미 있는 슬롯 채우기
        foreach (var slot in List_CargoSlots)
        {
            if (slot.String_ItemId == itemId && slot.Int_Count < 10)
            {
                int space = 10 - slot.Int_Count;
                int add = Mathf.Min(remainingCount, space);
                slot.Int_Count += add;
                remainingCount -= add;
                if (remainingCount <= 0) break;
            }
        }

        // 빈 슬롯 찾기
        if (remainingCount > 0)
        {
            foreach (var slot in List_CargoSlots)
            {
                if (string.IsNullOrEmpty(slot.String_ItemId))
                {
                    slot.String_ItemId = itemId;
                    int add = Mathf.Min(remainingCount, 10);
                    slot.Int_Count = add;
                    remainingCount -= add;
                    if (remainingCount <= 0) break;
                }
            }
        }

        // 4. UI 갱신 (살아있을 때만 호출! 에러가 나지 않도록 체크)
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RefreshInventoryDisplay(List_CargoSlots);
        }
        else
        {
            Debug.Log($"[인벤토리] UI가 현재 닫혀있습니다. 데이터 {itemId}를 저장만 했습니다.");
        }
    }

    private void UpdateSpecialUI()
    {
        if (InventoryUI.Instance != null)
            InventoryUI.Instance.UpdateSpecialResourceText(Int_GoldCount, Int_FuelCount, Int_SuppliesCount);
    }
    public int GetGoldCount()
    {
        return Int_GoldCount;
    }

    public int GetFuelCount()
    {
        return Int_FuelCount;
    }

    public int GetSuppliesCount()
    {
        return Int_SuppliesCount;
    }

}