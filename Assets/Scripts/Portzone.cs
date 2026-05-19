using UnityEngine;

public class PortZone : MonoBehaviour
{
    [Header("Port Settings")]
    [SerializeField] private string String_PortName = "기본 항구";

    // 배가 항구 영역 안으로 진입했을 때 물리 엔진이 자동으로 호출하는 정석 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[물리 체크] 항구 구역에 무언가 물리적으로 접촉함! 대상 이름: {collision.gameObject.name}");

        if (collision.CompareTag("Player") == true)
        {
            Debug.Log($"[항구 시스템] {String_PortName}에 플레이어 진입!");

            PlayerInteractionPort interactionPort = collision.GetComponent<PlayerInteractionPort>();

            if (interactionPort != null)
            {
                interactionPort.SetNearPort(true, this);
            }
        }
    }

    // 배가 항구 영역 밖으로 벗어났을 때 자동으로 호출되는 함수
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            Debug.Log($"[항구 시스템] {String_PortName}에서 플레이어 이탈.");

            PlayerInteractionPort interactionPort = collision.GetComponent<PlayerInteractionPort>();

            if (interactionPort != null)
            {
                interactionPort.SetNearPort(false, null);
            }
        }
    }

    public string GetPortName()
    {
        return String_PortName;
    }
}