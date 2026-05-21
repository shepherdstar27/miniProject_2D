using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Equipped Item Data IDs")]
    [SerializeField] private string String_EquippedShipId = "Ship_Player_0001";
    [SerializeField] private string String_EquippedEngineId = "Engine_Player_0001";
    [SerializeField] private string String_EquippedWeaponId = "Weapon_Canon_0002";

    private void Start()
    {
        LoadAndDistributeEquipment();
    }

    // [핵심 공정]: JSON 데이터를 교차 검색하여 각 스크립트로 분배하는 함수
    private void LoadAndDistributeEquipment()
    {
        if (GameDataManager.Instance == null) return;

        // 1. 배(Ship) 데이터 로드 및 체력 컴포넌트 배달
        ShipData shipMaster = GameDataManager.Instance.GetShipData(String_EquippedShipId);
        if (shipMaster != null)
        {
            HealthController healthScript = GetComponent<HealthController>();
            if (healthScript != null)
            {
                healthScript.SetupMaxHp(shipMaster.Max_HP);
            }
        }

        // 2. 엔진(Engine) 데이터 로드 및 이동 컴포넌트 배달
        EngineData engineMaster = GameDataManager.Instance.GetEngineData(String_EquippedEngineId);
        if (engineMaster != null)
        {
            PlayerMove moveScript = GetComponent<PlayerMove>();
            if (moveScript != null)
            {
                moveScript.SetupEngineSpecs(engineMaster.MoveSpeed, engineMaster.RotateSpeed, engineMaster.Acceleration);
            }
        }

        // 3. 무기(Weapon) 데이터 로드 및 공격 컴포넌트 배달
        WeaponData weaponMaster = GameDataManager.Instance.GetWeaponData(String_EquippedWeaponId);
        if (weaponMaster != null)
        {
            PlayerAttack attackScript = GetComponent<PlayerAttack>();
            if (attackScript != null)
            {
                attackScript.SetupWeaponSpecs(String_EquippedWeaponId, weaponMaster.FireCooldown);
            }
        }

        Debug.Log("[장비 관리자] 플레이어 세팅 완료. 배/엔진/무기 제원 배달 공정이 정상 종료되었습니다.");
    }
}