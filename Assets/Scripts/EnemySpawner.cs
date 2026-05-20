using UnityEngine;

public class EnemySpawner : MonoBehaviour






{
    public GameObject Prefab_Enemy;
    public Transform Root_Enemy;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"[EnemySpawner] P 가 눌려짐");

            SpawnPrefab();
        }
    }

    private void SpawnPrefab()
    {
        //프리팹의 동적 생성
        var EnemyObject = Instantiate(Prefab_Enemy);
        EnemyObject.name = "아무이름";//생성된 Enemy 프리팹의 명칭

    }
}
