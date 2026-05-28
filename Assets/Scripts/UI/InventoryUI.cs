using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : UIBase
{
    // 람다식 배제 및 고전적인 get/set 블록 사용
    private static InventoryUI _instance;
    public static InventoryUI Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    [Header("UI Resource References")]
    [SerializeField] private GameObject GameObject_SlotUiPrefab;       // 장착할 InventorySlotUI 프리팹
    [SerializeField] private Transform Transform_SlotGridContainer;    // LayoutGroup 컴포넌트가 붙은 그리드 부모 트랜스폼

    [Header("Left Tooltip UI Object Link")]
    [SerializeField] private ItemTooltipUI Component_LeftTooltipUI;    // 좌측 아이템 상세 정보창 컴포넌트 주소

    [Header("Special Resource Texts")]
    [SerializeField] private TextMeshProUGUI TextMesh_GoldAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_FuelAmount;
    [SerializeField] private TextMeshProUGUI TextMesh_SuppliesAmount;

    [Header("Close Buttons")]
    [SerializeField] private UIButton Button_Close;

    // 규칙 반영: 일반 멤버 변수는 _소문자로 시작
    private List<InventorySlotUI> _createdSlotScripts = new List<InventorySlotUI>();

    private void Awake()
    {
        Instance = this;
        BindEvents();
    }

    private void OnEnable()
    {
        // GameManager 내의 PlayerModel 원본 데이터를 직접 참조하여 동기화
        if (GameManager.Inst != null && GameManager.Inst.PlayerModel != null)
        {
            PlayerModel playerModel = GameManager.Inst.PlayerModel;
            List<InventorySlotData> data = playerModel.CargoSlots;

            // 데이터가 없으면 무시, 있으면 생성 및 갱신
            if (data != null && data.Count > 0)
            {
                if (_createdSlotScripts.Count == 0)
                {
                    CreateUIContainerSlots(data.Count);
                }
                RefreshInventoryDisplay(data);
            }

            // 재화 역시 PlayerModel에서 직접 가져와 갱신
            UpdateSpecialResourceText(playerModel.Gold, playerModel.Fuel, playerModel.Supplies);
        }
    }

    public int GetCreatedSlotCount()
    {
        return _createdSlotScripts.Count;
    }

    private void BindEvents()
    {
        if (Button_Close != null)
        {
            Button_Close.BindOnClickButtonEvent(OnClick_Close);
        }
    }

    private void OnClick_Close()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Debug.Log("[인벤토리 UI] UIManager에게 정식 폐쇄를 요청합니다.");

        // UIManager의 CloseUI를 호출합니다.
        UIManager.Inst.CloseUI(UIType.InventoryUI);
    }

    // 좌측 툴팁에 손쉽게 교차 접근을 허용하기 위한 중간 외교 게이트웨이 함수
    public ItemTooltipUI GetTooltipUI()
    {
        return Component_LeftTooltipUI;
    }

    // [슬롯 실물 작도]: 배 스펙 칸 수에 맞추어 격자 화면 UI 기물을 실시간 생성합니다.
    public void CreateUIContainerSlots(int totalCount)
    {
        // 기존에 잔존하던 레이아웃 박스 요소 일제 청소 루프
        for (int i = 0; i < _createdSlotScripts.Count; i++)
        {
            if (_createdSlotScripts[i] != null)
            {
                Destroy(_createdSlotScripts[i].gameObject);
            }
        }
        _createdSlotScripts.Clear();

        // 새로운 슬롯 인스턴스 일제 생성 주입
        for (int i = 0; i < totalCount; i++)
        {
            GameObject spawnSlotObj = Instantiate(GameObject_SlotUiPrefab, Transform_SlotGridContainer);
            InventorySlotUI slotScript = spawnSlotObj.GetComponent<InventorySlotUI>();

            if (slotScript != null)
            {
                _createdSlotScripts.Add(slotScript);
                slotScript.ClearSlotGraphic(); // 순정 공백 정돈
            }
        }
    }

    // [화면 새로고침 동기화 공정]: 아이템 습득 시 백엔드 리스트를 토스받아 그래픽을 한 번에 정렬합니다.
    public void RefreshInventoryDisplay(List<InventorySlotData> backendSlots)
    {
        for (int i = 0; i < _createdSlotScripts.Count; i++)
        {
            if (i < backendSlots.Count)
            {
                // 이전 단계에서 구조체 프로퍼티명이 ItemId, Count로 변경되었음을 반영
                _createdSlotScripts[i].SetupSlotDetails(backendSlots[i].ItemId, backendSlots[i].Count);
            }
        }
    }

    // [특수 재화 실시간 수치 인쇄부]
    public void UpdateSpecialResourceText(int gold, int fuel, int supplies)
    {
        if (TextMesh_GoldAmount != null) TextMesh_GoldAmount.text = gold.ToString();
        if (TextMesh_FuelAmount != null) TextMesh_FuelAmount.text = fuel.ToString();
        if (TextMesh_SuppliesAmount != null) TextMesh_SuppliesAmount.text = supplies.ToString();
    }
}