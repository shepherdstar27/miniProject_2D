using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; set; }

    [Header("Master Tables")]
    [SerializeField] private List<ItemData> List_ItemTable = new List<ItemData>();
    [SerializeField] private List<WeaponData> List_WeaponTable = new List<WeaponData>(); // 무기 테이블 그릇 추가
    // [SerializeField] private List<PortData> List_PortTable = new List<PortData>();       // 추후 확장용

    private void Awake()
    {
        InitSingleton();
        LoadAllMasterTables();
    }

    private void InitSingleton()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadAllMasterTables()
    {
        //---  ItemData 로드 로직 ---

        // Resources 폴더에서 기획자가 작성한 Json 데이터를 긁어옵니다.
        string itemJsonPath = "JsonOutput/Item";
        TextAsset itemTextAsset = Resources.Load<TextAsset>(itemJsonPath);

        if (itemTextAsset != null)
        {
            //  [핵심 해결책] 유니티가 배열 [ ] 을 읽지 못하는 병을 고치기 위해, 
            // 텍스트 앞뒤로 {"Data": 와 } 를 강제로 붙여서 객체 포맷으로 둔갑시킵니다.
            string wrappedJson = "{ \"Data\": " + itemTextAsset.text + " }";

            // JsonUtility는 루트가 배열([ ])이면 파싱하지 못하므로, 유저님이 선언해둔 ItemTable 컨테이너 클래스를 활용합니다.
            ItemTable parsedTable = JsonUtility.FromJson<ItemTable>(itemTextAsset.text);
            if (parsedTable != null && parsedTable.Data != null)
            {
                List_ItemTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_ItemTable.Count}개의 아이템 기획 데이터를 로드했습니다.");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {itemJsonPath} 경로에서 Json 데이터를 찾지 못했습니다!");
        }

        //---  WeaponData 로드 로직 ---

        string weaponJsonPath = "JsonOutput/Weapon";
        TextAsset weaponTextAsset = Resources.Load<TextAsset>(weaponJsonPath);

        if (weaponTextAsset != null)
        {
            string cleanJson = weaponTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";
            WeaponTable parsedTable = JsonUtility.FromJson<WeaponTable>(wrappedJson);
            if (parsedTable != null && parsedTable.Data != null)
            {
                List_WeaponTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_WeaponTable.Count}개의 무기 기획 데이터를 로드했습니다.");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {weaponJsonPath} 경로에서 Json 데이터를 찾지 못했습니다!");
        }
    }




    // 몬스터나 항구 정보도 여기에 동일한 규격으로 Resources.Load 함수들을 늘려가시면 데이터 드리븐이 완성됩니다.


    public ItemData GetItemData(string targetId)
    {
        foreach (ItemData item in List_ItemTable)
        {
            if (item.Id == targetId)
            {
                return item;
            }
        }

        Debug.LogWarning($"[GameDataManager] Id [{targetId}] 에 해당하는 아이템 정보가 기획 데이터에 없습니다!");
        return null;
    }

    public WeaponData GetWeaponData(string targetId)
    {
        foreach (WeaponData weapon in List_WeaponTable)
        {
            if (weapon.Id == targetId)
            {
                return weapon;
            }
        }

        Debug.LogWarning($"[GameDataManager] 무기 Id [{targetId}] 에 해당하는 정보가 기획 데이터에 없습니다!");
        return null;
    }
}