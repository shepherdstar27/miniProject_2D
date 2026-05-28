using System.Collections.Generic;

public class TradeModel
{
    private Dictionary<string, int> _shopInventory = new Dictionary<string, int>();
    private Dictionary<string, int> _buyCart = new Dictionary<string, int>();
    private Dictionary<string, int> _sellCart = new Dictionary<string, int>();

    public Dictionary<string, int> ShopInventory
    {
        get { return _shopInventory; }
        set { _shopInventory = value; }
    }
    public Dictionary<string, int> BuyCart
    {
        get { return _buyCart; }
        set { _buyCart = value; }
    }
    public Dictionary<string, int> SellCart
    {
        get { return _sellCart; }
        set { _sellCart = value; }
    }
}