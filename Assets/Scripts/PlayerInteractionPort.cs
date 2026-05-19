using UnityEngine;

public class PlayerInteractionPort : MonoBehaviour
{
    private PlayerMove m_playerMove;
    private Rigidbody2D m_rigidbody2D;

    private bool m_isNearPort = false;
    private PortZone m_currentPortZone = null;
    private bool m_isAnchored = false;

    private void Awake()
    {
        InitComponents();
    }

    private void InitComponents()
    {
        m_playerMove = GetComponent<PlayerMove>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 정박 상태에 따른 입력 처리 분기
        if (m_isAnchored == true)
        {
            HandlePortExitInput();
        }
        else
        {
            HandlePortInteractionInput();
        }
    }

    // PortZone 스크립트가 호출하는 공용 메서드
    public void SetNearPort(bool isNear, PortZone portZone)
    {
        m_isNearPort = isNear;
        m_currentPortZone = portZone;

        if (m_isNearPort == true)
        {
            ShowPortGuideHUD();
        }
        else
        {
            HidePortGuideHUD();
        }
    }

    private void ShowPortGuideHUD()
    {
        if (m_currentPortZone != null)
        {
            Debug.Log($"[HUD 가이드] 'E' 키를 눌러 {m_currentPortZone.GetPortName()}에 정박하세요.");
        }
    }

    private void HidePortGuideHUD()
    {
        Debug.Log("[HUD 가이드] 항구 안내문 닫힘.");
    }

    private void HandlePortInteractionInput()
    {
        if (m_isNearPort == true && Input.GetKeyDown(KeyCode.E) == true)
        {
            AnchorToPort();
        }
    }

    private void AnchorToPort()
    {
        m_isAnchored = true;

        //  타 스크립트(PlayerMove)의 상태를 안전하게 제어
        if (m_playerMove != null)
        {
            m_playerMove.StopShipMoveAndResetGear();
            m_playerMove.SetControlActive(false); // 정박 중엔 이동 입력 잠금
        }

        // 물리 속도 강제 제로화
        if (m_rigidbody2D != null)
        {
            m_rigidbody2D.linearVelocity = Vector2.zero;
            m_rigidbody2D.angularVelocity = 0f;
        }

        if (m_currentPortZone != null)
        {
            Debug.Log($"[항구 정박] {m_currentPortZone.GetPortName()}에 안전하게 정박했습니다. 항구 UI를 오픈합니다.");
        }

        UIManager.Inst.OpenPortUI();
    }

    private void HandlePortExitInput()
    {
        // 정박 중 다시 E키를 누르면 매니저에게 UI 파괴를 요청합니다.
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            UIManager.Inst.ClosePortUI();
        }
    }
    public void ExitPortAnchor()
    {
        m_isAnchored = false;
        Debug.Log("[항구 출항] 정박이 정상 해제되었습니다. 다시 자유로운 항해가 가능합니다.");

        if (m_playerMove != null)
        {
            m_playerMove.SetControlActive(true);
        }
    }
}