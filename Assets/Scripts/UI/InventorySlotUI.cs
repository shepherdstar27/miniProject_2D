using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour
{ 
    [Header("기본 정보")]
    [SerializeField] private TextMeshProUGUI TextMesh_Item_Image;    // 아이템 이미지
    [SerializeField] private TextMeshProUGUI TextMesh_Item_StackCount;    // 아이템 수량
    [SerializeField] private GameObject TextMesh_Item_Selected;    // 아이템 선택됨 이미지, 왜 GameObject인가? 활성/비활성화 기능으로만 사용

    private int _generatedKey; // 슬롯이 자기가 살아있는동안 어떤 슬롯인지 보관,  클릭했을때 위에서 Id를 통해서 부모에게 전달해줘야 하므로 임시보관을 하는 것임
    private string _slotDataID; // 슬롯이 자기가 살아있는동안 어떤 슬롯인지 보관,  클릭했을때 위에서 Id를 통해서 부모에게 전달해줘야 하므로 임시보관을 하는 것임
    private int _itemStackCount; // 슬롯이 자기가 살아있는동안 어떤 슬롯인지 보관,  클릭했을때 위에서 Id를 통해서 부모에게 전달해줘야 하므로 임시보관을 하는 것임

    private event Action<int> OnSelectEvent;

    public int SlotInstanceId { get; private set; }


    private void OnDisable()
    {
        OnSelectEvent = null;
    }

    //부모(InventoryUI) 에서 사용해야 하니 public으로 작성
    public void InitSlot(int generatedKey, string dataId, int itemStackCount) // 카테고리에 따라 다른 데이터를 받아올 수 있도록 파라미터를 추가 하면 됨
    {
        SlotInstanceId = generatedKey;
        _slotDataID = dataId;
        _itemStackCount= itemStackCount;
    }

    public void OnClick_SelectItem()
    {
        // 부모한테 알려주자
        OnSelectEvent?.Invoke(SlotInstanceId);


        Debug.Log($"{SlotInstanceId}눌러졌다");
        // 나중에 툴팁, 팝업 다 여기서 띄워주면 된다
    }

}