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

    static private float ratio = 1;    // 배속 버튼 비율
    static public float Ratio => ratio;
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;
    public void IntializeToStartWave()
    {
        // 웨이브 마다 초기화 시켜줘야 할 것들
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

        IntializeToStartWave();   // 웨이브 끝날때 한번만 실행하는게 나을듯
    }
    public void SkipWave()
    {
        if (enemySpawner.EnemyList.Count != 0)
        {
            enemySpawner.DestroyAllEnemy(enemySpawner);
            IntializeToStartWave();   // 웨이브 끝날때 한번만 실행하는게 나을듯
        }
    }
    public void SpeedUpWave()
    {
        // 몬스터들에게 적용이 좀 느린편인데 스폰을 미리하고 몇초 뒤에 등장하게 하는 형식이면 적용 받을듯
        speedButtonCount++;
        ratio = (speedButtonCount) % 3 + 1;

        switch (ratio)  // 스피드 배속 타입 결정 굳이 이렇게 적을 필요는 없긴함
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

        // 방금 클릭한 게임 오브젝트를 가져와서 저장
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        // 배속 버튼의 스프라이트 변경
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