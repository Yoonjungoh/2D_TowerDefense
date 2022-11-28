using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    public TowerTemplate[] TowerTemplate => towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;   // Ÿ�� �Ǽ� ��ư ������?
    private GameObject followTowerClone = null; // �ӽ� Ÿ�� -> ���콺 ����
    private int towerType;

    private List<TowerController> towerList;
    public List<TowerController> TowerList => towerList;    // get�� �ʿ���� set�� �ʿ�

    //private List<TowerController> towerList;  // �� ��������;; nullReference
    //public List<TowerController> TowerList => towerList;    // get�� �ʿ���� set�� �ʿ�

    void Awake()
    {
        towerList = new List<TowerController>();
    }
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        if (isOnTowerButton == true)    // ��ư ������ ������ �� ����
            return;

        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(Define.SystemMSGType.Money);
            return;
        }

        isOnTowerButton = true;

        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);

        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        if (isOnTowerButton == false)
            return;

        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower)  // �̹� ������ ��
        {
            systemTextViewer.PrintText(Define.SystemMSGType.Build);
            return;
        }


        Vector3 position = tileTransform.position + Vector3.back;   // Ÿ�Ϻ��� z�� -1�� ��ġ�� ��ġ �Ͽ� �켱 ���� �̰Ŷ����� 3d ������

        GameObject tower = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);

        #region clone ����
        // Ŭ�� �̸� ���� �κ�
        int index = tower.name.IndexOf("(Clone)");
        if (index > 0)
            tower.name = tower.name.Substring(0, index);
        #endregion
         
        tower.transform.parent = gameObject.transform;  // Instantiate �� ���� ���� ������ ������ �Ŀ� �θ� �����ؾ� ���� �ȳ�

        isOnTowerButton = false;

        tile.IsBuildTower = true;

        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;


        tower.GetComponent<TowerController>().Setup(this, enemySpawner, playerGold, tile);  // Ÿ���� enemySpawner ���� ����
        OnBuffAllBuffTowers();  // ���� ����

        Destroy(followTowerClone);

        StopCoroutine("OnTowerCancelSystem");
        towerList.Add(tower.GetComponent<TowerController>());   // Ÿ������Ʈ�� Ÿ�� �߰�
        TowerController towerObj = tower.GetComponent<TowerController>();
        towerObj.MultiplyAbility(WaveSystem.Type, WaveSystem.Ratio);
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) // esc Ȥ�� ���콺 ������ ��ư
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }


            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()   // ���� Ÿ������ ���߿� ��ġ�� Ÿ���鵵 ���� �ֱ� ����
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i =0; i < towers.Length; i++)
        {
            TowerController tower = towers[i].GetComponent<TowerController>();

            if (tower.WeaponType == Define.WeaponType.Buff)
                tower.OnBuffAroundTower();  // ���� ����
        }
    }
}
