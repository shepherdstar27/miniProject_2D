using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{

    public static TradeManager Instance { get; set; }

    private TradeModel _tradeModel = new TradeModel();

    // 구매 칸으로 아이템 이동
    public void MoveItemToBuyCart(string itemId, int count)
    {
        if (_tradeModel._shopInventory.ContainsKey(itemId) && _tradeModel._shopInventory[itemId] >= count)
        {
            _tradeModel._shopInventory[itemId] -= count;

            if (_tradeModel._buyCart.ContainsKey(itemId))
                _tradeModel._buyCart[itemId] += count;
            else
                _tradeModel._buyCart.Add(itemId, count);
        }
    }

    // 거래 확정
    public void ConfirmTrade()
    {
        // 1. 총합 계산 및 플레이어 잔고 확인
        int totalCost = CalculateTotalCost();
        PlayerModel playerModel = GameManager.Inst.PlayerModel; // GameManager에서 PlayerModel 소유

        if (playerModel.Gold < totalCost)
        {
            // 잔고 부족 처리 (경고 팝업 등)
            return;
        }

        // 2. 재화 차감 및 획득
        playerModel.Gold -= totalCost;

        // 3. 인벤토리 적용 (구매 아이템 추가, 판매 아이템 제거)
        foreach (var item in _tradeModel._buyCart)
        {
            playerModel.AddItem(item.Key, item.Value); // PlayerModel에 AddItem 메서드가 있다고 가정
        }
        foreach (var item in _tradeModel._sellCart)
        {
            playerModel.RemoveItem(item.Key, item.Value);
            // 교역소 재고에 추가
            if (_tradeModel._shopInventory.ContainsKey(item.Key))
                _tradeModel._shopInventory[item.Key] += item.Value;
        }

        // 4. 카트 초기화
        _tradeModel._buyCart.Clear();
        _tradeModel._sellCart.Clear();

        // 데이터 저장 필요 시 NetworkManager 호출
        // NetworkManager.Instance.SaveTradeData(...);
    }

    private int CalculateTotalCost()
    {
        int cost = 0;
        // GameUtil.CalculateTradePrice를 활용하여 _buyCart와 _sellCart의 가격 총합 산출 (구매는 +, 판매는 -)
        return cost;
    }
}
