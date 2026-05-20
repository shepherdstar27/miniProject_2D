using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Live Specs")]
    [SerializeField] private float float_CurrentHP;
    [SerializeField] private float float_Max_HP;
    [SerializeField] private float Float_MoveSpeed;
    [SerializeField] private int Int_AttackDamage;

    public void SetupEnemyDetails(string enemyId)
    {
        // 1. 적 마스터 테이블 조회
        EnemyData enemyMaster = GameDataManager.Instance.GetEnemyData(enemyId);
        if (enemyMaster == null) return;

        float_Max_HP = enemyMaster.Max_HP;
        float_CurrentHP = float_Max_HP;

        // 2. 엔진 ID를 활용해 엔진 마스터 테이블 교차 검색 (속도 낚아채기)
        EngineData engineMaster = GameDataManager.Instance.GetEngineData(enemyMaster.Engine_Id);
        if (engineMaster != null)
        {
            Float_MoveSpeed = engineMaster.MoveSpeed;
        }

        // 3. 무기 ID를 활용해 무기 마스터 테이블 교차 검색 (데미지 낚아채기)
        WeaponData weaponMaster = GameDataManager.Instance.GetWeaponData(enemyMaster.Weapon_Id);
        if (weaponMaster != null)
        {
            Int_AttackDamage = weaponMaster.Damage;
        }

        Debug.Log($"[적 스펙 조립완료] 명칭: {enemyMaster.Name} / HP: {float_Max_HP} / 속도: {Float_MoveSpeed} / 공격력: {Int_AttackDamage}");
    }
}
