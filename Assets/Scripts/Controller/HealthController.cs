using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Configuration")]
    [SerializeField] private bool Bool_IsPlayer = false; // 플레이어 배인지 적 배인지 구분하는 인스펙터 스위치

    [Header("Live Health Status")]
    [SerializeField] private float float_CurrentHP;
    [SerializeField] private float float_Max_HP;

    // [핵심 데이터 주입구]: 각 함선의 초기화 스크립트가 JSON 수치를 가져와 이 함수를 때려줍니다.
    public void SetupMaxHp(float maxHpValue)
    {
        float_Max_HP = maxHpValue;
        float_CurrentHP = float_Max_HP;
        Debug.Log($"[{gameObject.name}] 체력 데이터 동동기화 완료. HP: {float_Max_HP}");
    }

    // [핵심 피격 제어]: 포탄이 나를 맞췄을 때 호출하는 정식 물리 대미지 차감 함수
    public void TakeDamage(float damageAmount)
    {
        if (BattleManager.Instance.GetIsGameOver() == true) return;

        float_CurrentHP -= damageAmount;
        Debug.Log($"[{gameObject.name}] 피격 발생! 데미지: {damageAmount} / 남은체력: {float_CurrentHP}");

        if (float_CurrentHP <= 0f)
        {
            float_CurrentHP = 0f;
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        if (Bool_IsPlayer == true)
        {
            // 나 자신이 플레이어였다면 심판에게 패배를 타전

            BattleManager.Instance.CallPlayerDefeat();
        }
        else
        {
            // 1. [연동 완치]: 내 몸에 장착된 EnemyAI 컴포넌트를 정밀 역추적합니다.
            EnemyAI enemyAILogic = GetComponent<EnemyAI>();
            string finalDropTableId = "DropTable_0001"; // 데이터 유실을 대비한 예외처리 기본 보험값

            if (enemyAILogic != null)
            {
                // 2. 적 AI 스크립트 내부에서 JSON 조립이 완료된 실시간 드랍테이블 ID를 안전하게 낚아챕니다.
                finalDropTableId = enemyAILogic.GetDropTableId();
            }

            // 3. 심판(BattleManager)에게 최종 정렬된 드랍 테이블 ID와 현재 격침된 위치 좌표를 전송합니다.
            BattleManager.Instance.CallPlayerVictory(finalDropTableId, transform.position);

            // 4. 월드 하이어라키에서 선체 실물 철거
            Destroy(gameObject);
        }
    }
}