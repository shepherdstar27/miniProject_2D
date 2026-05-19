using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UIBase
{
    [SerializeField] private Inventory TargetInventory;
    [SerializeField] private Transform SlotGridContent;
    [SerializeField] private GameObject SlotPrefab;

    [Header("Inventory Close Button")]
    [SerializeField] private UIButton Button_Close; //  창을 닫기 위한 닫기 버튼 변수 추가

    private List<InventorySlotUI> _uiSlots = new List<InventorySlotUI>(); 
    private void Start()
    {
        BindEvents();
        InitUISlots();
        UpdateInventoryUI();
    }

    private void OnEnable()
    {
        BindEvents();
        UpdateInventoryUI(); // 창이 켜질 때마다 최신 실시간 가방 데이터 동기화
    }

    private void BindEvents()
    {
        if (Button_Close != null)
        {
            Button_Close.BindOnClickButtonEvent(OnClick_CloseInventory);
        }
    }

    private void OnClick_CloseInventory()
    {
        Debug.Log("[인벤토리 UI] 닫기 버튼 클릭됨 - UI 매니저에게 메모리 파괴를 요청합니다.");

        // UI 매니저 확장 메서드를 불러서 자신을 완전히 파괴(Destroy)하게 만듭니다.
        UIManager.Inst.CloseInventoryUI();
    }

    private void InitUISlots()
    {
        foreach (Transform child in SlotGridContent)
        {
            Destroy(child.gameObject);
        }

        _uiSlots.Clear();

        int maxSlots = 20;
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject go = Instantiate(SlotPrefab, SlotGridContent);
            InventorySlotUI uiSlot = go.GetComponent<InventorySlotUI>();
            if (uiSlot != null)
            {
                _uiSlots.Add(uiSlot);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        if (TargetInventory == null) return;

        List<InventorySlot> realDataSlots = TargetInventory.GetSlots();

        for (int i = 0; i < _uiSlots.Count; i++)
        {
            if (i < realDataSlots.Count)
            {
                _uiSlots[i].SetupSlot(realDataSlots[i]);
            }
            else
            {
                _uiSlots[i].ClearSlot();
            }
        }
    }


}