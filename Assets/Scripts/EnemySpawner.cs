using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private string String_TargetEnemyId = "mob_ship_0001"; // 인스펙터 입력용 변수
    [SerializeField] private Transform Transform_SpawnPoint;                // 소환될 위치

    private void Start()
    {
        TrySpawnEnemy();
    }

    private void TrySpawnEnemy()
    {
        // 1. 내가 소환하려는 ID의 기획 데이터를 조회합니다.
        EnemyData enemyData = GameDataManager.Instance.GetEnemyData(String_TargetEnemyId);

        if (enemyData == null)
        {
            Debug.LogError($"[스포너] ID [{String_TargetEnemyId}] 는 기획 테이블에 존재하지 않아 소환에 실패했습니다.");
            return;
        }

        // 2. 데이터 드리븐 핵심: 데이터에 적힌 "PrefabPath" 경로를 통해 프리팹을 동적으로 로드합니다.
        // 예: Resources/Prefabs/Enemy/Enemy_Basic.prefab 이라면 경로를 "Prefabs/Enemy/Enemy_Basic" 으로 세팅
        GameObject loadedPrefab = Resources.Load<GameObject>(enemyData.PrefabPath);

        if (loadedPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            if (Transform_SpawnPoint != null) spawnPos = Transform_SpawnPoint.position;

            // 3. 적 프리팹 생성
            GameObject spawnedEnemy = Instantiate(loadedPrefab, spawnPos, Quaternion.identity);

            // 4. 생성된 적에게 붙어있는 제어 스크립트를 낚아채서 데이터를 최종 주입합니다.
            EnemyAI enemyAILogic = spawnedEnemy.GetComponent<EnemyAI>();
            if (enemyAILogic != null)
            {
                enemyAILogic.SetupEnemyDetails(String_TargetEnemyId);
            }
        }
        else
        {
            Debug.LogError($"[스포너] {enemyData.PrefabPath} 경로에서 적 프리팹을 찾지 못했습니다! 폴더 구조나 대소문자를 확인하세요.");
        }
    }
}