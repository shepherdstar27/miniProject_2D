using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [Header("Enemy Component Cache")]
    [SerializeField] private Rigidbody2D Rigidbody_Rb2D;

    [Header("Enemy Live Specs")]
    [SerializeField] private float Float_MoveSpeed;
    [SerializeField] private int Int_AttackDamage;
    [SerializeField] private float Float_RotateSpeed; // 배의 회전 속도
    [SerializeField] private float Float_Acceleration; // 배의 관성

    [Header("AI Logic Helpers")]
    [SerializeField] private Transform Transform_TargetPlayer; // 추적할 플레이어 타겟

    [Header("Live Movement Logic Status")]
    [SerializeField] private float _currentMoveSpeed = 0f; // 규칙 반영: 현재 실시간 이동 속도 상태 저장 변수

    [Header("Attack Settings")]
    [SerializeField] private GameObject GameObject_CannonballPrefab; // 쏠 포탄 프리팹
    [SerializeField] private Transform Transform_FirePoint;          // 포탄이 생성될 포구의 위치
    [SerializeField] private float Float_FireCooldown = 1f;          // 연사 대기 시간 (1초에 1발)

    [Header("Raycast AI Detector Settings")]
    [SerializeField] private float Float_DetectionDistance = 8f;     //  레이캐스트 사거리 제한
    [SerializeField] private LayerMask LayerMask_PlayerLayer;        //  플레이어만 골라 감지할 필터 레이어

    [Header("AI Intelligence Settings")]
    [SerializeField] private float Float_ChaseRange = 3f;          //  이 거리 안으로 들어오면 추적 및 사격 개시
    [SerializeField] private bool Bool_IsChasingPlayer = false;      //  현재 추적 상태인지 여부 기록 그릇

    private float _lastFireTime = 0f; // 마지막 발사 시간 기록용 일반 private 변수
    private string _equippedWeaponId; // 적이 장착 중인 무기 ID



    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        FindPlayerTarget();
    }

    private void FixedUpdate()
    {
        if (BattleManager.Instance.GetIsGameOver() == true)
        {
            if (Rigidbody_Rb2D != null) Rigidbody_Rb2D.linearVelocity = Vector2.zero;
            return;
        }

        if (Transform_TargetPlayer == null)
        {
            FindPlayerTarget();
            return;
        }

        //  [핵심 가동]: 매 물리 프레임마다 나와 플레이어 사이의 정밀 거리를 연산합니다.
        HandleAIIntelligence();

        //  인지 필터 가동: 추적 모드가 발동했을 때만 기동 및 포격을 수행합니다.
        if (Bool_IsChasingPlayer == true)
        {
            HandleMovementAI();
            HandleRaycastAttackAI();
        }
        else
        {
            // 플레이어가 인지 사정거리 밖에 있다면 관성에 의해 서서히 감속하다 멈추게 유도합니다.
            HandleIdleFriction();
        }
    }

    private void CacheComponents()
    {
        if (Rigidbody_Rb2D == null)
        {
            Rigidbody_Rb2D = GetComponent<Rigidbody2D>();
        }
    }

    private void FindPlayerTarget()
    {
        // 씬에서 Player 태그를 가진 구동 기물을 찾아 조타 목표로 설정합니다.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Transform_TargetPlayer = playerObj.transform;
        }
    }

    private void HandleAIIntelligence()
    {
        // 나와 플레이어 사이의 순수한 2D 평면 거리를 잽니다.
        float currentDistance = Vector2.Distance(transform.position, Transform_TargetPlayer.position);

        if (currentDistance <= Float_ChaseRange)
        {
            // 지정한 사정거리 안으로 플레이어가 침투하면 추적 모드 On
            if (Bool_IsChasingPlayer == false)
            {
                Bool_IsChasingPlayer = true;
                Debug.Log($"[AI 경보] 플레이어 침입 포착! 거리: {currentDistance} / 추적 및 사격을 개시합니다.");
            }
        }
        else
        {
            // 플레이어가 범위를 탈출하면 추적 모드 Off (다시 은닉 유도)
            if (Bool_IsChasingPlayer == true)
            {
                Bool_IsChasingPlayer = false;
                Debug.Log($"[AI 상황 종료] 플레이어가 시야를 벗어났습니다. 대기 상태로 전환합니다.");
            }
        }
    }

    // 비추적 상태일 때 배가 관성에 의해 영원히 미끄러지는 현상을 막는 감속 브레이크 함수
    private void HandleIdleFriction()
    {
        if (Rigidbody_Rb2D == null) return;

        // 목표 속도를 0으로 잡고 관성 가속도 수치만큼 서서히 감속시킵니다.
        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, 0f, Float_Acceleration * Time.fixedDeltaTime);
        Rigidbody_Rb2D.linearVelocity = transform.right * _currentMoveSpeed;
    }

    public void SetupEnemyDetails(string enemyId)
    {
        // 1. 적 마스터 테이블 조회
        EnemyData enemyMaster = GameDataManager.Instance.GetEnemyData(enemyId);
        if (enemyMaster == null) return;

        // 적 체력 동기화

        // ChaseRange 값으로 두 사거리 변수 덮어 씌우기
        Float_ChaseRange = enemyMaster.ChaseRange;
        Float_DetectionDistance = enemyMaster.ChaseRange;

        // 2. 엔진 ID를 활용해 엔진 마스터 테이블 교차 검색 (속도 낚아채기)
        EngineData engineMaster = GameDataManager.Instance.GetEngineData(enemyMaster.Engine_Id);
        if (engineMaster != null)
        {
            Float_MoveSpeed = engineMaster.MoveSpeed;
            Float_RotateSpeed = engineMaster.RotateSpeed;
            Float_Acceleration = engineMaster.Acceleration;
        }

        // 3. 무기 ID를 활용해 무기 마스터 테이블 교차 검색 (데미지 낚아채기)
        WeaponData weaponMaster = GameDataManager.Instance.GetWeaponData(enemyMaster.Weapon_Id);
        if (weaponMaster != null)
        {
            Int_AttackDamage = weaponMaster.Damage;
            _equippedWeaponId = enemyMaster.Weapon_Id;
        }

        // 4. 배 ID를 활용해 배 마스터 테이블 교차 검색 (배Hp 낚아채기)
        ShipData shipMaster = GameDataManager.Instance.GetShipData(enemyMaster.Ship_Id);
        if (shipMaster != null)
        {
            HealthController myHealth = GetComponent<HealthController>();
            if (myHealth != null)
            {
                myHealth.SetupMaxHp(shipMaster.Max_HP);
            }
        }

        Debug.Log($"[적 스펙 조립완료] 명칭: {enemyMaster.Name} / HP: {shipMaster.Max_HP} / 속도: {Float_MoveSpeed} / 공격력: {Int_AttackDamage}");
    }

    private void HandleMovementAI()
    {
        if (Transform_TargetPlayer == null || Rigidbody_Rb2D == null)
        {
            return;
        }

        // 1. [방향 연산] 플레이어가 있는 방향의 벡터를 구합니다.
        Vector2 direction = (Vector2)Transform_TargetPlayer.position - Rigidbody_Rb2D.position;
        direction.Normalize();

        // 2. [선회 제어] 현재 적 배가 플레이어를 바라보도록 정밀 각도 연산 및 선회 회전 수행
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float smoothedAngle = Mathf.MoveTowardsAngle(Rigidbody_Rb2D.rotation, targetAngle, Float_RotateSpeed * Time.fixedDeltaTime);
        Rigidbody_Rb2D.MoveRotation(smoothedAngle);

        // 3. [가속도 관성 연산] 유저님이 요청하신 플레이어 스타일의 부드러운 가속 공식 대입
        // 목표 속도는 JSON 테이블에서 로드된 최고 속도(Float_MoveSpeed)입니다.
        float targetSpeed = Float_MoveSpeed;

        // Mathf.MoveTowards를 사용해 현재 속도를 최고 속도까지 가속도 수치만큼 매 프레임 야금야금 올립니다.
        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, targetSpeed, Float_Acceleration * Time.fixedDeltaTime);

        // 4. [물리 이동 적용] 계산된 실시간 현재 속도 방향을 리지드바디 속도(Velocity)에 직접 정밀 주입합니다.
        // 유니티 6 순정 규격인 linearVelocity를 활용해 자신이 바라보는 앞방향(transform.right)으로 미끄러지듯 전진시킵니다.
        Rigidbody_Rb2D.linearVelocity = transform.right * _currentMoveSpeed;
    }

    // 플레이어를 감지하고 공격하는 함수(레이 캐스터를 가지고 배의 정면에 있을때 감지)
    private void HandleRaycastAttackAI()
    {
        if (Transform_FirePoint == null)
        {
            return;
        }

        // 시작점: 대포가 발사될 포구 위치 (FirePoint)
        Vector2 rayStartPos = Transform_FirePoint.position;

        // 방향: 적 함선이 현재 바라보고 있는 정면 방향 (transform.right)
        Vector2 rayDirection = transform.right;

        // 유니티 2D 물리 월드에 레이저를 투사하여 그 결과를 받아옵니다.
        RaycastHit2D hitResult = Physics2D.Raycast(rayStartPos, rayDirection, Float_DetectionDistance, LayerMask_PlayerLayer);

        // 에디터 씬(Scene) 뷰에서 적 배가 쏘고 있는 조준 레이저를 눈으로 확인하기 위한 디버그 빔 라인 작도
        Debug.DrawRay(rayStartPos, rayDirection * Float_DetectionDistance, Color.red);

        // 무언가 레이저 장벽에 걸렸는지 1차 검증
        if (hitResult.collider != null)
        {
            // 레이저에 닿은 물체가 "Player" 태그를 보유하고 있는지 확실하게 2차 검증 수행

            if (hitResult.collider.CompareTag("Player") == true)
            {
                // 조준선 상에 플레이어가 들어왔으므로 즉시 발사 메커니즘 가동

                TryFireCannon();
            }

        }

    }
   
    
    // 쿨타임을 계산하여 발사 가능 여부를 검증하는 함수
    private void TryFireCannon()
    {
        // 현재 시간이 (마지막 발사 시간 + 쿨타임)을 지났는지 확인
        if (Time.time >= _lastFireTime + Float_FireCooldown)
        {
            FireCannon();
            _lastFireTime = Time.time; // 발사 성공 시 시간 갱신
        }
        else
        {
            Debug.Log("[함포] 아직 재장전 중입니다!");
        }
    }

    // 실제로 프리팹을 생성하여 허공에 쏘아 보내는 함수
    private void FireCannon()
    {
        if (GameObject_CannonballPrefab != null && Transform_FirePoint != null)
        {
            // 1. 포탄을 먼저 복사 생성합니다.
            GameObject spawnedBall = Instantiate(GameObject_CannonballPrefab, Transform_FirePoint.position, Transform_FirePoint.rotation);

            // [추가 방어 코드] 포탄이 날아가다 사격한 '자기 자신(Enemy)' 콜라이더와 부딪혀 
            // 제자리에서 즉시 폭발하는 유니티 물리 억까 버그를 차단하기 위해 충돌을 무시시킵니다.
            Collider2D enemyCollider = GetComponent<Collider2D>();
            Collider2D ballCollider = spawnedBall.GetComponent<Collider2D>();
            if (enemyCollider != null && ballCollider != null)
            {
                Physics2D.IgnoreCollision(enemyCollider, ballCollider);
            }

            // 2. 생성된 포탄에서 컴포넌트를 낚아챕니다.
            Cannonball cannonballScript = spawnedBall.GetComponent<Cannonball>();
            if (cannonballScript != null)
            {
                //  3. 기획 데이터 ID를 주입합니다
                if (string.IsNullOrEmpty(_equippedWeaponId) == false)
                {
                    cannonballScript.SetupCannonballData(_equippedWeaponId);
                }
                else
                {
                    // 예외 처리용 기본 대포 보험 가동
                    cannonballScript.SetupCannonballData("Weapon_Canon_0001");
                }
            }

            Debug.Log($"[적 포격 소장] 플레이어가 사정거리 내에 포착되어 발포했습니다! 데미지 연동 완료.");

            // TODO: 대포 발사 사운드 재생 (AudioManager 활용)
        }
        else
        {
            Debug.LogWarning("[함포] 포탄 프리팹이나 발사구 위치(FirePoint)가 할당되지 않았습니다.");
        }
    }


}
