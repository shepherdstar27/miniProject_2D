using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image m_iconImage;
    [SerializeField] private Text m_countText;

    // 슬롯에 데이터를 주입하는 함수
    public void SetupSlot(InventorySlot slotData)
    {
        if (slotData == null || slotData.itemID == 0)
        {
            ClearSlot();
            return;
        }

        // 데이터 드리븐의 핵심: ID를 통해 원본 DB(GameDataManager)에서 기획 정보를 조회해옵니다.
        ItemData originData = GameDataManager.Instance.GetItemData(slotData.itemID);

        if (originData != null)
        {
            // Resources.Load를 통해 에셋 폴더 내의 아이콘 이미지를 동적으로 불러와 할당합니다.
            Sprite icon = Resources.Load<Sprite>(originData.IconPath);
            if (icon != null) m_iconImage.sprite = icon;

            // 수량 표시 (1개 이상 겹쳐있을 때만 수량 텍스트 활성화)
            m_countText.text = slotData.itemCount.ToString();
            m_countText.gameObject.SetActive(slotData.itemCount > 1);
        }
    }

    public void ClearSlot()
    {
        m_iconImage.sprite = null;
        m_countText.gameObject.SetActive(false);
    }
}