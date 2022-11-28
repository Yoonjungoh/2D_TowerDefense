using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private Sprite[] speedButtonSprites;

    private int currentWaveIndex = -1;

    static private Define.MultiplySpeedType type;
    static public Define.MultiplySpeedType Type => type;
    private int speedButtonCount = 0;

    static private float ratio = 1;    // ��� ��ư ����
    static public float Ratio => ratio;
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;
    public void IntializeToStartWave()
    {
        // ���̺� ���� �ʱ�ȭ ������� �� �͵�
        //speedButtonCount = 0;

        int towerCount = towerSpawner.TowerList.Count;

        if (towerSpawner.TowerList != null)
        {
            for (int i = 0; i < towerCount; i++)
            {
                TowerController tower = towerSpawner.TowerList[i];
                tower.IntializeAbility(type, ratio);
            }
        }
    }
    public void StartWave()
    {
        if(enemySpawner.EnemyList.Count == 0 & currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }

        IntializeToStartWave();   // ���̺� ������ �ѹ��� �����ϴ°� ������
    }
    public void SkipWave()
    {
        if (enemySpawner.EnemyList.Count != 0)
        {
            enemySpawner.DestroyAllEnemy(enemySpawner);
            IntializeToStartWave();   // ���̺� ������ �ѹ��� �����ϴ°� ������
        }
    }
    public void SpeedUpWave()
    {
        // ���͵鿡�� ������ �� �������ε� ������ �̸��ϰ� ���� �ڿ� �����ϰ� �ϴ� �����̸� ���� ������
        speedButtonCount++;
        ratio = (speedButtonCount) % 3 + 1;

        switch (ratio)  // ���ǵ� ��� Ÿ�� ���� ���� �̷��� ���� �ʿ�� ������
        {
            case (1):
                type = Define.MultiplySpeedType.X1;
                break;
            case (2):
                type = Define.MultiplySpeedType.X2;
                break;
            case (3):
                type = Define.MultiplySpeedType.X3;
                break;
        }

        int enemyCount = enemySpawner.EnemyList.Count;
        int towerCount = towerSpawner.TowerList.Count;

        for (int i = 0; i < enemyCount; i++)
            enemySpawner.EnemyList[i].GetComponent<Movement2D>().MultiplymoveSpeed(type, ratio);

        if (towerSpawner.TowerList != null)
        {
            for (int i = 0; i < towerCount; i++)
                towerSpawner.TowerList[i].MultiplyAbility(type, ratio);
        }

        // ��� Ŭ���� ���� ������Ʈ�� �����ͼ� ����
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        // ��� ��ư�� ��������Ʈ ����
        clickObject.GetComponentInChildren<Image>().sprite = speedButtonSprites[(int)ratio - 1];
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}