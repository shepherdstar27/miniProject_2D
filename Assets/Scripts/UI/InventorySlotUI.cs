using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour
{ 
    [Header("기본 정보")]
    [SerializeField] private TextMeshProUGUI TextMesh_Item_Image;    // 아이템 이미지
    [SerializeField] private TextMeshProUGUI TextMesh_Item_Number;    // 아이템 수량
    [SerializeField] private TextMeshProUGUI TextMesh_Item_Selected;    // 아이템 선택됨 이미지

}