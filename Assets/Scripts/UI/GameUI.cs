using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 실제 인게임 플레이 화면을 총괄하는 UI 컴포넌트
public class GameUI : UIBase
{
    // 규칙: 유니티 참조 객체는 대문자 시작, SerializeField private 구조

    [Header("Bottom Menu Buttons")]
    [SerializeField] private UIButton Button_Inventory;


    private void OnEnable()
    {
        BindEvents();
    }

    private void BindEvents()
    {
        // 규칙: 함수는 동사로 시작할 것

        Button_Inventory.BindOnClickButtonEvent(OnClick_Inventory);

    }



    //  인게임 메뉴창 토글


    private void OnClick_Inventory()
    {
        Debug.Log("[GameUI] 인벤토리 버튼 클릭");

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


}