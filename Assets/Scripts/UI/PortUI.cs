using UnityEngine;

public class PortUI : UIBase
{
    [Header("Port UI Buttons")]
    [SerializeField] private UIButton Button_Story; 
    [SerializeField] private UIButton Button_Inventory;
    [SerializeField] private UIButton Button_Crew;
    [SerializeField] private UIButton Button_Trade;
    [SerializeField] private UIButton Button_pier;

    [Header("Port UI Button_pier")]
    [SerializeField] private UIButton Button_ClosePort;
    // 추가 계속

    [Header("Port UI Menu Window")]
    [SerializeField] private GameObject PortUI_pier_Menu;



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
        Button_Story.BindOnClickButtonEvent(OnClick_Story);
        Button_Inventory.BindOnClickButtonEvent(OnClick_Inventory);
        Button_Crew.BindOnClickButtonEvent(OnClick_Crew);
        Button_Trade.BindOnClickButtonEvent(OnClick_Trade);
        Button_pier.BindOnClickButtonEvent(OnClick_pier);

        Button_ClosePort.BindOnClickButtonEvent(OnClick_ClosePort);

    }

    private void OnClick_Story()
    {
        Debug.Log("[항구 UI] 스토리 버튼 클릭됨");
    }
    private void OnClick_Inventory()
    {
        Debug.Log("[항구 UI] 스토리 버튼 클릭됨");
    }
    private void OnClick_Crew()
    {
        Debug.Log("[항구 UI] 승무원 버튼 클릭됨");
    }
    private void OnClick_Trade()
    {
        Debug.Log("[항구 UI] 교역소 버튼 클릭됨");
    }
    private void OnClick_pier()
    {
        Debug.Log("[항구 UI] 선착장 버튼 클릭됨");
        Toggle_pier_Window();
    }

    private void OnClick_ClosePort()
    {
        Debug.Log("[항구 UI] 출항하기 버튼 클릭됨 - UI 매니저에게 정박 UI 폐쇄를 요청합니다.");

        // 동적 생성 방식 규칙: 매니저에게 나를 파괴해달라고 요청합니다.
        UIManager.Inst.ClosePortUI();
    }

    private void Toggle_pier_Window()
    {
        bool isMenuCurrentActive = PortUI_pier_Menu.activeSelf;
        PortUI_pier_Menu.SetActive(!isMenuCurrentActive);
    }





}