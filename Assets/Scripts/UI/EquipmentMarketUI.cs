using System.Collections.Generic;
using UnityEngine;

public class EquipmentMarketUI : MonoBehaviour
{
    [Header("UI Group Transforms")]
    [SerializeField] private Transform Transform_EquipmentScrollContent;

    [Header("Slot Prefab")]
    [SerializeField] private GameObject GameObject_EquipmentSlotUiPrefab;

    private List<GameObject> _createdSlots = new List<GameObject>();

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        ClearAllSlots();
        PopulateMarketProducts();
    }

    private void PopulateMarketProducts()
    {
        if (GameDataManager.Instance == null) return;

        // 1. 무기(갑판포) 테이블 상품 진열
        WeaponData _Weapon_0001 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0001");
        if (_Weapon_0001 != null)
        {
            CreateEquipmentSlot(_Weapon_0001.Id, _Weapon_0001.Name, _Weapon_0001.Price, _Weapon_0001.IconPath, "Weapon");
        }

        WeaponData _Weapon_0002 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0002");
        if (_Weapon_0002 != null)
        {
            CreateEquipmentSlot(_Weapon_0002.Id, _Weapon_0002.Name, _Weapon_0002.Price, _Weapon_0002.IconPath, "Weapon");
        }
        WeaponData _Weapon_0003 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0003");
        if (_Weapon_0003 != null)
        {
            CreateEquipmentSlot(_Weapon_0003.Id, _Weapon_0003.Name, _Weapon_0003.Price, _Weapon_0003.IconPath, "Weapon");
        }
        WeaponData _Weapon_0004 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0004");
        if (_Weapon_0004 != null)
        {
            CreateEquipmentSlot(_Weapon_0004.Id, _Weapon_0004.Name, _Weapon_0004.Price, _Weapon_0004.IconPath, "Weapon");
        }
        WeaponData _Weapon_0005 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0005");
        if (_Weapon_0005 != null)
        {
            CreateEquipmentSlot(_Weapon_0005.Id, _Weapon_0005.Name, _Weapon_0005.Price, _Weapon_0005.IconPath, "Weapon");
        }
        WeaponData _Weapon_0006 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0005");
        if (_Weapon_0006 != null)
        {
            CreateEquipmentSlot(_Weapon_0006.Id, _Weapon_0006.Name, _Weapon_0006.Price, _Weapon_0006.IconPath, "Weapon");
        }

        // 2. 엔진 테이블 상품 진열
        EngineData _Engine_0001 = GameDataManager.Instance.GetEngineData("Engine_0001");
        if (_Engine_0001 != null)
        {
            CreateEquipmentSlot(_Engine_0001.Id, _Engine_0001.Name, _Engine_0001.Price, _Engine_0001.IconPath, "Engine"); 
        }
        EngineData _Engine_0002 = GameDataManager.Instance.GetEngineData("Engine_0002");
        if (_Engine_0002 != null)
        {
            CreateEquipmentSlot(_Engine_0002.Id, _Engine_0002.Name, _Engine_0002.Price, _Engine_0002.IconPath, "Engine"); 
        }
        EngineData _Engine_0003 = GameDataManager.Instance.GetEngineData("Engine_0003");
        if (_Engine_0003 != null)
        {
            CreateEquipmentSlot(_Engine_0003.Id, _Engine_0003.Name, _Engine_0003.Price, _Engine_0003.IconPath, "Engine"); 
        }
        EngineData _Engine_0004 = GameDataManager.Instance.GetEngineData("Engine_0004");
        if (_Engine_0004 != null)
        {
            CreateEquipmentSlot(_Engine_0004.Id, _Engine_0004.Name, _Engine_0004.Price, _Engine_0004.IconPath, "Engine"); 
        }
        EngineData _Engine_0005 = GameDataManager.Instance.GetEngineData("Engine_0005");
        if (_Engine_0005 != null)
        {
            CreateEquipmentSlot(_Engine_0005.Id, _Engine_0005.Name, _Engine_0005.Price, _Engine_0005.IconPath, "Engine"); 
        }


    }

    private void CreateEquipmentSlot(string itemId, string itemName, int priceText, string iconPath, string itemType)
    {
        if (GameObject_EquipmentSlotUiPrefab == null || Transform_EquipmentScrollContent == null) return;

        GameObject _slotObj = Instantiate(GameObject_EquipmentSlotUiPrefab, Transform_EquipmentScrollContent);
        _createdSlots.Add(_slotObj);

        EquipmentMarketSlotUI _slotUI = _slotObj.GetComponent<EquipmentMarketSlotUI>();
        if (_slotUI != null)
        {
            _slotUI.SetupSlotDetails(itemId, itemName, priceText, iconPath, itemType);
        }
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < _createdSlots.Count; i++)
        {
            if (_createdSlots[i] != null)
            {
                Destroy(_createdSlots[i]);
            }
        }
        _createdSlots.Clear();
    }
}