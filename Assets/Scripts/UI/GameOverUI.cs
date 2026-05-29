using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameOverUI : UIBase
{
    [Header("GameOver Window")]
    [SerializeField] private GameObject GameOver_Window;

    [Header("Button")]
    [SerializeField] private UIButton Button_GameOver;


    private void OnEnable()
    {
        BindEvents();
    }

    private void BindEvents()
    {
        // 규칙: 함수는 동사로 시작
        Button_GameOver.BindOnClickButtonEvent(OnClick_BackToMenu);
    }

    private void OnClick_BackToMenu()
    {

        Debug.Log("메인 메뉴로 돌아갑니다.");

        if (GameDataManager.Instance != null && GameDataManager.Instance.PlayerData != null)
        {
            GameDataManager.Instance.PlayerData.ResetToDefault();
        }


        UIManager.Inst.CloseUI(UIType.GameOverUI);
        UIManager.Inst.BackToMainMenuUI();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

}
