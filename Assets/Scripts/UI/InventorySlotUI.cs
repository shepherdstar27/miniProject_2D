using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image Image_Icon;
    [SerializeField] private TextMeshProUGUI Text_Count;

    private InventorySlot _currentSlotData;

    // 슬롯 기물에 실시간 알맹이 데이터를 주입하여 시각화하는 마술사 함수
    public void SetupSlot(InventorySlot slotData)
    {
        if (slotData == null || slotData.itemID == 0)
        {
            ClearSlot();
            return;
        }

        // 데이터 드리븐의 심장: 동적 획득한 데이터 ID를 마스터 기획 DB에 대입해 기획 스펙을 빼옵니다.
        ItemData originData = GameDataManager.Instance.GetItemData(slotData.itemID);

        if (originData != null)
        {
            // Resources.Load를 통해 에셋 폴더 내의 아이콘 이미지를 동적으로 낚아채와 스크라이트에 주입합니다.
            Sprite loadedIcon = Resources.Load<Sprite>(originData.IconPath);
            if (loadedIcon != null)
            {
                Image_Icon.sprite = loadedIcon;
            }

            // 수량 표시 (기획 마스터 데이터 상 Stack이 1개 초과가 가능한 물건일 때만 수량 켜기)
            Text_Count.text = slotData.itemCount.ToString();
            if (slotData.itemCount > 1)
            {
                Text_Count.gameObject.SetActive(true);
            }
            else
            {
                Text_Count.gameObject.SetActive(false);
            }
        }
    }

    public void ClearSlot()
    {
        _currentSlotData = null;
        Image_Icon.sprite = null;
        Text_Count.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 빈 슬롯이거나 데이터가 없다면 툴팁을 띄우지 않고 리턴합니다.
        if (_currentSlotData == null || _currentSlotData.itemID == 0)
        {
            return;
        }

        // 데이터 드리븐: 아이템 ID를 마스터 테이블에 대입해 기획 정보를 원격 조회합니다.
        ItemData originData = GameDataManager.Instance.GetItemData(_currentSlotData.itemID);
        if (originData != null)
        {
            // 툴팁 매니저에게 기획 원본 데이터를 던지며 출력을 요청합니다.
            ItemTooltipUI.Instance.ShowTooltip(originData);
        }
    }

    // 슬롯에서 마우스 커서가 벗어났을 때 실행되는 유니티 순정 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 떠나면 툴팁창을 즉시 닫습니다.
        ItemTooltipUI.Instance.HideTooltip();
    }


}