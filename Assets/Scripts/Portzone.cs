using UnityEngine;

public class PortZone : MonoBehaviour
{
    [Header("Port Settings")]
    // 규칙 반영: 일반 변수 _소문자 시작, 유니티 인스펙터 개방
    [SerializeField] private string _portName = "기본 항구";
    [SerializeField] private string _tradeId = "Trade_0001"; // 하이어라키에서 이 값을 교역소마다 다르게 설정

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            Debug.Log($"[항구 시스템] {_portName}에 플레이어 진입! 교역소 ID: {_tradeId}");

            PlayerInteractionPort interactionPort = collision.GetComponent<PlayerInteractionPort>();

            if (interactionPort != null)
            {
                interactionPort.SetNearPort(true, this);

                // 플레이어가 진입하는 즉시 TradeManager에 현재 상점 ID를 갱신합니다.
                if (TradeManager.Instance != null)
                {
                    TradeManager.Instance.CurrentTradeId = _tradeId;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            Debug.Log($"[항구 시스템] {_portName}에서 플레이어 이탈.");

            PlayerInteractionPort interactionPort = collision.GetComponent<PlayerInteractionPort>();

            if (interactionPort != null)
            {
                interactionPort.SetNearPort(false, null);

                if (TradeManager.Instance != null)
                {
                    TradeManager.Instance.CurrentTradeId = ""; // 이탈 시 초기화
                }
            }
        }
    }

    public string GetPortName()
    {
        return _portName;
    }
}