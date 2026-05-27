using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;       // 장착할 Inventory_SlotUI 프리팹
    [SerializeField] private Transform Prefab_ItemTooltipUI;    // 장착할 ItemTooltipUI 프리팹

    [SerializeField] private UIButton Button_CreateSlot;
    [SerializeField] private UIButton Button_CloseSelf;
    [SerializeField] private UIButton Button_CloseSelfAllArea;

    [Header("왼쪽 툴팁 UI 오브젝트 링크")]
    [SerializeField] private TextMeshProUGUI Image_BigItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemName;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemDescription;

    [Header("슬롯 리스트")]
    [SerializeField] private Transform Transform_Prefab_Slot_Root;  // Inventory_SlotUI 프리팹의 부모 위치


    //자료구조를 추가하여 아이템 추적
    private int _generatedKey = 0;
    private Dictionary<int, InventorySlotUI> _itemSlotList = new Dictionary<int, InventorySlotUI>();



    //이 UI가 열릴때, 스스로 아이템UI 기본적으로 안에 있는 모든 데이터를 불러온다
    private void OnEnable()
    {
        Button_CreateSlot.BindOnClickButtonEvent(OnClick_CreateSlotTest);
        Button_CloseSelf.BindOnClickButtonEvent(OnClick_ClosePopup);
        Button_CloseSelfAllArea.BindOnClickButtonEvent(OnClick_ClosePopup);
        SetInventoryItemSlotOnEnable();
    }

    private void SetInventoryItemSlotOnEnable()
    {
        // 슬롯 정리 - 혹시 오픈 시점에 다른 슬롯들이 있다면 제거
        if (_itemSlotList.Count > 0)
        {
            foreach (KeyValuePair<int, InventorySlotUI> slot in _itemSlotList)
            {
                DestroyImmediate(slot.Value.gameObject);
            }
            _itemSlotList.Clear();
        }

        //인벤오픈 1-1) 인벤토리가 열릴때 플레이어가 보유한 모든 아이템을 출력하는 로직을 넣어봅시다
        List<ItemModel> itemList = GameManager.Inst.GetPlayerItemList();
        if (itemList == null || itemList.Count == 0)
        {
            Debug.LogWarning("보유한 아이템이 없습니다!");
            return;
        }

        foreach (ItemModel itemModel in itemList)
        {
            CreateSlot(itemModel.ItemDataId, itemModel.ItemStackCount);
        }
    }

    private void ReadItemLIst()
    {
        GameDataManager.Instance.List_ItemTable;
    }

    public void OnClick_ClosePopup()
    {
        UIManager.Instance.CloseContentUI(UIType.Inventory);
    }


    public void OnClick_CreateSlotTest()
    {
        // CreateSlot();
    }






    //     일단 슬롯 1개 생성하고 있음
    private void CreateSlot(string dataId, int StackCount)
    {
        // 1-1 Instantiate로 Prefab_Slot 생성, 경로는 Transform_Prefab_Slot_Root
        GameObject gameObject = Instantiate(Prefab_Slot, Transform_Prefab_Slot_Root);
        if (gameObject == null) return; // 동적생성할때는 널인지 자주 확인해줘야 함

        // 1-2 자식 슬롯의 컴포넌트를 가져온다 -> 위에 게임오브젝트는 스크립트가 아직 아니므로
        // 여기서 게임 오브젝트는 동적 생성 완료
        InventorySlotUI slotComponent = gameObject.GetComponent<InventorySlotUI>();
        if (slotComponent == null) return; // 동적생성할때는 널인지 자주 확인해줘야 함

        //인벤토리이니 아이템의 dataId는 중복이 될 수 있으니 임시 키 발급
        _generatedKey++;

        // 1-3 여기서 slotComponent가지고 뭔가를 하는 겁니다!
        slotComponent.InitSlot(dataId, StackCount);
        _itemSlotList.Add(_generatedKey, slotComponent);
    }










}