using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject GameObject_CannonballPrefab; // 쏠 포탄 프리팹
    [SerializeField] private Transform Transform_FirePoint;          // 포탄이 생성될 포구의 위치
    [SerializeField] private float Float_FireCooldown = 1f;          // 연사 대기 시간 (1초에 1발)

    [Header("Aim Settings")]
    [SerializeField] private float Float_AimRotateSpeed = 100f;       // 조준선 회전 속도
    [SerializeField] private float Float_MinAimAngle = -45f;         // 좌측 최대 각도 제한 (배 정면 기준)
    [SerializeField] private float Float_MaxAimAngle = 45f;          // 우측 최대 각도 제한 (배 정면 기준)

    private float _lastFireTime = 0f; // 마지막으로 발사한 시간 기록용
    private float _currentAimAngle = 0f; // 현재 조준하고 있는 로컬 각도 상태 저장

    private void Update()
    {
        HandleAimInput();
        HandleAttackInput();
    }
    private void HandleAimInput()
    {
        // Shift 키가 누르고 있는 상태인지 먼저 확인합니다. (LeftShift 또는 RightShift 모두 허용)
        if (Input.GetKey(KeyCode.LeftShift) == true || Input.GetKey(KeyCode.RightShift) == true)
        {
            float rotateDirection = 0f;

            // Shift를 누른 상태에서 좌/우 화살표(방향키) 입력을 받습니다.
            if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                rotateDirection = 1f;  // 반시계 회전
            }
            else if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                rotateDirection = -1f; // 시계 회전
            }

            if (rotateDirection != 0f)
            {
                CalculateAimRotation(rotateDirection);
            }
        }
    }

    private void CalculateAimRotation(float direction)
    {
        if (Transform_FirePoint == null)
        {
            return;
        }
        // 1. 프레임 독립적인 회전량을 현재 각도에 더해줍니다.
        _currentAimAngle += direction * Float_AimRotateSpeed * Time.deltaTime;

        // 2. 뼈대 핵심: 지정한 최소/최대 각도를 벗어나지 못하도록 철저하게 가둡니다.
        _currentAimAngle = Mathf.Clamp(_currentAimAngle, Float_MinAimAngle, Float_MaxAimAngle);

        // 3. 로컬 회전(localRotation)에 적용하여, 배가 회전하더라도 포구는 배를 기준으로 고정 각도를 유지하게 만듭니다.
        Transform_FirePoint.localRotation = Quaternion.Euler(0f, 0f, _currentAimAngle);

        Debug.Log($"[함포 조준] 현재 함포 로컬 조준 각도: {_currentAimAngle}°");
    }


    // 플레이어의 공격 입력을 감지하는 함수
    private void HandleAttackInput()
    {
        // 스페이스바를 누르면 발사 시도
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            TryFireCannon();
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

            // 2. 생성된 포탄에서 컴포넌트를 낚아챕니다.
            Cannonball cannonballScript = spawnedBall.GetComponent<Cannonball>();

            if (cannonballScript != null)
            {
                //  3. 기획 데이터 ID를 주입합니다! (예: 표준 캘버린 포 장착 상태라고 가정)
                string currentEquippedWeaponId = "Weapon_Canon_0002";

                cannonballScript.SetupCannonballData(currentEquippedWeaponId);
            }

            Debug.Log("[함포] 제원이 반영된 포탄 발사 완료!");

            // TODO: 대포 발사 사운드 재생 (AudioManager 활용)
        }
        else
        {
            Debug.LogWarning("[함포] 포탄 프리팹이나 발사구 위치(FirePoint)가 할당되지 않았습니다.");
        }
    }
}