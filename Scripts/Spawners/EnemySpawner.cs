using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyHPSliderPrefab;
    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private Transform[] wayPoints;  // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;

    private Wave currentWave;
    private int currentEnemyCount;
    private List<EnemyController> enemyList;
    public List<EnemyController> EnemyList => enemyList;    // get은 필요없고 set만 필요

    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    void Awake()
    {
        enemyList = new List<EnemyController>();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);

            clone.transform.parent = gameObject.transform;

            EnemyController enemy = clone.GetComponent<EnemyController>();

            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount++;

            enemy.GetComponent<Movement2D>().MultiplymoveSpeed(WaveSystem.Type, WaveSystem.Ratio);

            yield return new WaitForSeconds(currentWave.spawnTime);
        }

    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);   // UI는 Canvas의 자식이어야 보이므로
        sliderClone.transform.localScale = Vector3.one; // 계층 설정으로 인해 바뀐 크기 재조정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);    // 슬라이더 위치 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>()); // 슬라이더 체력 설정
    }

    public void DestroyEnemy(Define.EnemyDestroyType type, EnemyController enemy, int gold = 0)
    {
        if (type == Define.EnemyDestroyType.Arrive)
            playerHP.OnDamage(1);   // 하드코딩 고치기 TODO
        else if (type == Define.EnemyDestroyType.Kill)
            playerGold.CurrentGold += gold;

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);  // 오브젝트 풀링 TODO
    }
    public void DestroyAllEnemy(EnemySpawner enemySpawner)
    {
        int count = enemySpawner.enemyList.Count;

        playerHP.OnDamage(count);

        for (int i = 0; i < count; i++)
            Destroy(enemySpawner.enemyList[i].gameObject);

        enemySpawner.enemyList.Clear();
    }
}
