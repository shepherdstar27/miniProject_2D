using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentMarketSlotUI : MonoBehaviour
{
    [Header("Slot Display Graphics")]
    [SerializeField] private TMP_Text Text_EquipmentName;
    [SerializeField] private TMP_Text Text_EquipmentPrice;
    [SerializeField] private Image Image_EquipmentIcon;
    [SerializeField] private UIButton Button_PurchaseEquipment;

    [Header("Assigned Product Data")]
    private string _currentProductId = "";
    private string _currentProductType = ""; // "Weapon", "Engine", "Ship" 구분용
    private int _parsedPrice = 0;

    private void OnEnable()
    {
        BindEvents();
    }

    private void BindEvents()
    {
        if (Button_PurchaseEquipment != null)
        {
            Button_PurchaseEquipment.BindOnClickButtonEvent(OnClick_PurchaseEquipment);
        }
    }

    public void SetupSlotDetails(string itemId, string itemName, string priceText, string iconPath, string itemType)
    {
        _currentProductId = itemId;
        _currentProductType = itemType;

        if (Text_EquipmentName != null) Text_EquipmentName.text = itemName;
        if (Text_EquipmentPrice != null) Text_EquipmentPrice.text = priceText;

        // 가격 안전 파싱
        if (int.TryParse(priceText.Trim(), out int _price) == true)
        {
            _parsedPrice = _price;
        }

        // 아이콘 이미지 로드
        if (Image_EquipmentIcon != null && string.IsNullOrEmpty(iconPath) == false)
        {
            Sprite _loadedSprite = Resources.Load<Sprite>(iconPath);
            if (_loadedSprite != null)
            {
                Image_EquipmentIcon.sprite = _loadedSprite;
                Image_EquipmentIcon.enabled = true;
            }
            else
            {
                Image_EquipmentIcon.enabled = false;
            }
        }
    }

    private void OnClick_PurchaseEquipment()
    {
        if (string.IsNullOrEmpty(_currentProductId) == true || GameManager.Inst == null) return;

        PlayerModel _playerModel = GameManager.Inst.PlayerModel;
        if (_playerModel == null) return;

        // 1. 소지금 검증
        if (_playerModel.Gold < _parsedPrice)
        {
            Debug.LogWarning("[장비 상점] 소지금이 부족하여 장비를 구매할 수 없습니다.");
            return;
        }

        // 2. 재화 차감 및 장비 변경 처리 (프로젝트 프로퍼티 Setter 및 메서드 규칙 활용)
        _playerModel.Gold -= _parsedPrice;

        if (_currentProductType == "Weapon")
        {
            _playerModel.EquippedWeaponId = _currentProductId;
            Debug.Log($"[장비 상점] 무기 변경 완료: {_currentProductId}");
        }
        else if (_currentProductType == "Engine")
        {
            _playerModel.EquippedEngineId = _currentProductId;
            Debug.Log($"[장비 상점] 엔진 변경 완료: {_currentProductId}");
        }
        else if (_currentProductType == "Ship")
        {
            _playerModel.EquippedShipId = _currentProductId;
            Debug.Log($"[장비 상점] 선박 변경 완료: {_currentProductId}");
        }

        // 3. 거래가 완료되었으므로 좌측 장비 장착창 전체 UI 실시간 리프레시 호출
        if (EquipmentUI.Instance != null)
        {
            EquipmentUI.Instance.RefreshUI();
        }

        // 4. 상점 창 자체의 자원 표기 및 상태 리프레시 (필요 시)
        // 현재 슬롯의 부모 패널 컴포넌트를 찾아 리프레시 처리를 할 수 있습니다.
        EquipmentMarketUI _marketUI = GetComponentInParent<EquipmentMarketUI>();
        if (_marketUI != null)
        {
            _marketUI.RefreshUI();
        }
    }
}