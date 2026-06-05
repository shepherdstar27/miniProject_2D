using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : UIBase
{
    private static EquipmentUI _instance;
    public static EquipmentUI Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    [Header("ShipName Texts")]
    [SerializeField] private TMP_Text Text_ShipName;

    [Header("FrontGun Texts")]
    [SerializeField] private TMP_Text Text_FrontGunName;
    [SerializeField] private TMP_Text Text_FrontGunDamage;
    [SerializeField] private TMP_Text Text_FrontGunCoolTime;

    [Header("DeckGun Texts")]
    [SerializeField] private TMP_Text Text_DeckGunName;
    [SerializeField] private TMP_Text Text_DeckGunDamage;
    [SerializeField] private TMP_Text Text_DeckGunCoolTime;

    [Header("SternGun Texts")]
    [SerializeField] private TMP_Text Text_SternGunName;
    [SerializeField] private TMP_Text Text_SternGunDamage;
    [SerializeField] private TMP_Text Text_SternGunCoolTime;

    [Header("EquipmentUI 그룹")]
    [SerializeField] private Transform Transform_Searchlight;
    [SerializeField] private Transform Transform_Engine;
    [SerializeField] private Transform Transform_AdditionalEquipment;
    [SerializeField] private Transform Transform_FrontGun;
    [SerializeField] private Transform Transform_DeckGun;
    [SerializeField] private Transform Transform_SternGun;
    [SerializeField] private Transform Transform_Ship;

    [Header("Engine 그룹")]
    [SerializeField] private TMP_Text Text_EngineName;
    [SerializeField] private Image Image_EngineIcon;

    [Header("Ship그룹")]
    [SerializeField] private Image Image_ShipIcon;

    [Header("Weapon그룹")]
    [SerializeField] private Image Image_WeaponIcon;

    [Header("Inventory Resource Texts")]
    [SerializeField] private TextMeshProUGUI TextMesh_GoldAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_FuelAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_SuppliesAmount;

    [SerializeField] private UIButton Button_CloseEquipment;

    [Header("Slot Prefab")]
    [SerializeField] private GameObject GameObject_SlotUiPrefab;

    [Header("오른쪽 패널 그룹")]
    [SerializeField] private GameObject GameObject_NormalInventoryPanel;
    [SerializeField] private GameObject GameObject_EquipmentShopPanel;


    private List<GameObject> _createdSlots = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        BindEvents();
        UpdateRightSidePanel();
        RefreshUI(); // UI가 열릴 때 최신 정보로 갱신

    }

    private void BindEvents()
    {
        if (Button_CloseEquipment != null)
        {
            Button_CloseEquipment.BindOnClickButtonEvent(OnClick_CloseEquipment);
        }
    }

    private void OnClick_CloseEquipment()
    {
        // UIManager의 핵심 로직을 통해 UI 닫기
        UIManager.Instance.CloseUI(UIType.EquipmentUI);
    }

    // 외부(슬롯 등)에서 장비가 변경되었을 때 호출하는 갱신 메서드
    public void RefreshUI()
    {
        PlayerModel _playerModel = GameManager.Instance.PlayerModel;
        if (_playerModel == null) return;

        //1. 각 장비 타입별 변수 선언 및 초기화
        WeaponData _deckGunData = null;
        WeaponData _FrontData = null;
        WeaponData _SternData = null;

        EngineData _engineData = null;
        ShipData _shipData = null;


        // 데이터 조회
        if (GameDataManager.Instance != null)
        {
            if (string.IsNullOrEmpty(_playerModel.EquippedWeaponId) == false)
            {
                // [수정 완료] 갑판포 데이터
                _deckGunData = GameDataManager.Instance.GetWeaponData(_playerModel.EquippedWeaponId);
            }

            if (string.IsNullOrEmpty(_playerModel.EquippedEngineId) == false)
            {
                // [수정 완료] 엔진 데이터는 _engineData 변수에 할당
                _engineData = GameDataManager.Instance.GetEngineData(_playerModel.EquippedEngineId);
            }

            if (string.IsNullOrEmpty(_playerModel.EquippedShipId) == false)
            {
                // [수정 완료] 배 데이터는 _shipData 변수에 할당
                _shipData = GameDataManager.Instance.GetShipData(_playerModel.EquippedShipId);
            }

        }

        // 2. 무기 UI 업데이트
        UpdateDeckGunUI(_deckGunData);
        UpdateFrontGunUI(_FrontData);
        UpdateSternGunUI(_SternData);

        UpdateEngineUI(_engineData);
        UpdateShipUI(_shipData);


        // 자원 갱신 예시
        TextMesh_GoldAmount.text = _playerModel.Gold.ToString();
        TextMesh_FuelAmount.text = _playerModel.Fuel.ToString();
        TextMesh_SuppliesAmount.text = _playerModel.Supplies.ToString();
    }

    private void UpdateDeckGunUI(WeaponData _weaponData)
    {
        // 장착된 무기 데이터가 없을 경우
        if (_weaponData == null)
        {
            if (Text_DeckGunName != null) Text_DeckGunName.text = "장비 없음";
            if (Text_DeckGunDamage != null) Text_DeckGunDamage.text = "-";
            if (Text_DeckGunCoolTime != null) Text_DeckGunCoolTime.text = "-";
            return;
        }

        // 정상적으로 데이터를 불러왔을 경우
        if (Text_DeckGunName != null) Text_DeckGunName.text = _weaponData.Name;
        if (Text_DeckGunDamage != null) Text_DeckGunDamage.text = _weaponData.Damage.ToString();
        if (Text_DeckGunCoolTime != null) Text_DeckGunCoolTime.text = _weaponData.FireCoolDown.ToString();

        if (Image_WeaponIcon != null)
        {
            if (string.IsNullOrEmpty(_weaponData.IconPath) == false)
            {
                // Resources 폴더에서 IconPath 경로의 Sprite를 불러옵니다.
                Sprite _loadedSprite = Resources.Load<Sprite>(_weaponData.IconPath);

                if (_loadedSprite != null)
                {
                    Image_WeaponIcon.sprite = _loadedSprite;
                    Image_WeaponIcon.enabled = true; // 이미지가 있으므로 활성화
                }
                else
                {
                    Debug.LogWarning($"[EquipmentUI] 무기 아이콘 로드 실패. 경로를 확인하세요: {_weaponData.IconPath}");
                    Image_WeaponIcon.enabled = false;
                }
            }
        }
        else
        {
            // 경로가 비어있을 경우
            Image_WeaponIcon.enabled = false;
        }
    }
    private void UpdateFrontGunUI(WeaponData _weaponData)
    {
        if (_weaponData == null)
        {
            // [수정 완료] FrontGun 텍스트 사용
            if (Text_FrontGunName != null) Text_FrontGunName.text = "장비 없음";
            if (Text_FrontGunDamage != null) Text_FrontGunDamage.text = "-";
            if (Text_FrontGunCoolTime != null) Text_FrontGunCoolTime.text = "-";
            return;
        }

        // [수정 완료] FrontGun 텍스트 사용
        if (Text_FrontGunName != null) Text_FrontGunName.text = _weaponData.Name;
        if (Text_FrontGunDamage != null) Text_FrontGunDamage.text = _weaponData.Damage.ToString();
        if (Text_FrontGunCoolTime != null) Text_FrontGunCoolTime.text = _weaponData.FireCoolDown.ToString();
    }
    private void UpdateSternGunUI(WeaponData _weaponData)
    {
        if (_weaponData == null)
        {
            // [수정 완료] SternGun 텍스트 사용
            if (Text_SternGunName != null) Text_SternGunName.text = "장비 없음";
            if (Text_SternGunDamage != null) Text_SternGunDamage.text = "-";
            if (Text_SternGunCoolTime != null) Text_SternGunCoolTime.text = "-";
            return;
        }

        // [수정 완료] SternGun 텍스트 사용
        if (Text_SternGunName != null) Text_SternGunName.text = _weaponData.Name;
        if (Text_SternGunDamage != null) Text_SternGunDamage.text = _weaponData.Damage.ToString();
        if (Text_SternGunCoolTime != null) Text_SternGunCoolTime.text = _weaponData.FireCoolDown.ToString();
    }



    private void UpdateEngineUI(EngineData _engineData)
    {
        // 장착된 데이터가 없을 경우
        if (_engineData == null)
        {
            if (Text_EngineName != null) Text_EngineName.text = "정보 없음";

            if (Image_EngineIcon != null)
            {
                Image_EngineIcon.sprite = null;
                Image_EngineIcon.enabled = false;
            }
            return;
        }

        // 2. 정상적으로 데이터를 불러왔을 경우 - 이름 표기
        if (Text_EngineName != null) Text_EngineName.text = _engineData.Name;

        // 3. 아이콘 이미지 로드 및 표기
        if (Image_EngineIcon != null)
        {
            if (string.IsNullOrEmpty(_engineData.IconPath) == false)
            {
                // Resources 폴더에서 IconPath 경로의 Sprite를 불러옵니다.
                Sprite _loadedSprite = Resources.Load<Sprite>(_engineData.IconPath);

                if (_loadedSprite != null)
                {
                    Image_EngineIcon.sprite = _loadedSprite;
                    Image_EngineIcon.enabled = true; // 이미지가 있으므로 활성화
                }
                else
                {
                    Debug.LogWarning($"[EquipmentUI] 엔진 아이콘 로드 실패. 경로를 확인하세요: {_engineData.IconPath}");
                    Image_EngineIcon.enabled = false;
                }
            }
            else
            {
                // 경로가 비어있을 경우
                Image_EngineIcon.enabled = false;
            }
        }
    }


    private void UpdateShipUI(ShipData _shipData)
    {
        if (_shipData == null)
        {
            if (Text_ShipName != null) Text_ShipName.text = "배 정보 없음";
            return;
        }

        if (Text_ShipName != null) Text_ShipName.text = _shipData.Name;


        // 3. 아이콘 이미지 로드 및 표기
        if (Image_ShipIcon != null)
        {
            if (string.IsNullOrEmpty(_shipData.IconPath) == false)
            {
                // Resources 폴더에서 IconPath 경로의 Sprite를 불러옵니다.
                Sprite _loadedSprite = Resources.Load<Sprite>(_shipData.IconPath);

                if (_loadedSprite != null)
                {
                    Image_ShipIcon.sprite = _loadedSprite;
                    Image_ShipIcon.enabled = true; // 이미지가 있으므로 활성화

                    Image_ShipIcon.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    Image_ShipIcon.rectTransform.localScale = Vector3.one;
                }
                else
                {
                    Debug.LogWarning($"[EquipmentUI] 배 이미지 로드 실패.  {_shipData.IconPath}");
                    Image_ShipIcon.enabled = false;
                }
            }
            else
            {
                // 경로가 비어있을 경우
                Image_ShipIcon.enabled = false;
            }
        }

    }


    private void UpdateRightSidePanel()
    {
        if (TradeManager.Instance != null && string.IsNullOrEmpty(TradeManager.Instance.CurrentTradeId) == false)
        {
            GameObject_EquipmentShopPanel.SetActive(true);
            GameObject_NormalInventoryPanel.SetActive(false);
        }

        else
        {
            GameObject_EquipmentShopPanel.SetActive(false);
            GameObject_NormalInventoryPanel.SetActive(true);
        }
    }
}




