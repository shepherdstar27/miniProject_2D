using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image Image_Icon;
    [SerializeField] private Text Text_Count;

    // 슬롯 기물에 실시간 알맹이 데이터를 주입하여 시각화하는 마술사 함수
    public void SetupSlot(InventorySlot slotData)
    {
        if (slotData == null || slotData.itemID == 0)
        {
            ClearSlot();
            return;
        }

        // ⭕ 데이터 드리븐의 심장: 동적 획득한 데이터 ID를 마스터 기획 DB에 대입해 기획 스펙을 빼옵니다.
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
        Image_Icon.sprite = null;
        Text_Count.gameObject.SetActive(false);
    }
}