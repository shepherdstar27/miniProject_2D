using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float string_Engine_Id = 1f; // 엔진
    [SerializeField] private float string_Weapon_Id = 30f; // 무기
    [SerializeField] private float Float_HP = 20f;  // 적의 체력

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupEnemyData(string enemyId)
    {
        EnemyData masterData = GameDataManager.Instance.GetEnemyData(enemyId);

        if (masterData != null)
        {
            Float_HP = masterData.HP;
            Debug.Log($"[적 셋업] 무기 [{masterData.Name}]의 데이터 동기화 완료. 체력: {Float_HP}");
        }
    }
}
