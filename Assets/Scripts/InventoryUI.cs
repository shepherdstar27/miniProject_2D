using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory m_targetInventory; // 연결할 가방 정보
    [SerializeField] private Transform m_slotGridContent; // ScrollView의 Content(LayoutGroup 부착된 곳)
    [SerializeField] private GameObject m_slotPrefab;      // UI 슬롯 칸 프리팹

    private List<InventorySlotUI> m_uiSlots = new List<InventorySlotUI>();

    void Start()
    {
        InitUISlots();
        UpdateInventoryUI();
    }

    // 최대 슬롯 개수만큼 화면에 빈 그릇(프리팹)들을 채워둡니다 (안정적인 uGUI 정렬용)
    private void InitUISlots()
    {
        // 기존 컨텐트 자식들 청소
        foreach (Transform child in m_slotGridContent) Destroy(child.gameObject);

        int maxSlots = 20; // 가방 크기 규격만큼 미리 생성
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject go = Instantiate(m_slotPrefab, m_slotGridContent);
            InventorySlotUI uiSlot = go.GetComponent<InventorySlotUI>();
            m_uiSlots.Add(uiSlot);
        }
    }

    // 가방 데이터의 실시간 현황을 UI 슬롯 기물들에 동기화(업데이트)하는 함수
    public void UpdateInventoryUI()
    {
        List<InventorySlot> realDataSlots = m_targetInventory.GetSlots();

        for (int i = 0; i < m_uiSlots.Count; i++)
        {
            if (i < realDataSlots.Count)
            {
                m_uiSlots[i].SetupSlot(realDataSlots[i]);
            }
            else
            {
                m_uiSlots[i].ClearSlot(); // 데이터가 없는 칸은 빈 슬롯 처리
            }
        }
    }
}