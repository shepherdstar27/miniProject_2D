using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlotData
{
    private string _itemId = "";
    private int _count = 0;

    public string ItemId
    {
        get { return _itemId; }
        set { _itemId = value; }
    }

    public int Count
    {
        get { return _count; }
        set { _count = value; }
    }
}

[Serializable]
public class PlayerModel
{
    private string _playerName;
    private int _playerTotalExp;
    private string _lastMapDataId;
    private Vector3 _lastMapPosition;

    // 장비 ID 데이터
    private string _equippedShipId = "ship_0001";
    private string _equippedEngineId = "Engine_0005";
    private string _equippedWeaponId = "Weapon_Canon_0006";

    // 인벤토리 핵심 데이터
    private int _gold;
    private int _fuel;
    private int _supplies;
    private List<InventorySlotData> _cargoSlots = new List<InventorySlotData>();

    // 프로퍼티 개방 (람다 배제)
    public string PlayerName { get { return _playerName; } set { _playerName = value; } }
    public int PlayerTotalExp { get { return _playerTotalExp; } set { _playerTotalExp = value; } }
    public string LastMapDataId { get { return _lastMapDataId; } set { _lastMapDataId = value; } }
    public Vector3 LastMapPosition { get { return _lastMapPosition; } set { _lastMapPosition = value; } }

    public string EquippedShipId { get { return _equippedShipId; } set { _equippedShipId = value; } }
    public string EquippedEngineId { get { return _equippedEngineId; } set { _equippedEngineId = value; } }
    public string EquippedWeaponId { get { return _equippedWeaponId; } set { _equippedWeaponId = value; } }

    public int Gold { get { return _gold; } set { _gold = value; } }
    public int Fuel { get { return _fuel; } set { _fuel = value; } }
    public int Supplies { get { return _supplies; } set { _supplies = value; } }
    public List<InventorySlotData> CargoSlots { get { return _cargoSlots; } set { _cargoSlots = value; } }

    public void InitializeCargo(int cargoCapacity)
    {
        _cargoSlots = new List<InventorySlotData>();
        for (int i = 0; i < cargoCapacity; i++)
        {
            InventorySlotData slot = new InventorySlotData();
            slot.ItemId = "";
            slot.Count = 0;
            _cargoSlots.Add(slot);
        }
    }

    public void AddItem(string itemId, int count)
    {
        // 1. 특수 재화 우선 처리
        if (itemId == "Gold_0001") { _gold += count; return; }
        if (itemId == "Fuel_0002") { _fuel += count; return; }
        if (itemId == "Supplies_0003") { _supplies += count; return; }

        // 2. [지연 초기화 공정]: 화물창이 비어있다면 임의의 수가 아닌 배의 실제 스펙을 조회합니다.
        if (_cargoSlots == null || _cargoSlots.Count == 0)
        {
            if (GameDataManager.Instance != null)
            {
                ShipData shipData = GameDataManager.Instance.GetShipData(_equippedShipId);
                if (shipData != null)
                {
                    InitializeCargo(shipData.Cargo);
                    Debug.Log($"[PlayerModel] 화물창 자동 초기화 완료: {_equippedShipId} 스펙에 따라 {shipData.Cargo}칸 배정.");
                }
                else
                {
                    Debug.LogWarning($"[PlayerModel] 에러: {_equippedShipId} 배의 데이터를 찾을 수 없어 아이템 적재가 불가합니다.");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("[PlayerModel] GameDataManager가 로드되지 않아 화물창 스펙을 조회할 수 없습니다.");
                return;
            }
        }

        // 3. 일반 아이템 적재 로직
        int remainingCount = count;

        foreach (InventorySlotData slot in _cargoSlots)
        {
            if (slot.ItemId == itemId && slot.Count < 10)
            {
                int space = 10 - slot.Count;
                int add = Mathf.Min(remainingCount, space);
                slot.Count += add;
                remainingCount -= add;
                if (remainingCount <= 0) return;
            }
        }

        if (remainingCount > 0)
        {
            foreach (InventorySlotData slot in _cargoSlots)
            {
                if (string.IsNullOrEmpty(slot.ItemId))
                {
                    slot.ItemId = itemId;
                    int add = Mathf.Min(remainingCount, 10);
                    slot.Count = add;
                    remainingCount -= add;
                    if (remainingCount <= 0) return;
                }
            }

            Debug.LogWarning($"[PlayerModel] 화물창이 가득 차서 {itemId} {remainingCount}개를 적재하지 못했습니다.");
        }
    }

    public void RemoveItem(string itemId, int count)
    {
        if (itemId == "Gold_0001") { _gold = Mathf.Max(0, _gold - count); return; }
        if (itemId == "Fuel_0002") { _fuel = Mathf.Max(0, _fuel - count); return; }
        if (itemId == "Supplies_0003") { _supplies = Mathf.Max(0, _supplies - count); return; }

        int remainingCount = count;

        for (int i = _cargoSlots.Count - 1; i >= 0; i--)
        {
            InventorySlotData slot = _cargoSlots[i];
            if (slot.ItemId == itemId)
            {
                if (slot.Count <= remainingCount)
                {
                    remainingCount -= slot.Count;
                    slot.ItemId = "";
                    slot.Count = 0;
                }
                else
                {
                    slot.Count -= remainingCount;
                    remainingCount = 0;
                }
                if (remainingCount <= 0) break;
            }
        }
    }


    public void ResetToDefault()
    {
        // 1. 소지금 및 특수 재화 초기화 
        _gold = 100;
        _fuel = 10;      // 예: 기본 연료 100
        _supplies = 10;  // 예: 기본 보급품 100

        // 2. 소지품(화물창) 비우기
        if (_cargoSlots != null)
        {
            foreach (InventorySlotData slot in _cargoSlots)
            {
                slot.ItemId = "";
                slot.Count = 0;
            }
        }

        // 3. 시작 위치 및 맵 초기화 (실제 게임의 첫 시작 맵 ID와 좌표로 설정)
        _lastMapPosition = Vector3.zero;

        // 4. (선택) 장비도 초기화해야 한다면 기본 장비 ID로 덮어씌웁니다.
        _equippedShipId = "ship_0001";
        _equippedEngineId = "Engine_0001";
        _equippedWeaponId = "Weapon_Canon_0001";

        Debug.Log("[PlayerModel] 플레이어 데이터가 초기 시작 상태로 리셋되었습니다.");
    }



}