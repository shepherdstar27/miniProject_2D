using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeUI : UIBase
{
    private static TradeUI _instance;
    public static TradeUI Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    [Header("Trade UI 그룹")]
    [SerializeField] private Transform Transform_ShopListGroup;
    [SerializeField] private Transform Transform_InventoryGroup;
    [SerializeField] private Transform Transform_BuyCartGroup;
    [SerializeField] private Transform Transform_SellCartGroup;

    [Header("Trade Texts")]
    [SerializeField] private TMP_Text Text_TotalPrice;
    [SerializeField] private TMP_Text Text_CurrentGold;
    [SerializeField] private TMP_Text Text_RemainingGold;

    [Header("Inventory Resource Texts")]
    [SerializeField] private TextMeshProUGUI TextMesh_GoldAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_FuelAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_SuppliesAmount;

    [SerializeField] private UIButton Button_ConfirmTrade;
    [SerializeField] private UIButton Button_CloseTrade;

    [Header("Slot Prefab")]
    [SerializeField] private GameObject GameObject_SlotUiPrefab;

    private List<GameObject> _createdSlots = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        BindEvents();

        if (TradeManager.Instance != null)
        {
            // PortZone에서 넘겨준 동적인 현재 교역소 ID를 사용해 재고를 로드
            TradeManager.Instance.SetupShopInventory(TradeManager.Instance.CurrentTradeId);
        }

        RefreshUI();
    }

    private void BindEvents()
    {
        if (Button_ConfirmTrade != null)
            Button_ConfirmTrade.BindOnClickButtonEvent(OnClick_ConfirmTrade);

        if (Button_CloseTrade != null)
            Button_CloseTrade.BindOnClickButtonEvent(OnClick_CloseTrade);
    }

    public void RefreshUI()
    {
        if (TradeManager.Instance == null || GameManager.Inst == null) return;

        ClearAllSlots();

        TradeModel tradeModel = TradeManager.Instance.GetTradeModel();
        PlayerModel playerModel = GameManager.Inst.PlayerModel;

        // 1. 교역소 판매 목록 (좌측)
        foreach (KeyValuePair<string, int> item in tradeModel.ShopInventory)
        {
            CreateSlot(Transform_ShopListGroup, item.Key, item.Value, TradeSlotType.Shop);
        }

        if (playerModel.CargoSlots != null)
        {
            // 핵심 시각적 차감 로직: 인벤토리의 같은 아이템들을 합산한 뒤 장바구니에 올린 갯수를 뺍니다.
            Dictionary<string, int> inventorySummary = new Dictionary<string, int>();

            foreach (InventorySlotData slot in playerModel.CargoSlots)
            {
                if (string.IsNullOrEmpty(slot.ItemId) == false && slot.Count > 0)
                {
                    if (inventorySummary.ContainsKey(slot.ItemId))
                    {
                        inventorySummary[slot.ItemId] += slot.Count;
                    }
                    else
                    {
                        inventorySummary.Add(slot.ItemId, slot.Count);
                    }
                }
            }

            foreach (KeyValuePair<string, int> invItem in inventorySummary)
            {
                // 장바구니에 올라가 있는 갯수
                int alreadyInCart = tradeModel.SellCart.ContainsKey(invItem.Key) ? tradeModel.SellCart[invItem.Key] : 0;

                // 화면에 보여줄 갯수 = 내 총 보유량 - 장바구니에 올린 갯수
                int displayCount = invItem.Value - alreadyInCart;

                if (displayCount > 0)
                {
                    CreateSlot(Transform_InventoryGroup, invItem.Key, displayCount, TradeSlotType.PlayerInventory);
                }
            }
        }

        // 3. 구매 카트
        foreach (KeyValuePair<string, int> item in tradeModel.BuyCart)
        {
            CreateSlot(Transform_BuyCartGroup, item.Key, item.Value, TradeSlotType.BuyCart);
        }

        // 4. 판매 카트
        foreach (KeyValuePair<string, int> item in tradeModel.SellCart)
        {
            CreateSlot(Transform_SellCartGroup, item.Key, item.Value, TradeSlotType.SellCart);
        }

        // 5. 가격 정보 갱신 및 특수재화 표기
        int currentGold = playerModel.Gold;
        int currentFuel = playerModel.Fuel;         // 모델에서 연료량 가져오기
        int currentSupplies = playerModel.Supplies; // 모델에서 보급품량 가져오기

        int totalCost = TradeManager.Instance.CalculateTotalCost();
        int remainingGold = currentGold - totalCost; // 판매액이 더 크면 총 비용이 마이너스이므로 잔금이 늘어남

        // 거래 영수증(좌측 하단) 텍스트 갱신
        if (Text_CurrentGold != null) Text_CurrentGold.text = currentGold.ToString();
        if (Text_TotalPrice != null) Text_TotalPrice.text = totalCost.ToString();
        if (Text_RemainingGold != null) Text_RemainingGold.text = remainingGold.ToString();

        //  우측 내 인벤토리 패널의 특수 재화 텍스트 갱신
        if (TextMesh_GoldAmount != null) TextMesh_GoldAmount.text = currentGold.ToString();
        if (TextMesh_FuelAmount != null) TextMesh_FuelAmount.text = currentFuel.ToString();
        if (TextMesh_SuppliesAmount != null) TextMesh_SuppliesAmount.text = currentSupplies.ToString();
    }

    private void CreateSlot(Transform parent, string itemId, int count, TradeSlotType slotType)
    {
        if (GameObject_SlotUiPrefab == null) return;

        GameObject slotObj = Instantiate(GameObject_SlotUiPrefab, parent);
        _createdSlots.Add(slotObj);

        InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
        if (slotUI != null)
        {
            slotUI.SetupSlotDetails(itemId, count);
        }

        // 방금 만든 슬롯에 클릭 핸들러 부착
        TradeSlotClickHandler clickHandler = slotObj.AddComponent<TradeSlotClickHandler>();
        clickHandler.ItemId = itemId;
        clickHandler.SlotType = slotType;
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < _createdSlots.Count; i++)
        {
            if (_createdSlots[i] != null) Destroy(_createdSlots[i]);
        }
        _createdSlots.Clear();
    }

    // 클릭 핸들러가 찔러주는 게이트웨이 함수
    public void HandleSlotClick(TradeSlotType slotType, string itemId)
    {
        if (TradeManager.Instance == null) return;

        int tradeAmount = 1; // 1회 클릭 시 1개 이동

        if (slotType == TradeSlotType.Shop)
        {
            TradeManager.Instance.MoveItemToBuyCart(itemId, tradeAmount);
        }
        else if (slotType == TradeSlotType.PlayerInventory)
        {
            TradeManager.Instance.MoveItemToSellCart(itemId, tradeAmount);
        }
        else if (slotType == TradeSlotType.BuyCart)
        {
            TradeManager.Instance.CancelBuyItem(itemId, tradeAmount);
        }
        else if (slotType == TradeSlotType.SellCart)
        {
            TradeManager.Instance.CancelSellItem(itemId, tradeAmount);
        }

        RefreshUI(); // 이동 후 화면 다시 그리기
    }

    private void OnClick_ConfirmTrade()
    {
        if (TradeManager.Instance != null)
        {
            TradeManager.Instance.ConfirmTrade();
            RefreshUI();
        }
        else
        {
            Debug.LogError("[에러] TradeManager가 씬에 존재하지 않아 거래를 확정할 수 없습니다.");
        }
    }

    private void OnClick_CloseTrade()
    {
        if (TradeManager.Instance != null)
        {
            TradeManager.Instance.CancelAllTrade(); // 닫을 때 장바구니에 올린 물건 복구
        }
        UIManager.Inst.CloseUI(UIType.TradeUI);
    }
}