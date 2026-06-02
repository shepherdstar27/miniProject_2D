using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CanvasGroup))] // 드래그 시 레이캐스트 제어를 위해 필요
public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Slot Display Graphics")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_Price;
    [SerializeField] private TextMeshProUGUI TextMesh_StackCount;

    [Header("Live Assigned Slot Asset")]
    [SerializeField] private string _currentSlotItemId = "";
    [SerializeField] private int _currentPrice = 0;
    [SerializeField] private int _currentCount = 0;

    // --- 드래그 앤 드롭을 위한 추가 변수 ---
    private Vector3 _originalPosition;
    private Transform _originalParent;
    private CanvasGroup CanvasGroup_Slot;

    private void Awake()
    {
        // 유니티 참조 컴포넌트 캐싱
        CanvasGroup_Slot = GetComponent<CanvasGroup>();
        if (CanvasGroup_Slot == null)
        {
            CanvasGroup_Slot = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetupSlotDetails(string itemId, int count)
    {
        _currentSlotItemId = itemId;
        _currentCount = count;

        if (string.IsNullOrEmpty(itemId) == true || count <= 0)
        {
            ClearSlotGraphic();
            return;
        }

        if (GameDataManager.Instance != null)
        {
            ItemData itemMaster = GameDataManager.Instance.GetItemData(itemId);

            if (itemMaster != null)
            {
                // [해결 1] 가격 텍스트 안전 파싱 및 강제 활성화
                if (TextMesh_Price != null)
                {
                    string cleanPrice = "";
                    if (string.IsNullOrEmpty(itemMaster.Price) == false)
                    {
                        cleanPrice = itemMaster.Price.Trim(); // 혹시 모를 공백 제거
                    }

                    int basePrice = 0;
                    if (int.TryParse(cleanPrice, out basePrice) == true)
                    {
                        int finalPrice = basePrice;

                        if (TradeManager.Instance != null)
                        {
                            finalPrice = TradeManager.Instance.GetItemTradePrice(itemId, basePrice);
                        }

                        _currentPrice = finalPrice;
                        TextMesh_Price.text = finalPrice.ToString();
                        TextMesh_Price.gameObject.SetActive(true); // 컴포넌트 enabled 대신 오브젝트 자체를 On
                    }
                    else
                    {
                        Debug.LogWarning($"[가격 에러] 아이템 {itemId}의 Price 값 '{itemMaster.Price}'을 숫자로 변환할 수 없습니다.");
                        TextMesh_Price.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogWarning("[UI 에러] Slot 프리팹에 TextMesh_Price가 인스펙터에 할당되지 않았습니다!");
                }

                if (Image_ItemIcon != null)
                {
                    Sprite loadedSprite = Resources.Load<Sprite>(itemMaster.IconPath);
                    if (loadedSprite != null)
                    {
                        Image_ItemIcon.sprite = loadedSprite;
                        Image_ItemIcon.enabled = true;
                    }
                }
            }
        }

        if (TextMesh_StackCount != null)
        {
            TextMesh_StackCount.text = count.ToString();
            TextMesh_StackCount.enabled = true;
        }
    }

    public void ClearSlotGraphic()
    {
        _currentSlotItemId = "";
        _currentCount = 0;
        _currentPrice = 0;

        if (Image_ItemIcon != null)
        {
            Image_ItemIcon.sprite = null;
            Image_ItemIcon.enabled = false;
        }
        if (TextMesh_StackCount != null)
        {
            TextMesh_StackCount.enabled = false;
        }
        if (TextMesh_Price != null)
        {
            TextMesh_Price.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        if (InventoryUI.Instance != null && InventoryUI.Instance.GetTooltipUI() != null)
        {
            InventoryUI.Instance.GetTooltipUI().RenderItemTooltip(_currentSlotItemId);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        // 1. 기존: 클릭 시 툴팁 표시
        if (InventoryUI.Instance != null && InventoryUI.Instance.GetTooltipUI() != null)
        {
            InventoryUI.Instance.GetTooltipUI().RenderItemTooltip(_currentSlotItemId);
        }

        // 2. 추가: 더블 클릭 시 장착 시도
        if (eventData.clickCount == 2)
        {
            EquipCurrentItem();
        }
    }

    // ==========================================
    // --- 추가된 드래그 앤 드롭 및 장착 로직 ---
    // ==========================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        // 원래 위치와 부모 기억
        _originalPosition = transform.position;
        _originalParent = transform.parent;

        // 다른 UI에 가려지지 않도록 화면 최상단(EquipmentUI 또는 루트 캔버스)으로 이동
        if (EquipmentUI.Instance != null)
        {
            transform.SetParent(EquipmentUI.Instance.transform);
        }

        // 드랍 판정을 위해 드래그 중인 이 오브젝트의 레이캐스트를 끕니다.
        if (CanvasGroup_Slot != null)
        {
            CanvasGroup_Slot.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        // 마우스 포인터를 따라다님
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        // 인벤토리 슬롯 자체는 드랍 성공 여부와 상관없이 원래 자리로 복구
        transform.SetParent(_originalParent);
        transform.position = _originalPosition;

        // 레이캐스트 다시 켬
        if (CanvasGroup_Slot != null)
        {
            CanvasGroup_Slot.blocksRaycasts = true;
        }
    }

    public void EquipCurrentItem()
    {
        if (string.IsNullOrEmpty(_currentSlotItemId) == true) return;

        // 게임 데이터 매니저를 통해 아이템 정보를 불러오고 무기/장비인지 검증
        if (GameDataManager.Instance != null)
        {
            ItemData itemMaster = GameDataManager.Instance.GetItemData(_currentSlotItemId);
            if (itemMaster != null)
            {
                // GameManager의 PlayerModel에 장착 데이터를 넘김 (메서드명은 PlayerModel 구조에 맞게 적용)
                if (GameManager.Instance != null && GameManager.Instance.PlayerModel != null)
                {
                    // TODO: PlayerModel 내부에 무기 장착 처리 메서드를 연결하세요.
                    // 예시: GameManager.Instance.PlayerModel.EquipWeapon(itemMaster);

                    // 장비창 UI 갱신
                    if (EquipmentUI.Instance != null)
                    {
                        EquipmentUI.Instance.RefreshUI();
                    }
                }
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.PlayerModel != null)
        {
            // 데이터 클래스 전체가 아닌 ID(문자열)만 넘겨서 갱신
            GameManager.Instance.PlayerModel.EquipWeapon(_currentSlotItemId);

            if (EquipmentUI.Instance != null)
            {
                EquipmentUI.Instance.RefreshUI();
            }
        }
    }
}