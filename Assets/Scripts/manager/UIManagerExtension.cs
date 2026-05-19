using UnityEngine;
public static class UIManagerExtension
{
    public static void ShowStartupUIOnGameStart(this UIManager uiManager)
    {
        // 처음 실행 시 메인 화면 로비 오픈
        uiManager.OpenUI(UIRootType.MainUI, UIType.MainUI);
    }

    // 새 게임 버튼을 눌렀을 때 메인 UI를 닫고 게임 캔버스를 켜는 로직 확장
    public static void StartNewGameUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIRootType.MainUI, UIType.MainUI);
        uiManager.OpenUI(UIRootType.MainUI, UIType.GameUI);
    }
    public static void BackToMainMenuUI(this UIManager uiManager)
    {
        // 1. 현재 떠 있는 플레이 화면(GameUI)을 닫고
        uiManager.CloseUI(UIRootType.MainUI, UIType.GameUI);

        // 2. 최초 메인 화면(MainUI) 프리팹을 다시 동적 생성하여 오픈
        uiManager.OpenUI(UIRootType.MainUI, UIType.MainUI);
    }

    public static void OpenPortUI(this UIManager uiManager)
    {
        //  PortUI 열기
        uiManager.OpenUI(UIRootType.ContentUI, UIType.PortUI);
    }

    public static void ClosePortUI(this UIManager uiManager)
    {
        uiManager.CloseUI(UIRootType.ContentUI, UIType.PortUI);

        // PortUI 닫기
        PlayerInteractionPort playerInteraction = Object.FindFirstObjectByType<PlayerInteractionPort>();
        if (playerInteraction != null)
        {
            playerInteraction.ExitPortAnchor();
        }
    }

    public static void OpenInventoryUI(this UIManager manager)
    {
        // PopupUI 또는 ContentUI 레이어에 InventoryUI 프리팹을 찍어냅니다.
        manager.OpenUI(UIRootType.PopupUI, UIType.InventoryUI);
    }

    // 인벤토리 UI 완전 파괴 및 닫기 함수
    public static void CloseInventoryUI(this UIManager manager)
    {
        manager.CloseUI(UIRootType.PopupUI, UIType.InventoryUI);
    }


}