using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    // 슬롯 UI들이 원격으로 데이터를 쏠 수 있도록 싱글톤 구조 유지
    public static ItemTooltipUI Instance { get; set; }

    [Header("Tooltip Panel Group")]
    [SerializeField] private GameObject GameObject_TooltipPanel; // 껐다 켤 고정 패널 부모

    [Header("Tooltip Info Elements")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private TextMeshProUGUI Text_ItemName;
    [SerializeField] private TextMeshProUGUI Text_ItemDescription;
    [SerializeField] private TextMeshProUGUI Text_ItemType;
    [SerializeField] private TextMeshProUGUI Text_ItemGrade;

    // 매니저가 켜질 때 자신을 등록하고 창을 숨기는 초기화 과정 (필수)
    private void Awake()
    {
        InitInstance();
    }

    private void InitInstance()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(ItemData itemData)
    {
        if (itemData == null)
        {
            return;
        }

        // 1. 텍스트 데이터 안전하게 주입
        if (Text_ItemName != null)
        {
            Text_ItemName.text = itemData.Name;
        }
        if (Text_ItemDescription != null)
        {
            Text_ItemDescription.text = itemData.Description;
        }
        if (Text_ItemType != null)
        {
            Text_ItemType.text = itemData.ItemType;
        }
        if (Text_ItemGrade != null)
        {
            Text_ItemGrade.text = itemData.Grade;
        }

        // 2. 아이콘 이미지 안전하게 주입
        if (Image_ItemIcon != null)
        {
            Sprite loadedIcon = Resources.Load<Sprite>(itemData.IconPath);
            if (loadedIcon != null)
            {
                Image_ItemIcon.sprite = loadedIcon;
            }
        }


        // 3. 데이터 조립이 끝나면 좌측 패널을 켭니다.
        GameObject_TooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        if (GameObject_TooltipPanel != null)
        {
            GameObject_TooltipPanel.SetActive(false);
        }
    }
}