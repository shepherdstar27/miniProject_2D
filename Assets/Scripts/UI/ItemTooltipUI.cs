using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    [Header("Tooltip Panel Frame View")]
    [SerializeField] private GameObject GameObject_TooltipMasterFrame; // 좌측 정보창 총괄 캔버스 덩어리 오브젝트

    [Header("Tooltip Detail Renderer Elements")]
    [SerializeField] private Image Image_BigItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemName;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemDescription;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemPrice;



    //  슬롯에서 호출 시 프레임을 켜고 JSON 정보를 추출 매핑합니다.
    public void RenderItemTooltip(string itemId)
    {
        if (GameDataManager.Instance == null || GameObject_TooltipMasterFrame == null) return;

        ItemData itemMaster = GameDataManager.Instance.GetItemData(itemId);
        if (itemMaster == null) return;

        // 1. 텍스트 명세 수치 동기화
        if (TextMesh_ItemName != null) TextMesh_ItemName.text = itemMaster.Name;
        if (TextMesh_ItemDescription != null) TextMesh_ItemDescription.text = itemMaster.Description;
        if (TextMesh_ItemPrice != null) TextMesh_ItemPrice.text = itemMaster.Price;


        // 2. 외형 리소스 실시간 동적 할당
        if (Image_BigItemIcon != null)
        {
            Sprite loadedSprite = Resources.Load<Sprite>(itemMaster.IconPath);
            if (loadedSprite != null)
            {
                Image_BigItemIcon.sprite = loadedSprite;
                Image_BigItemIcon.enabled = true;
            }
            else
            {
                Image_BigItemIcon.enabled = false;
            }
        }

        // 3. 최종 완성된 정보 보드 뷰 레이어 해제 및 출력
        GameObject_TooltipMasterFrame.SetActive(true);
    }


}