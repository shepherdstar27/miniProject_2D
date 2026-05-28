using System;
using System.Collections.Generic;

[Serializable]
public class BuyAndSellData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 고유 ID
    public string Name;        // 이름
    public string Trade_Id;        // 소속교역소
    public int Gold_0001;         // 가격배율
    public int Fuel_0002;          // 가격배율
    public int Supplies_0003;      // 가격배율
    public int TradingGoods_0004;  // 가격배율  
    public int TradingGoods_0005;  // 가격배율  
    public int TradingGoods_0006;  // 가격배율 
    public int TradingGoods_0007;  // 가격배율 
}

[Serializable]
public class BuyAndSellTable
{
    public List<BuyAndSellData> Data;
}
