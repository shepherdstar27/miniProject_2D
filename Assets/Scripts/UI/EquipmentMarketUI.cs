using System.Collections.Generic;
using UnityEngine;

public class EquipmentMarketUI : MonoBehaviour
{
    [Header("UI Group Transforms")]
    [SerializeField] private Transform Transform_EquipmentScrollContent;

    [Header("Tab Buttons")]
    [SerializeField] private UIButton Button_LightTab;
    [SerializeField] private UIButton Button_EngineTab;
    [SerializeField] private UIButton Button_AccessoryTab;
    [SerializeField] private UIButton Button_WeaponTab;
    [SerializeField] private UIButton Button_ShipTab;


    [Header("Slot Prefab")]
    [SerializeField] private GameObject GameObject_EquipmentSlotUiPrefab;

    private List<GameObject> _createdSlots = new List<GameObject>();


    private string _currentCategory = "Light";


    private void OnEnable()
    {
        BindEvents();
        RefreshUI();
    }

    private void BindEvents()
    {
        // 메서드 연결
        if (Button_LightTab != null) Button_LightTab.BindOnClickButtonEvent(OnClick_LightTab);
        if (Button_EngineTab != null) Button_EngineTab.BindOnClickButtonEvent(OnClick_EngineTab);
        if (Button_AccessoryTab != null) Button_AccessoryTab.BindOnClickButtonEvent(OnClick_AccessoryTab);
        if (Button_WeaponTab != null) Button_WeaponTab.BindOnClickButtonEvent(OnClick_WeaponTab);
        if (Button_ShipTab != null) Button_ShipTab.BindOnClickButtonEvent(OnClick_ShipTab);
    }


    // 탭 버튼 클릭 이벤트
    private void OnClick_LightTab()
    {
        _currentCategory = "Light";
        RefreshUI();
    }
    private void OnClick_EngineTab()
    {
        _currentCategory = "Engine";
        RefreshUI();
    }
    private void OnClick_AccessoryTab()
    {
        _currentCategory = "Accessory";
        RefreshUI();
    }
    private void OnClick_WeaponTab()
    {
        _currentCategory = "Weapon";
        RefreshUI();
    }
    private void OnClick_ShipTab()
    {
        _currentCategory = "Ship";
        RefreshUI();
    }


    // UI 갱신 
    public void RefreshUI()
    {
        ClearAllSlots();
        PopulateMarketProducts();
    }


    // 현재 카테고리 상태에 따라 분기
        private void PopulateMarketProducts()
    {
        if (GameDataManager.Instance == null) return;

        if (_currentCategory == "Light")
        {
            LoadLightProducts();
        }
        else if (_currentCategory == "Engine")
        {
            LoadEngineProducts();
        }
        else if (_currentCategory == "Accessory")
        {
            LoadAccessoryProducts();
        }
        else if (_currentCategory == "Weapon")
        {
            LoadWeaponProducts();
        }
        else if (_currentCategory == "Ship")
        {
            LoadShipProducts();
        }
    }

    // 상품 진열
    private void LoadLightProducts()
    {
        // 탐조등 채울 자리
    }
    private void LoadEngineProducts()
    {
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
    private void LoadAccessoryProducts()
    {
        // 악세서리 채울 자리
    }
    private void LoadWeaponProducts()
    {
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
        WeaponData _Weapon_0006 = GameDataManager.Instance.GetWeaponData("Weapon_Canon_0006");
        if (_Weapon_0006 != null)
        {
            CreateEquipmentSlot(_Weapon_0006.Id, _Weapon_0006.Name, _Weapon_0006.Price, _Weapon_0006.IconPath, "Weapon");
        }
    }
    private void LoadShipProducts()
    {
        ShipData _Ship_0001 = GameDataManager.Instance.GetShipData("ship_0001");
        if (_Ship_0001 != null)
        {
            CreateEquipmentSlot(_Ship_0001.Id, _Ship_0001.Name, _Ship_0001.Price, _Ship_0001.IconPath, "Ship");
        }

        ShipData _Ship_0002 = GameDataManager.Instance.GetShipData("ship_0002");
        if (_Ship_0002 != null)
        {
            CreateEquipmentSlot(_Ship_0002.Id, _Ship_0002.Name, _Ship_0002.Price, _Ship_0002.IconPath, "Ship");
        }

        ShipData _Ship_0003 = GameDataManager.Instance.GetShipData("ship_0003");
        if (_Ship_0003 != null)
        {
            CreateEquipmentSlot(_Ship_0003.Id, _Ship_0003.Name, _Ship_0003.Price, _Ship_0003.IconPath, "Ship");
        }
    }


    private void CreateEquipmentSlot(string itemId, string itemName, int price, string iconPath, string itemType)
    {
        if (GameObject_EquipmentSlotUiPrefab == null || Transform_EquipmentScrollContent == null) return;

        GameObject _slotObj = Instantiate(GameObject_EquipmentSlotUiPrefab, Transform_EquipmentScrollContent);
        _createdSlots.Add(_slotObj);

        EquipmentMarketSlotUI _slotUI = _slotObj.GetComponent<EquipmentMarketSlotUI>();
        if (_slotUI != null)
        {
            _slotUI.SetupSlotDetails(itemId, itemName, price, iconPath, itemType);
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