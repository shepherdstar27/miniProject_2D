using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Slot Display Graphics")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_Price;
    [SerializeField] private TextMeshProUGUI TextMesh_StackCount;

    [Header("Live Assigned Slot Asset")]
    [SerializeField] private string _currentSlotItemId = "";
    [SerializeField] private int _currentPrice = 0;
    [SerializeField] private int _currentCount = 0;

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

        if (InventoryUI.Instance != null && InventoryUI.Instance.GetTooltipUI() != null)
        {
            InventoryUI.Instance.GetTooltipUI().RenderItemTooltip(_currentSlotItemId);
        }
    }
}