using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private void Start()
    {
        LoadAndDistributeEquipment();
    }

    // [핵심 공정]: PlayerModel의 장비 데이터를 교차 검색하여 각 스크립트로 분배하는 함수
    private void LoadAndDistributeEquipment()
    {
        if (GameDataManager.Instance == null) return;
        if (GameManager.Inst == null || GameManager.Inst.PlayerModel == null) return;

        PlayerModel playerModel = GameManager.Inst.PlayerModel;

        // 1. 배(Ship) 데이터 로드 및 체력/화물창 컴포넌트 배달
        ShipData shipMaster = GameDataManager.Instance.GetShipData(playerModel.EquippedShipId);
        if (shipMaster != null)
        {
            HealthController healthScript = GetComponent<HealthController>();
            if (healthScript != null)
            {
                healthScript.SetupMaxHp(shipMaster.Max_HP);
            }

            // [안전 초기화]: 세이브된 화물창 데이터가 비어있을 경우에만 배 스펙에 맞춰 새로 칸을 생성합니다.
            if (playerModel.CargoSlots == null || playerModel.CargoSlots.Count == 0)
            {
                playerModel.InitializeCargo(shipMaster.Cargo);
                Debug.Log($"[장비 관리자] 새로운 화물창이 {shipMaster.Cargo}칸으로 초기화되었습니다.");
            }
        }

        // 2. 엔진(Engine) 데이터 로드 및 이동 컴포넌트 배달
        EngineData engineMaster = GameDataManager.Instance.GetEngineData(playerModel.EquippedEngineId);
        if (engineMaster != null)
        {
            PlayerMove moveScript = GetComponent<PlayerMove>();
            if (moveScript != null)
            {
                moveScript.SetupEngineSpecs(playerModel.EquippedEngineId, engineMaster.MoveSpeed, engineMaster.RotateSpeed, engineMaster.Acceleration);
            }
        }

        // 3. 무기(Weapon) 데이터 로드 및 공격 컴포넌트 배달
        WeaponData weaponMaster = GameDataManager.Instance.GetWeaponData(playerModel.EquippedWeaponId);
        if (weaponMaster != null)
        {
            PlayerAttack attackScript = GetComponent<PlayerAttack>();
            if (attackScript != null)
            {
                attackScript.SetupWeaponSpecs(playerModel.EquippedWeaponId, weaponMaster.FireCoolDown, weaponMaster.FireRange);
            }
        }

        Debug.Log("[장비 관리자] 플레이어 세팅 완료. 배/엔진/무기 제원 배달 공정이 정상 종료되었습니다.");
    }
}