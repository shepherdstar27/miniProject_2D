using TMPro;
using UnityEngine;

public class TradeUI : UIBase
{
    [Header("Trade UI 그룹")]
    [SerializeField] private Transform Transform_ShopListGroup;
    [SerializeField] private Transform Transform_InventoryGroup;
    [SerializeField] private Transform Transform_BuyCartGroup;
    [SerializeField] private Transform Transform_SellCartGroup;

    [SerializeField] private TMP_Text Text_TotalPrice;
    [SerializeField] private TMP_Text Text_CurrentGold;
    [SerializeField] private TMP_Text Text_RemainingGold;
    [SerializeField] private UIButton Button_ConfirmTrade;
    
    [SerializeField] private UIButton Button_CloseTrade;

    private void OnEnable()
    {
        BindEvents();
    }

    private void BindEvents()
    {
  
        Button_ConfirmTrade.BindOnClickButtonEvent(OnClick_ConfirmTrade);
        Button_CloseTrade.BindOnClickButtonEvent(OnClick_CloseTrade);

    }



    public void RefreshUI()
    {
        // 1. 매니저들로부터 데이터를 받아와 슬롯(SlotUI)들 동적 생성 (Addressable 또는 GameObjectManager 활용)
        // 2. 아이템 슬롯 클릭 이벤트 바인딩 시 TradeManager의 MoveItemToBuyCart 등을 호출하도록 설정
        // 3. 텍스트 정보 업데이트 (소지금, 최종 비용 반영)
    }

    private void OnClick_ConfirmTrade()
    {
        // TradeManager에 거래 확정 명령
        TradeManager.Instance.ConfirmTrade();
        RefreshUI(); // 성공 후 화면 갱신
    }

    private void OnClick_CloseTrade()
    {
        UIManager.Inst.CloseUI(UIType.TradeUI);
    }





}
