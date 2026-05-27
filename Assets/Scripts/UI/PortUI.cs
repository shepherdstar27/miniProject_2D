using UnityEngine;
using UnityEngine.EventSystems;

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

    [Header("Port UI Menu Window")]
    [SerializeField] private GameObject PortUI_pier_Menu;





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
        Debug.Log("[항구 UI] 인벤토리 버튼 클릭, UI 매니저에게 동적 생성 요청");

        Toggle_Inventory_Window();
    }
    private void Toggle_Inventory_Window()
    {
        // 버튼 클릭 후 유니티 UI 포커스 강제 해제 (안전장치 유지)
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        // UIManager를 통해 현재 인벤토리가 열려있는지 확인하여 분기 처리
        if (UIManager.Inst.IsOpened(UIType.InventoryUI) == true)
        {
            Debug.Log("[항구 UI] 인벤토리가 이미 열려있으므로, UIManager에게 폐쇄");
            UIManager.Inst.CloseUI(UIType.InventoryUI);
        }
        else
        {
            Debug.Log("[항구 UI] 인벤토리가 닫혀있으므로, UIManager에게 동적 생성");
            UIManager.Inst.OpenInventoryUI(); // (또는 UIManager.Inst.OpenUI(UIRootType.PopupUI, UIType.InventoryUI);)
        }
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
    private void Toggle_pier_Window()
    {
        bool isMenuCurrentActive = PortUI_pier_Menu.activeSelf;
        PortUI_pier_Menu.SetActive(!isMenuCurrentActive);
    }

    private void OnClick_ClosePort()
    {
        Debug.Log("[항구 UI] 출항하기 버튼 클릭됨 - UI 매니저에게 정박 UI 폐쇄를 요청합니다.");

        // 동적 생성 방식 규칙: 매니저에게 나를 파괴해달라고 요청합니다.
        UIManager.Inst.ClosePortUI();
    }






}