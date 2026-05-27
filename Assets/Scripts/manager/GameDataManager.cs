using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; set; }

    [Header("Master Tables")]
    [SerializeField] private List<ItemData> List_ItemTable = new List<ItemData>();// 아이템 테이블 그릇 추가
    [SerializeField] private List<WeaponData> List_WeaponTable = new List<WeaponData>(); // 무기 테이블 그릇 추가
    [SerializeField] private List<EnemyData> List_EnemyTable = new List<EnemyData>(); // 적 테이블 그릇 추가
    [SerializeField] private List<EngineData> List_EngineTable = new List<EngineData>(); // 엔진 테이블 그릇 추가
    [SerializeField] private List<ShipData> List_ShipTable = new List<ShipData>(); // 배 테이블 그릇 추가
    [SerializeField] private List<DropTableData> List_DropTable = new List<DropTableData>(); // 드랍테이블 테이블 그릇 추가




    // [SerializeField] private List<PortData> List_PortTable = new List<PortData>();       // 추후 확장용

    private void Awake()
    {
        InitSingleton();
        LoadAllMasterTables();
    }

    private void InitSingleton()
    {
        Instance = this;

        // [ DontDestroyOnLoad 경고 해결 ]
        // 만약 이 오브젝트가 다른 오브젝트의 자식으로 등록되어 있다면 최상위 부모를 파괴 불가로 만듭니다.
        if (transform.parent != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void LoadAllMasterTables()
    {
        // =========================================================================
        // ItemData 로드 로직
        // =========================================================================
        // Resources 폴더에서 기획자가 작성한 Json 데이터를 긁어옵니다.
        string itemJsonPath = "JsonOutput/Item";
        TextAsset itemTextAsset = Resources.Load<TextAsset>(itemJsonPath);

        if (itemTextAsset != null)
        {
            //  [핵심 해결책] 유니티가 배열 [ ] 을 읽지 못하는 병을 고치기 위해, 
            // 텍스트 앞뒤로 {"Data": 와 } 를 강제로 붙여서 객체 포맷으로 둔갑시킵니다.
            string cleanJson = itemTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            // JsonUtility는 루트가 배열([ ])이면 파싱하지 못하므로, 유저님이 선언해둔 ItemTable 컨테이너 클래스를 활용합니다.
            ItemTable parsedTable = JsonUtility.FromJson<ItemTable>(wrappedJson);
            if (parsedTable != null && parsedTable.Data != null)
            {
                List_ItemTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_ItemTable.Count}개의 아이템 기획 데이터를 로드했습니다.");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {itemJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }

        // =========================================================================
        // WeaponData 로드 로직
        // =========================================================================
        string weaponJsonPath = "JsonOutput/Weapon";
        TextAsset weaponTextAsset = Resources.Load<TextAsset>(weaponJsonPath);

        if (weaponTextAsset != null)
        {
            //  [핵심 해결] 앞뒤 공백을 자르고, 중괄호 객체 형태로 강제 변환(래핑)합니다.
            string cleanJson = weaponTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            //  weaponTable 규격으로 FromJson을 실행

            WeaponTable parsedTable = JsonUtility.FromJson<WeaponTable>(wrappedJson);

            if (parsedTable != null && parsedTable.Data != null)
            {
                List_WeaponTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_WeaponTable.Count}개의 무기 기획 데이터를 성공적으로 로드했습니다!");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {weaponJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }

        // =========================================================================
        // EnemyData 로드 로직
        // =========================================================================
        string EnemyJsonPath = "JsonOutput/Enemy";
        TextAsset EnemyTextAsset = Resources.Load<TextAsset>(EnemyJsonPath);

        if (EnemyTextAsset != null)
        {
            //  [핵심 해결] 앞뒤 공백을 자르고, 중괄호 객체 형태로 강제 변환(래핑)합니다.
            string cleanJson = EnemyTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            //  EnemyTable 규격으로 FromJson을 실행
            EnemyTable parsedTable = JsonUtility.FromJson<EnemyTable>(wrappedJson);

            if (parsedTable != null && parsedTable.Data != null)
            {
                List_EnemyTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_EnemyTable.Count}개의 무기 기획 데이터를 성공적으로 로드했습니다!");
            }
        }
        else
        {   
        Debug.LogError($"[GameDataManager] {EnemyJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }

        // =========================================================================
        // EngineData 로드 로직
        // =========================================================================
        string EngineJsonPath = "JsonOutput/Engine";
        TextAsset EngineTextAsset = Resources.Load<TextAsset>(EngineJsonPath);

        if (EngineTextAsset != null)
        {
            //  [핵심 해결] 앞뒤 공백을 자르고, 중괄호 객체 형태로 강제 변환(래핑)합니다.
            string cleanJson = EngineTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            //  EngineTable 규격으로 FromJson을 실행
            EngineTable parsedTable = JsonUtility.FromJson<EngineTable>(wrappedJson);

            if (parsedTable != null && parsedTable.Data != null)
            {
                List_EngineTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_EngineTable.Count}개의 무기 기획 데이터를 성공적으로 로드했습니다!");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {EngineJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }

        // =========================================================================
        // ShipData 로드 로직
        // =========================================================================
        string ShipJsonPath = "JsonOutput/Ship";
        TextAsset ShipTextAsset = Resources.Load<TextAsset>(ShipJsonPath);

        if (ShipTextAsset != null)
        {
            //  [핵심 해결] 앞뒤 공백을 자르고, 중괄호 객체 형태로 강제 변환(래핑)합니다.
            string cleanJson = ShipTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            //  ShipTable 규격으로 FromJson을 실행
            ShipTable parsedTable = JsonUtility.FromJson<ShipTable>(wrappedJson);

            if (parsedTable != null && parsedTable.Data != null)
            {
                List_ShipTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_ShipTable.Count}개의 배 기획 데이터를 성공적으로 로드했습니다!");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {ShipJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }


        // =========================================================================
        // DropTableData 로드 로직
        // =========================================================================
        string DropTableJsonPath = "JsonOutput/DropTable";
        TextAsset DropTableTextAsset = Resources.Load<TextAsset>(DropTableJsonPath);

        if (DropTableTextAsset != null)
        {
            //  [핵심 해결] 앞뒤 공백을 자르고, 중괄호 객체 형태로 강제 변환(래핑)합니다.
            string cleanJson = DropTableTextAsset.text.Trim();
            string wrappedJson = "{ \"Data\": " + cleanJson + " }";

            //  DropTable 규격으로 FromJson을 실행
            DropTable parsedTable = JsonUtility.FromJson<DropTable>(wrappedJson);

            if (parsedTable != null && parsedTable.Data != null)
            {
                List_DropTable = parsedTable.Data;
                Debug.Log($"[GameDataManager] {List_DropTable.Count}개의 배 기획 데이터를 성공적으로 로드했습니다!");
            }
        }
        else
        {
            Debug.LogError($"[GameDataManager] {ShipJsonPath} 경로에서 데이터를 찾지 못했습니다.");
        }


    }


    // =========================================================================
    // 조회 메서드 세트 
    // =========================================================================

    // =========================================================================
    // ItemData 조회 메서드 
    // =========================================================================
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

    // =========================================================================
    // WeaponData 조회 메서드 
    // =========================================================================
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

    // =========================================================================
    // EnemyData 조회 메서드 
    // =========================================================================

    public EnemyData GetEnemyData(string targetId)
    {
        foreach (EnemyData Enemy in List_EnemyTable)
        {
            if (Enemy.Id == targetId)
            {
                return Enemy;
            }
        }
        Debug.LogWarning($"[GameDataManager] 적 Id [{targetId}] 에 해당하는 정보가 기획 데이터에 없습니다!");
        return null;
    }


    // =========================================================================
    // EngineData 조회 메서드 
    // =========================================================================
    public EngineData GetEngineData(string targetId)
    {
        foreach (EngineData Engine in List_EngineTable)
        {
            if (Engine.Id == targetId)
            {
                return Engine;
            }
        }
        Debug.LogWarning($"[GameDataManager] 엔진 Id [{targetId}] 에 해당하는 정보가 기획 데이터에 없습니다!");
        return null;
    }


    // =========================================================================
    // ShipData 조회 메서드 
    // =========================================================================
    public ShipData GetShipData(string targetId)
    {
        foreach (ShipData Ship in List_ShipTable)
        {
            if (Ship.Id == targetId)
            {
                return Ship;
            }
        }
        Debug.LogWarning($"[GameDataManager] 배 Id [{targetId}] 에 해당하는 정보가 기획 데이터에 없습니다!");
        return null;
    }

    // =========================================================================
    // DropTableData 조회 메서드 
    // =========================================================================
    public DropTableData GetDropItemData(string targetId)
    {
        foreach (DropTableData DropTable in List_DropTable)
        {
            if (DropTable.Id == targetId)
            {
                return DropTable;
            }
        }
        Debug.LogWarning($"[GameDataManager] 드랍테이블 Id [{targetId}] 에 해당하는 정보가 기획 데이터에 없습니다!");
        return null;
    }




}