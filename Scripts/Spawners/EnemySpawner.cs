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
    private Transform[] wayPoints;  // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;

    private Wave currentWave;
    private int currentEnemyCount;
    private List<EnemyController> enemyList;
    public List<EnemyController> EnemyList => enemyList;    // get�� �ʿ���� set�� �ʿ�

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
        sliderClone.transform.SetParent(canvasTransform);   // UI�� Canvas�� �ڽ��̾�� ���̹Ƿ�
        sliderClone.transform.localScale = Vector3.one; // ���� �������� ���� �ٲ� ũ�� ������
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);    // �����̴� ��ġ ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>()); // �����̴� ü�� ����
    }

    public void DestroyEnemy(Define.EnemyDestroyType type, EnemyController enemy, int gold = 0)
    {
        if (type == Define.EnemyDestroyType.Arrive)
            playerHP.OnDamage(1);   // �ϵ��ڵ� ��ġ�� TODO
        else if (type == Define.EnemyDestroyType.Kill)
            playerGold.CurrentGold += gold;

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);  // ������Ʈ Ǯ�� TODO
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
