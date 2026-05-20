using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [Header("Cannonball Settings")]
    [SerializeField] private float Float_Speed = 10f;       // 포탄 날아가는 속도
    [SerializeField] private int Int_Damage = 0;           // 적에게 줄 데미지
    [SerializeField] private float Float_LifeTime = 2f;     // 허공에 날아가다 사라질 시간 (메모리 누수 방지)


    [Header("Cannonball Destroy Effect")]
    public GameObject Prefab_CannonballDestroyEffect;       // 파괴될 때 파편이 튀는 폭발 이펙트 프리팹


    private void Start()
    {
        // 생성된 지 특정 시간이 지나면 스스로 파괴되도록 예약합니다.
        Destroy(gameObject, Float_LifeTime);
    }

    private void Update()
    {
        MoveProjectile();
    }

    // 매 프레임 앞으로 날아가는 물리 이동 함수
    private void MoveProjectile()
    {
        // transform.right는 오브젝트가 바라보는 우측(기본 앞방향)을 의미합니다.
        transform.Translate(Vector3.right * Float_Speed * Time.deltaTime);
    }

    // 대포를 발사하는 플레이어나 적이 생성 직후 이 함수를 호출하여 무기 제원을 주입
    public void SetupCannonballData(string weaponId)
    {
        WeaponData masterData = GameDataManager.Instance.GetWeaponData(weaponId);

        if (masterData != null)
        {
            Int_Damage = masterData.Damage;
            Debug.Log($"[포탄 셋업] 무기 [{masterData.Name}]의 데이터 동기화 완료. 위력: {Int_Damage}");
        }
    }

    // 적의 콜라이더와 겹쳤을 때 발동하는 센서 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 대상이 적(Enemy)인지 확인합니다.
        if (collision.CompareTag("Enemy") == true)
        {
            Debug.Log($"[포탄] 적 명중! {Int_Damage}의 데미지를 입힙니다.");

            // TODO: collision.GetComponent<EnemyHP>().TakeDamage(Int_Damage); 형태로 데미지 전달

            // 타격했으므로 포탄 자신은 파괴됩니다.
            DestroyCannonball();
        }
    }

    // 포탄을 안전하게 파괴하는 함수
    private void DestroyCannonball()
    {
        if (Prefab_CannonballDestroyEffect != null)
        {
            GameObject effectObj = Instantiate(Prefab_CannonballDestroyEffect, transform.position, Quaternion.identity);

            // 프리팹 자체를 디스트로이 시키는 실수를 막기 위해, 생성된 인스턴스 기물(effectObj)을 0.5초 뒤 파괴 지시
            Destroy(effectObj, 0.5f);
        }

        // 투사체 삭제
        Destroy(gameObject);

        // 파괴될 때 파편이 튀는 폭발 이펙트 프리팹
        //Instantiate(Prefab_CannonballDestroyEffect);
        //Destroy(Prefab_CannonballDestroyEffect, 1f);
    }
}