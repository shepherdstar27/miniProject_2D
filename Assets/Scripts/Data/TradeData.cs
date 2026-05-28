using System;
using System.Collections.Generic;

[Serializable]
public class TradeData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 교역소 고유 ID
    public string Name;        // 교역소 이름
    public string Description; // 교역소 설명
    public string Port_Id;     //  항구 ID
    public int Gold_0001_Stock;         // 재고량
    public int Fuel_0002_Stock;          // 재고량
    public int Supplies_0003_Stock;      // 재고량
    public int TradingGoods_0004_Stock;  // 재고량  
    public int TradingGoods_0005_Stock;  // 재고량  
    public int TradingGoods_0006_Stock;  // 재고량 
    public int TradingGoods_0007_Stock;  // 재고량 
}


[Serializable]
public class TradeTable
{
    public List<TradeData> Data;
}