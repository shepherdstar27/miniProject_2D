using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private string String_TargetEnemyId = "mob_ship_0001"; // 인스펙터 입력용 변수
    [SerializeField] private Transform Transform_SpawnPoint;                // 소환될 위치

    private void Update()
    {
        HandleSpawnInput();
    }

    private void HandleSpawnInput()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            TrySpawnEnemy();
        }
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
        GameObject loadedPrefab = Resources.Load<GameObject>(enemyData.PrefabPath);

        if (loadedPrefab != null)
        {
            // 기본 스포너 위치를 가져옵니다.
            Vector3 spawnPos = transform.position;

            // 지정된 소환 포인트가 있다면 그 위치를 대입합니다.
            if (Transform_SpawnPoint != null)
            {
                spawnPos = Transform_SpawnPoint.position;
            }
            // 2D 게임의 안전을 위해 Z축 좌표를 무조건 0f로 강제 고정합니다.
            spawnPos.z = 0f;

            // 3. 적 프리팹 실시간 복사 생성
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