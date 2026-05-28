using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    private string _currentTradeId = "";
    public string CurrentTradeId
    {
        get { return _currentTradeId; }
        set { _currentTradeId = value; }
    }

    private static TradeManager _instance;
    public static TradeManager Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private TradeModel _tradeModel = new TradeModel();

    public TradeModel GetTradeModel()
    {
        return _tradeModel;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 1. 판매대 -> 구매 카트
    public void MoveItemToBuyCart(string itemId, int count)
    {
        if (_tradeModel.ShopInventory.ContainsKey(itemId) && _tradeModel.ShopInventory[itemId] >= count)
        {
            _tradeModel.ShopInventory[itemId] -= count;
            if (_tradeModel.BuyCart.ContainsKey(itemId))
                _tradeModel.BuyCart[itemId] += count;
            else
                _tradeModel.BuyCart.Add(itemId, count);
        }
    }

    // 2. 인벤토리 -> 판매 카트
    public void MoveItemToSellCart(string itemId, int count)
    {
        int playerHasAmount = GetPlayerItemAmount(itemId);
        int alreadyInCart = _tradeModel.SellCart.ContainsKey(itemId) ? _tradeModel.SellCart[itemId] : 0;

        // [해결 2] 클릭 시도 시 데이터의 흐름을 콘솔에 명확히 찍어줍니다.
        Debug.Log($"[판매 시도] 아이템: {itemId} / 내 보유량: {playerHasAmount} / 현재 카트: {alreadyInCart} / 추가: {count}");

        if (playerHasAmount >= alreadyInCart + count)
        {
            if (_tradeModel.SellCart.ContainsKey(itemId))
            {
                _tradeModel.SellCart[itemId] += count;
            }
            else
            {
                _tradeModel.SellCart.Add(itemId, count);
            }
            Debug.Log($"[판매 성공] {itemId}가 판매 카트에 담겼습니다. (카트 내 총 {alreadyInCart + count}개)");
        }
        else
        {
            Debug.LogWarning($"[판매 실패] {itemId}의 수량이 부족하여 카트에 담을 수 없습니다.");
        }
    }

    // 3. 구매 카트 취소
    public void CancelBuyItem(string itemId, int count)
    {
        if (_tradeModel.BuyCart.ContainsKey(itemId) && _tradeModel.BuyCart[itemId] >= count)
        {
            _tradeModel.BuyCart[itemId] -= count;
            if (_tradeModel.BuyCart[itemId] <= 0) _tradeModel.BuyCart.Remove(itemId);
            _tradeModel.ShopInventory[itemId] += count; // 재고 원상복구
        }
    }

    // 4. 판매 카트 취소
    public void CancelSellItem(string itemId, int count)
    {
        if (_tradeModel.SellCart.ContainsKey(itemId) && _tradeModel.SellCart[itemId] >= count)
        {
            _tradeModel.SellCart[itemId] -= count;
            if (_tradeModel.SellCart[itemId] <= 0) _tradeModel.SellCart.Remove(itemId);
        }
    }

    public void CancelAllTrade()
    {
        _tradeModel.BuyCart.Clear();
        _tradeModel.SellCart.Clear();
        // 상점 초기 데이터로 재고 리셋 함수 필요 시 추가
    }

    public void ConfirmTrade()
    {
        int totalCost = CalculateTotalCost();
        PlayerModel playerModel = GameManager.Inst.PlayerModel;

        if (playerModel.Gold < totalCost)
        {
            Debug.LogWarning("소지금이 부족하여 교역 확정이 불가합니다.");
            return;
        }

        playerModel.Gold -= totalCost;

        foreach (KeyValuePair<string, int> item in _tradeModel.BuyCart)
        {
            playerModel.AddItem(item.Key, item.Value);
        }

        foreach (KeyValuePair<string, int> item in _tradeModel.SellCart)
        {
            playerModel.RemoveItem(item.Key, item.Value);
            if (_tradeModel.ShopInventory.ContainsKey(item.Key) == false)
                _tradeModel.ShopInventory.Add(item.Key, 0);
            _tradeModel.ShopInventory[item.Key] += item.Value;
        }

        _tradeModel.BuyCart.Clear();
        _tradeModel.SellCart.Clear();

        GameManager.Inst.SaveData(); // 거래 내역 세이브
    }

    public int CalculateTotalCost()
    {
        int cost = 0;

        if (GameDataManager.Instance == null)
        {
            return 0;
        }

        // 1. 구매 카트 아이템 비용 합산 (플레이어의 돈이 줄어드는 것이므로 + 비용)
        foreach (KeyValuePair<string, int> item in _tradeModel.BuyCart)
        {
            ItemData itemData = GameDataManager.Instance.GetItemData(item.Key);
            if (itemData != null)
            {
                string cleanPrice = "";
                if (string.IsNullOrEmpty(itemData.Price) == false)
                {
                    cleanPrice = itemData.Price.Trim();
                }

                int basePrice = 0;
                if (int.TryParse(cleanPrice, out basePrice) == true)
                {
                    // 배율이 적용된 개당 최종 가격 산출
                    int finalPrice = GetItemTradePrice(item.Key, basePrice);
                    // 개수를 곱하여 누산
                    cost += (finalPrice * item.Value);
                }
            }
        }

        // 2. 판매 카트 아이템 비용 차감 (플레이어가 돈을 버는 것이므로 - 비용)
        foreach (KeyValuePair<string, int> item in _tradeModel.SellCart)
        {
            ItemData itemData = GameDataManager.Instance.GetItemData(item.Key);
            if (itemData != null)
            {
                string cleanPrice = "";
                if (string.IsNullOrEmpty(itemData.Price) == false)
                {
                    cleanPrice = itemData.Price.Trim();
                }

                int basePrice = 0;
                if (int.TryParse(cleanPrice, out basePrice) == true)
                {
                    int finalPrice = GetItemTradePrice(item.Key, basePrice);
                    cost -= (finalPrice * item.Value);
                }
            }
        }

        return cost;
    }

    private int GetPlayerItemAmount(string itemId)
    {
        int total = 0;
        if (GameManager.Inst != null && GameManager.Inst.PlayerModel != null)
        {
            foreach (InventorySlotData slot in GameManager.Inst.PlayerModel.CargoSlots)
            {
                if (slot.ItemId == itemId) total += slot.Count;
            }
        }
        return total;
    }

    public void SetupShopInventory(string tradeId)
    {
        TradeModel tradeModel = GetTradeModel();
        tradeModel.ShopInventory.Clear();
        tradeModel.BuyCart.Clear();
        tradeModel.SellCart.Clear();

        if (GameDataManager.Instance != null)
        {
            TradeData tradeData = GameDataManager.Instance.GetTradeData(tradeId);

            if (tradeData != null)
            {
                // 변수명에 _Stock 반영
                if (tradeData.Gold_0001_Stock > 0) tradeModel.ShopInventory.Add("Gold_0001", tradeData.Gold_0001_Stock);
                if (tradeData.Fuel_0002_Stock > 0) tradeModel.ShopInventory.Add("Fuel_0002", tradeData.Fuel_0002_Stock);
                if (tradeData.Supplies_0003_Stock > 0) tradeModel.ShopInventory.Add("Supplies_0003", tradeData.Supplies_0003_Stock);
                if (tradeData.TradingGoods_0004_Stock > 0) tradeModel.ShopInventory.Add("TradingGoods_0004", tradeData.TradingGoods_0004_Stock);
                if (tradeData.TradingGoods_0005_Stock > 0) tradeModel.ShopInventory.Add("TradingGoods_0005", tradeData.TradingGoods_0005_Stock);
                if (tradeData.TradingGoods_0006_Stock > 0) tradeModel.ShopInventory.Add("TradingGoods_0006", tradeData.TradingGoods_0006_Stock);
                if (tradeData.TradingGoods_0007_Stock > 0) tradeModel.ShopInventory.Add("TradingGoods_0007", tradeData.TradingGoods_0007_Stock);

                Debug.Log($"[상점 초기화] {tradeId} 교역소의 판매 물품이 {tradeModel.ShopInventory.Count}종류 진열되었습니다.");
            }
            else
            {
                Debug.LogWarning($"[상점 에러] ID가 {tradeId}인 교역소 JSON 데이터를 찾을 수 없습니다.");
            }
        }
    }

    public int GetItemTradePrice(string itemId, int basePrice)
    {
        int ratePercentage = 100;

        if (GameDataManager.Instance != null && string.IsNullOrEmpty(_currentTradeId) == false)
        {
            BuyAndSellData rateData = GameDataManager.Instance.GetBuyAndSellDataByTradeId(_currentTradeId);

            if (rateData != null)
            {
                // 변수명에 _Multiplier 반영
                if (itemId == "Gold_0001") ratePercentage = rateData.Gold_0001_Multiplier;
                else if (itemId == "Fuel_0002") ratePercentage = rateData.Fuel_0002_Multiplier;
                else if (itemId == "Supplies_0003") ratePercentage = rateData.Supplies_0003_Multiplier;
                else if (itemId == "TradingGoods_0004") ratePercentage = rateData.TradingGoods_0004_Multiplier;
                else if (itemId == "TradingGoods_0005") ratePercentage = rateData.TradingGoods_0005_Multiplier;
                else if (itemId == "TradingGoods_0006") ratePercentage = rateData.TradingGoods_0006_Multiplier;
                else if (itemId == "TradingGoods_0007") ratePercentage = rateData.TradingGoods_0007_Multiplier;
            }
        }

        return TradeUtil.CalculateTradePrice(basePrice, ratePercentage);
    }
}