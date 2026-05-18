using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 실제 인게임 플레이 화면을 총괄하는 UI 컴포넌트
public class GameUI : UIBase
{
    // 규칙: 유니티 참조 객체는 대문자 시작, SerializeField private 구조
    [Header("InGame Menu Window")]
    [SerializeField] private GameObject GameObject_Menu;

    [Header("InGame Menu Buttons")]
    [SerializeField] private UIButton Button_Continue;
    [SerializeField] private UIButton Button_NewGame;
    [SerializeField] private UIButton Button_LoadGame;
    [SerializeField] private UIButton Button_Options;
    [SerializeField] private UIButton Button_BackToMenu;
    [SerializeField] private UIButton Button_ExitGame;

    [Header("Top Menu Buttons")]
    [SerializeField] private UIButton Button_InGameMenu;

    private void Start()
    {
        BindEvents();
    }

    private void OnEnable()
    {
        BindEvents();
        SetDefaultUI();
    }


    private void BindEvents()
    {
        // 규칙: 함수는 동사로 시작할 것
        Button_Continue.BindOnClickButtonEvent(OnClick_Continue);
        Button_NewGame.BindOnClickButtonEvent(OnClick_StartNewGame);
        Button_LoadGame.BindOnClickButtonEvent(OnClick_LoadGame);
        Button_Options.BindOnClickButtonEvent(OnClick_OpenOptions);
        Button_BackToMenu.BindOnClickButtonEvent(OnClick_BackToMenu);
        Button_ExitGame.BindOnClickButtonEvent(OnClick_ExitGame);
        Button_InGameMenu.BindOnClickButtonEvent(OnClick_InGameMenu);
    }

    private void SetDefaultUI()
    {
        // 인게임 UI가 처음 켜질 때는 메뉴판이 숨겨진 채로(게임 플레이 상태) 시작
        if (GameObject_Menu != null)
        {
            GameObject_Menu.SetActive(false);
        }
    }

    //  게임 중에 ESC 키를 누르면 메뉴판이 열리고 닫히는 기능
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& GameObject_Menu != null)
        {
            ToggleMenuWindow();
        }
    }

    //  인게임 메뉴창 토글
    private void ToggleMenuWindow()
    {
        bool isMenuCurrentActive = GameObject_Menu.activeSelf;
        GameObject_Menu.SetActive(!isMenuCurrentActive);
    }

    private void OnClick_Continue()
    {
        Debug.Log("계속하기 버튼 클릭됨, - 인게임 메뉴창을 닫습니다.");

        // [핵심 안전장치] 버튼이 사라지기 전에 유니티 UI 포커스(선택 상태)를 강제로 초기화
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (GameObject_Menu != null)
        {
            GameObject_Menu.SetActive(false);
        }
    }

    private void OnClick_StartNewGame()
    {
        Debug.Log("새 게임 버튼 클릭됨");
    }

    private void OnClick_LoadGame()
    {
        Debug.Log("불러오기 버튼 클릭됨");
    }

    private void OnClick_OpenOptions()
    {
        Debug.Log("옵션 버튼 클릭됨");
    }
    private void OnClick_BackToMenu()
    {
        Debug.Log("메인 메뉴로 돌아갑니다.");
        UIManager.Inst.BackToMainMenuUI();
    }
    private void OnClick_ExitGame()
    {
        Debug.Log("게임 종료 버튼 클릭됨 - 데이터 저장 후 종료");
        GameManager.Inst.SaveAndEndGame();
    }

    private void OnClick_InGameMenu()
    {
        ToggleMenuWindow();
    }


}