using System.Collections.Generic;
using UnityEngine;

public class TradeModel : MonoBehaviour
{
    // 교역소의 현재 재고
    public Dictionary<string, int> _shopInventory { get; private set; } = new Dictionary<string, int>();



    // 구매를 위해 올린 아이템 목록 (ItemId, 갯수)
    public Dictionary<string, int> _buyCart { get; private set; } = new Dictionary<string, int>();



    // 판매를 위해 올린 아이템 목록
    public Dictionary<string, int> _sellCart { get; private set; } = new Dictionary<string, int>();

    public void InitializeShop(List<TradeInventory> initialData)
    {
        _shopInventory.Clear();
        foreach (var data in initialData)
        {
            _shopInventory.Add(data.ItemId, data.InitialStock);
        }
    }
}
