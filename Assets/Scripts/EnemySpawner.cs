using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    // 규칙 반영: 일반 멤버 변수는 _소문자 시작
    [SerializeField] private string _targetEnemyId = "mob_ship_0001";

    [Header("Unity Component References")]
    // 규칙 반영: 유니티 참조 객체는 대문자 시작
    [SerializeField] private Transform Transform_SpawnPoint;
    [SerializeField] private Camera Camera_Main;

    // 현재 생존해 있는 몬스터를 추적하기 위한 변수
    private GameObject _currentEnemy;

    private void Start()
    {
        // 카메라가 할당되지 않았다면 메인 카메라를 자동으로 찾습니다.
        if (Camera_Main == null)
        {
            Camera_Main = Camera.main;
        }
    }

    private void Update()
    {
        HandleAutoSpawn();
    }

    private void HandleAutoSpawn()
    {
        // 추적 중인 몬스터가 없거나(null) 파괴되었다면 새로 소환 조건을 검사합니다.
        if (_currentEnemy == null)
        {
            // 스포너 위치가 현재 카메라 시야 밖일 때만 소환을 시도합니다.
            if (CheckIsOutsideCameraView() == true)
            {
                TrySpawnEnemy();
            }
        }
    }

    // 지정된 스폰 위치가 화면 밖인지 판별하는 함수
    private bool CheckIsOutsideCameraView()
    {
        if (Camera_Main == null) return true; // 카메라가 없으면 무조건 소환 허용

        // 1. 기준 좌표 설정
        Vector3 spawnPos = transform.position;
        if (Transform_SpawnPoint != null)
        {
            spawnPos = Transform_SpawnPoint.position;
        }

        // 2. 월드 좌표를 화면(Viewport) 비율 좌표로 변환
        Vector3 viewportPos = Camera_Main.WorldToViewportPoint(spawnPos);

        // 3. X나 Y 좌표가 0 미만이거나 1 초과라면 화면 밖입니다.
        if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
        {
            return true;
        }

        // 0.0 ~ 1.0 사이라면 화면 안(유저의 눈에 보이는 상태)입니다.
        return false;
    }

    private void TrySpawnEnemy()
    {
        if (GameDataManager.Instance == null) return;

        // 1. 기획 데이터 조회
        EnemyData enemyData = GameDataManager.Instance.GetEnemyData(_targetEnemyId);

        if (enemyData == null)
        {
            Debug.LogError($"[스포너] ID [{_targetEnemyId}] 는 기획 테이블에 존재하지 않아 소환에 실패했습니다.");
            return;
        }

        // 2. 프리팹 로드
        GameObject loadedPrefab = Resources.Load<GameObject>(enemyData.PrefabPath);

        if (loadedPrefab != null)
        {
            // 기준 좌표 가져오기
            Vector3 spawnPos = transform.position;
            if (Transform_SpawnPoint != null)
            {
                spawnPos = Transform_SpawnPoint.position;
            }
            spawnPos.z = 0f;

            // 3. 적 프리팹 실시간 복사 생성 및 변수에 캐싱
            _currentEnemy = Instantiate(loadedPrefab, spawnPos, Quaternion.identity);

            // 4. 데이터 최종 주입
            EnemyAI enemyAILogic = _currentEnemy.GetComponent<EnemyAI>();
            if (enemyAILogic != null)
            {
                enemyAILogic.SetupEnemyDetails(_targetEnemyId);
            }
        }
        else
        {
            Debug.LogError($"[스포너] {enemyData.PrefabPath} 경로에서 적 프리팹을 찾지 못했습니다! 폴더 구조나 대소문자를 확인하세요.");
        }
    }
}