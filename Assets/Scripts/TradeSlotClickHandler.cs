using UnityEngine;
using UnityEngine.EventSystems;

public enum TradeSlotType
{
    Shop,
    PlayerInventory,
    BuyCart,
    SellCart
}

public class TradeSlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    private string _itemId = "";
    private TradeSlotType _slotType;

    public string ItemId
    {
        get { return _itemId; }
        set { _itemId = value; }
    }

    public TradeSlotType SlotType
    {
        get { return _slotType; }
        set { _slotType = value; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(_itemId) == true) return;

        Debug.Log($"[상점 클릭 감지] {_slotType} 구역의 아이템 [{_itemId}] 클릭됨!");

        if (TradeUI.Instance != null)
        {
            TradeUI.Instance.HandleSlotClick(_slotType, _itemId);
        }
    }
}