using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    // [직렬화 필드]: 인스펙터 창에서 버튼의 구성 요소를 직접 지정할 수 있도록 선언
    [SerializeField] private Button Button_Base;   // 실제 클릭 기능을 담당하는 유니티 기본 버튼 컴포넌트
    [SerializeField] private Text Text_Base;       // 버튼 위에 표시될 글자
    [SerializeField] private Image Image_Base;     // 버튼의 기본 배경 이미지
    [SerializeField] private Image Image_Select;   // 버튼이 선택되었을 때만 켜질 이미지

    private void Awake()
    {
        // 1-2) 이 오브젝트가 생성될 때, 한번 컴포넌트를 찾아서 캐싱하자

        InitUIButton();   // 게임 시작 시 버튼 컴포넌트를 자동으로 찾아서 연결
        SetDefaultUI();   // 처음 시작할 때는 선택 이미지를 꺼둠
    }

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때마다 버튼 클릭 시 '선택 UI를 켜고 끄는 기능'을 자동으로 연결
        BindOnClickButtonEvent(OnClickSetSelectUI);
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화되면 메모리 누수 방지를 위해 모든 클릭 이벤트 연결을 해제
        Button_Base.onClick.RemoveAllListeners();
    }

    private void SetDefaultUI()
    {
        // 선택 상태 표시용 이미지가 있다면 초기화 시 꺼줌
        if (Image_Select != null)
        {
            Image_Select.gameObject.SetActive(false);
        }
    }

    private void InitUIButton()
    {
        // 이미 버튼이 할당되어 있다면 함수 종료
        if (Button_Base != null)
        {
            return;
        }

        // 버튼이 할당되지 않았다면 자기 자신이나 자식 중에서 Button 컴포넌트를 찾아 자동으로 할당
        var button = this.gameObject.GetComponentInChildren<Button>();
        if (button != null)
        {
            this.Button_Base = button;
        }
    }

    // 버튼 클릭 시 수행할 함수(Action)를 외부에서 받아 등록하는 함수
    public void BindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Base == null) return;
        // 유니티 이벤트 시스템에 클릭 함수를 리스너로 추가
        Button_Base.onClick.AddListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    // 버튼 클릭 이벤트에서 특정 함수 연결을 해제하는 함수
    public void UnBindOnClickButtonEvent(Action onClickCallback)
    {
        if (Button_Base == null) return;
        // 유니티 이벤트 시스템에서 특정 함수 연결을 제거
        Button_Base.onClick.RemoveListener(new UnityEngine.Events.UnityAction(onClickCallback));
    }

    // 버튼의 텍스트를 코드 상에서 즉시 변경하고 싶을 때 사용하는 함수
    public void ChangeButtonText(string buttonStr)
    {
        if (Text_Base == null) return; // 텍스트 컴포넌트가 없으면 무시
        Text_Base.text = buttonStr;    // 글자 변경
    }

    // 버튼을 클릭했을 때 호출되는 함수
    private void OnClickSetSelectUI()
    {
        if (Image_Select != null)
        {
            // 현재 선택 이미지의 상태(켜짐/꺼짐)를 파악
            bool currentActive = Image_Select.gameObject.activeSelf;
            // 현재 상태와 반대로 바꿈 (켜져있으면 끄고, 꺼져있으면 켬)
            Image_Select.gameObject.SetActive(!currentActive);
        }
    }
}