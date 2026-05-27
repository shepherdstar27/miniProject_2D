using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler
{
    [Header("Slot Display Graphics")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_StackCount;

    [Header("Live Assigned Slot Asset")]
    [SerializeField] private string String_CurrentSlotItemId = "";
    [SerializeField] private int Int_CurrentCount = 0;




    //  [슬롯 갱신 공정]: 아이콘 파일 경로를 찾아서 실시간 스프라이트 자원을 로드 매핑합니다.
    public void SetupSlotDetails(string itemId, int count)
    {

        String_CurrentSlotItemId = itemId;
        Int_CurrentCount = count;

        if (string.IsNullOrEmpty(itemId) == true || count <= 0)
        {
            ClearSlotGraphic();
            return;
        }

        // 아이템 마스터 테이블을 가동해 JSON 내 IconPath 주소를 서칭합니다.
        if (GameDataManager.Instance != null)
        {
            ItemData itemMaster = GameDataManager.Instance.GetItemData(itemId);
            if (itemMaster != null && Image_ItemIcon != null)
            {
                Sprite loadedSprite = Resources.Load<Sprite>(itemMaster.IconPath);
                if (loadedSprite != null)
                {
                    Image_ItemIcon.sprite = loadedSprite;
                    Image_ItemIcon.enabled = true; // 아이콘 활성화
                }
            }
        }

        // 개수 인쇄
        if (TextMesh_StackCount != null)
        {
            TextMesh_StackCount.text = count.ToString();
            TextMesh_StackCount.enabled = true;
        }
    }

    public void ClearSlotGraphic()
    {
        String_CurrentSlotItemId = "";
        Int_CurrentCount = 0;

        if (Image_ItemIcon != null)
        {
            Image_ItemIcon.sprite = null;
            Image_ItemIcon.enabled = false; // 빈 슬롯은 유령화 처리
        }
        if (TextMesh_StackCount != null)
        {
            TextMesh_StackCount.enabled = false;
        }
    }

    //  [인터페이스 요청 구현 1]: 마우스 커서가 본 슬롯 영역 안에 포개어 안착했을 때의 트리거
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 빈 슬롯 상태이면 툴팁 정보창 연산을 가동하지 않습니다.
        if (string.IsNullOrEmpty(String_CurrentSlotItemId) == true) return;

        // 중앙 UI 컨트롤러 허브를 거쳐 좌측 정보창 렌더러 함수를 록온 격발 호출합니다.
        if (InventoryUI.Instance != null && InventoryUI.Instance.GetTooltipUI() != null)
        {
            InventoryUI.Instance.GetTooltipUI().RenderItemTooltip(String_CurrentSlotItemId);
        }
    }


}