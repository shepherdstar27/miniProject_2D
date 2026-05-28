using UnityEngine;

public class TradeUtil
{
    // 기본 가격과 JSON에 정의된 배율을 곱해 최종 거래 가격을 반환

    public static int CalculateTradePrice(int basePrice, int ratePercentage)
    {
        // 예: 기본가 100 * 배율 200(%) / 100 = 200
        return basePrice * ratePercentage / 100;
    }
}
