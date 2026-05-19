using UnityEngine;
using TMPro;

public class DropdownButtonController : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        // 스크립트에서 이벤트를 리스너로 등록하는 방법
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownClicked);
        }
    }

    // 드롭다운의 선택지가 클릭될 때 실행되는 함수 (버튼의 onClick 역할)
    public void OnDropdownClicked(int index)
    {
        // index는 0부터 시작합니다 (Option A = 0, Option B = 1, Option C = 2)
        switch (index)
        {
            case 0:
                Debug.Log("Option A 버튼이 클릭되었습니다.");
                // 여기에 A 버튼 기능 구현
                break;
            case 1:
                Debug.Log("Option B 버튼이 클릭되었습니다.");
                // 여기에 B 버튼 기능 구현
                break;
            case 2:
                Debug.Log("Option C 버튼이 클릭되었습니다.");
                // 여기에 C 버튼 기능 구현
                break;
        }
    }
}