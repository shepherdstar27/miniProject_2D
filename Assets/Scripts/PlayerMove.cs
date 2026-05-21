using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 배의 속도 상태를 명확하게 관리하기 위한 열거형(enum)
    private enum EShipGearState
    {
        BackFast,  // 2단 감속 
        BackSlow,  // 1단 감속 
        Stop,      // 정지
        MoveSlow,  // 1단 가속 
        MoveFast   // 2단 가속
    }

    [Header("Speed Settings")]
    [SerializeField] private float Float_MoveFastSpeed = 1f;
    [SerializeField] private float Float_MoveSlowSpeed = 1f;
    [SerializeField] private float Float_MoveBackSlowSpeed = -1f;
    [SerializeField] private float Float_MoveBackFastSpeed = -1f;

    [Header("Engine Extracted Specs")]
    [SerializeField] private string  String_EngineId = "";
    [SerializeField] private float   Float_MoveSpeed = 1f;
    [SerializeField] private float   Float_RotateSpeed = 150f;
    [SerializeField] private float   Float_Acceleration = 3f;

    private Rigidbody2D m_rigidbody2D; // 기존 변수명 유지

    // 현재 배의 기어 상태 (기본값은 정지)
    private EShipGearState _currentGear = EShipGearState.Stop;

    private float _targetMoveSpeed = 0f;  // 기어에 따른 '목표' 속도
    private float _currentMoveSpeed = 0f; // 관성이 반영된 '현재' 실제 속도
    private float _rotateDirection = 0f;


    // 외부 상호작용 스크립트가 배의 조작을 잠그거나 풀 수 있도록 제어하는 플래그 변수
    private bool _isControlActive = true;



    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 항구 정박 등으로 인해 조작 권한이 비활성화되었다면 키보드 입력을 받지 않고 패스
        if (_isControlActive == false)
        {
            return;
        }
        SetupSpeedSettings();
        HandleSpeedInput();
        HandleRotateInput();
    }

    void FixedUpdate()
    {

        CalculateVelocityInertia();
        MoveShip();
        RotateShip();
    }

    //[핵심 데이터 주입구]: PlayerEquipment에서 JSON의 EngineData 데이터를 읽어와 원격 호출

    public void SetupEngineSpecs(string engineId, float movespeed, float rotatespeed, float acceleration)
    {
        String_EngineId = engineId;
        Float_MoveSpeed = movespeed;
        Float_RotateSpeed = rotatespeed;
        Float_Acceleration = acceleration;
    }

    // EngineData 의 speed를 읽어와 배율 곱해줌
    private void SetupSpeedSettings()
    {
        Float_MoveFastSpeed = Float_MoveSpeed * 2;
        Float_MoveSlowSpeed = Float_MoveSpeed;
        Float_MoveBackSlowSpeed = Float_MoveSpeed * -1;
        Float_MoveBackFastSpeed = Float_MoveSpeed * -2;
    }


    private void HandleSpeedInput()
    {
        // GetKeyDown을 사용하여 키를 '누르는 순간 딱 한 번만' 단계가 변하도록 합니다.
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShiftGearUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ShiftGearDown();
        }
    }

    // 기어 올리기 (W 키)
    private void ShiftGearUp()
    {
        if (_currentGear < EShipGearState.MoveFast)
        {
            _currentGear++; // enum 순서상 다음 단계로 상승
            ApplyGearTargetSpeed();
            Debug.Log($"[배 조작] 기어 업! 현재 상태: {_currentGear} (속도: {_targetMoveSpeed})");
        }
    }

    // 기어 내리기 (S 키)
    private void ShiftGearDown()
    {
        if (_currentGear > EShipGearState.BackFast)
        {
            _currentGear--; // enum 순서상 이전 단계로 하락
            ApplyGearTargetSpeed();
            Debug.Log($"[배 조작] 기어 다운! 현재 상태: {_currentGear} (속도: {_targetMoveSpeed})");
        }
    }

    // 변경된 기어 상태에 맞춰 실제 목표 속도를 매칭해주는 함수
    private void ApplyGearTargetSpeed()
    {
        switch (_currentGear)
        {
            case EShipGearState.BackFast: _targetMoveSpeed = Float_MoveBackFastSpeed; break;
            case EShipGearState.BackSlow: _targetMoveSpeed = Float_MoveBackSlowSpeed; break;
            case EShipGearState.Stop: _targetMoveSpeed = 0f; break;
            case EShipGearState.MoveSlow: _targetMoveSpeed = Float_MoveSlowSpeed; break;
            case EShipGearState.MoveFast: _targetMoveSpeed = Float_MoveFastSpeed; break;
        }
    }

    private void CalculateVelocityInertia()
    {
        // Mathf.MoveTowards(현재값, 목표값, 최대변화량)
        // FixedUpdate 안이므로 Time.fixedDeltaTime을 곱해 컴퓨터 사양에 상관없이 일정한 가속을 보장합니다.
        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, _targetMoveSpeed, Float_Acceleration * Time.fixedDeltaTime);
    }

    // 2. A/D 키 입력을 받아 좌/우 회전 방향을 결정하는 함수
    private void HandleRotateInput()
    {
        _rotateDirection = 0f;

        // 회전은 누르고 있는 동안 계속 돌아야 하므로 GetKey를 사용합니다.
        if (Input.GetKey(KeyCode.A))
        {
            _rotateDirection = 1f; // 유니티 2D에서 +회전은 반시계방향(좌회전)입니다.
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rotateDirection = -1f; // -회전은 시계방향(우회전)입니다.
        }
    }

    // 3. 물리 엔진을 이용해 배를 전진/후진 시키는 함수
    private void MoveShip()
    {
        // 배가 바라보고 있는 방향(우측 방향)을 기준으로 속도를 적용합니다.
        Vector2 moveVelocity = transform.right * _currentMoveSpeed;
        m_rigidbody2D.linearVelocity = moveVelocity;
    }

    // 4. 물리 엔진을 이용해 배를 회전시키는 함수
    private void RotateShip()
    {
        bool isMovingInput = _currentMoveSpeed != 0;


        if (!isMovingInput || _rotateDirection == 0f)
        {
            m_rigidbody2D.angularVelocity = 0f;
            return;
        }

        // 현재 회전 방향과 회전 속도를 곱해 물리 회전력을 가합니다.
        m_rigidbody2D.angularVelocity = _rotateDirection * Float_RotateSpeed;
    }

    // PlayerInteractionPort에서 호출하여 배의 조작 권한을 잠그거나 풀 때 사용하는 함수
    public void SetControlActive(bool isActive)
    {
        _isControlActive = isActive;
    }

    // PlayerInteractionPort에서 호출하여 정박 시 기어와 내부 속도 연산용 변수들을 깔끔하게 초기화하는 함수
    public void StopShipMoveAndResetGear()
    {
        _currentGear = EShipGearState.Stop;
        _targetMoveSpeed = 0f;
        _currentMoveSpeed = 0f;
    }


}