using UnityEngine;

public class GameQuit : MonoBehaviour
{
    // 버튼 클릭 이벤트에 바인딩되거나 외부에서 호출될 게임 종료 함수 (동사 시작)
    public void QuitGame()
    {
        // 1. 유니티 에디터에서 플레이 중일 때 멈춤
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        // 2. 실제 빌드된 독립 실행형 게임 종료
        Application.Quit();
    }
}
