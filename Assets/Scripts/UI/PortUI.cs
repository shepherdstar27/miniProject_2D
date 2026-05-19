using UnityEngine;

public class PortUI : UIBase
{
    [Header("Port UI Buttons")]
    [SerializeField] private UIButton Button_ShipRepair; // 배 수리 버튼
    [SerializeField] private UIButton Button_ClosePort;  // 출항하기(닫기) 버튼

    private void Start()
    {
        BindEvents();
    }

    private void OnEnable()
    {
        BindEvents();
    }

    private void BindEvents()
    {
        Button_ShipRepair.BindOnClickButtonEvent(OnClick_ShipRepair);
        Button_ClosePort.BindOnClickButtonEvent(OnClick_ClosePort);
    }

    private void OnClick_ShipRepair()
    {
        Debug.Log("[항구 UI] 배를 수리합니다. (골드 차감 로직 등 확장 가능)");
    }

    private void OnClick_ClosePort()
    {
        Debug.Log("[항구 UI] 출항하기 버튼 클릭됨 - UI 매니저에게 정박 UI 폐쇄를 요청합니다.");

        // 동적 생성 방식 규칙: 매니저에게 나를 파괴해달라고 요청합니다.
        UIManager.Inst.ClosePortUI();
    }
}