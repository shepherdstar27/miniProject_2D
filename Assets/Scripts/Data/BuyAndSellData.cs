using System;
using System.Collections.Generic;

[Serializable]
public class BuyAndSellData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 고유 ID
    public string Name;        // 이름
    public string Trade_Id;        // 소속교역소
    public int Gold_0001_Multiplier;         // 가격배율
    public int Fuel_0002_Multiplier;          // 가격배율
    public int Supplies_0003_Multiplier;      // 가격배율
    public int TradingGoods_0004_Multiplier;  // 가격배율  
    public int TradingGoods_0005_Multiplier;  // 가격배율  
    public int TradingGoods_0006_Multiplier;  // 가격배율 
    public int TradingGoods_0007_Multiplier;  // 가격배율 
}

[Serializable]
public class BuyAndSellTable
{
    public List<BuyAndSellData> Data;
}
