using UnityEngine;
using UnityEngine.EventSystems;

// EquipmentUI의 각 장비 슬롯(FrontGun, DeckGun 등)에 부착할 스크립트
public class EquipmentSlotUI : MonoBehaviour, IDropHandler
{
    // 드래그 중인 아이템을 이 슬롯에 놓았을 때 발생
    public void OnDrop(PointerEventData _eventData)
    {
        // 놓여진 오브젝트에서 InventorySlotUI 컴포넌트를 찾음
        GameObject _droppedObject = _eventData.pointerDrag;
        if (_droppedObject != null)
        {
            InventorySlotUI _draggedSlot = _droppedObject.GetComponent<InventorySlotUI>();
            if (_draggedSlot != null)
            {
                // 인벤토리 슬롯의 장착 메서드 호출
                _draggedSlot.EquipCurrentItem();
            }
        }
    }
}