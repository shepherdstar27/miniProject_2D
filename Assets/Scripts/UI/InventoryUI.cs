using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UIManager
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;       // 장착할 Inventory_SlotUI 프리팹
    [SerializeField] private Transform Prefab_ItemTooltipUI;    // 장착할 ItemTooltipUI 프리팹

    [Header("왼쪽 툴팁 UI 오브젝트 링크")]
    [SerializeField] private TextMeshProUGUI Image_BigItemIcon;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemName;
    [SerializeField] private TextMeshProUGUI TextMesh_ItemDescription;

    [Header("슬롯 리스트")]
    [SerializeField] private Transform Transform_Prefab_Slot_Root;  // Inventory_SlotUI 프리팹의 부모 위치


}