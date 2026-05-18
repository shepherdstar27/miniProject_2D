using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button Button_Base;

    private Action _onClickCallback;

    private void Awake()
    {
        InitButtonComponent();
    }



    private void InitButtonComponent()
    {
        if (Button_Base == null)
        {
            Button_Base = GetComponentInChildren<Button>();
        }
    }

    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (onClickCallback == null)
        {
            Debug.LogError($"[{gameObject.name}] 등록하려는 클릭 액션(Callback)이 null입니다!");
            return;
        }

        // ⭕ 부모의 Awake/OnEnable 타이밍 분리를 위해 여기서 한 번 더 컴포넌트 체크를 보장합니다.
        InitButtonComponent();

        if (Button_Base == null)
        {
            Debug.LogError($"[{gameObject.name}] 내부의 유니티 순정 Button 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        _onClickCallback = onClickCallback;

        Button_Base.onClick.RemoveAllListeners();
        Button_Base.onClick.AddListener(OnClickButton);

        Debug.Log($"[{gameObject.name}] 버튼에 클릭 이벤트 등록 성공");
    }

    // 유니티 버튼 시스템에 의해 직접 호출되는 일반 메서드
    private void OnClickButton()
    {
        // [로그 2] 물리적인 클릭 신호가 유니티 엔진을 거쳐 여기까지 도달했는지 확인하는 핵심 길목
        Debug.Log($"[UIButton 피드백] 물리적으로 [{gameObject.name}] 버튼이 클릭됨! 보관된 액션을 실행합니다.");

        // 3. 보관해 두었던 실제 로직을 실행합니다.
        _onClickCallback?.Invoke();
    }

}