using UnityEngine;

// 메인 타이틀 화면의 UI 연동 및 버튼 이벤트를 관리하는 컴포넌트
public class MainUI : UIBase
{
    // 규칙: 유니티에서 참조하는 객체는 대문자 시작, SerializeField private 구조
    [SerializeField] private UIButton Button_Continue;
    [SerializeField] private UIButton Button_NewGame;
    [SerializeField] private UIButton Button_LoadGame;
    [SerializeField] private UIButton Button_Options;
    [SerializeField] private UIButton Button_ExitGame;

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
        // 규칙: 함수는 동사로 시작할 것
        // UIButton에 만들어둔 코드 기반 이벤트 바인딩 메서드 활용
        Button_Continue.BindOnClickButtonEvent(OnClick_Continue);
        Button_NewGame.BindOnClickButtonEvent(OnClick_StartNewGame);
        Button_LoadGame.BindOnClickButtonEvent(OnClick_LoadGame);
        Button_Options.BindOnClickButtonEvent(OnClick_OpenOptions);
        Button_ExitGame.BindOnClickButtonEvent(OnClick_ExitGame);
    }

    private void OnClick_Continue()
    {
        Debug.Log("계속하기 버튼 클릭됨");
        // TODO: 이어하기 로직 연결
    }

    private void OnClick_StartNewGame()
    {
        Debug.Log("새 게임 버튼 클릭됨 - UI 전환 요청");
        // 규칙: UI 관리는 UIManager를 통하며, 확장 메서드(Extension)를 활용해 전환
        UIManager.Inst.StartNewGameUI();
    }

    private void OnClick_LoadGame()
    {
        Debug.Log("불러오기 버튼 클릭됨");
    }

    private void OnClick_OpenOptions()
    {
        Debug.Log("옵션 버튼 클릭됨");
    }

    private void OnClick_ExitGame()
    {
        Debug.Log("게임 종료 버튼 클릭됨 - 데이터 저장 후 종료");
        // 규칙: 게임의 전체적인 흐름 및 데이터 저장은 GameManager를 통함
        GameManager.Inst.SaveAndEndGame();
    }
}