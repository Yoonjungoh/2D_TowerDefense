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
    private bool isOnTowerButton = false;   // 타워 건설 버튼 눌렀나?
    private GameObject followTowerClone = null; // 임시 타워 -> 마우스 따라감
    private int towerType;

    private List<TowerController> towerList;
    public List<TowerController> TowerList => towerList;    // get은 필요없고 set만 필요

    //private List<TowerController> towerList;  // 왜 에러나지;; nullReference
    //public List<TowerController> TowerList => towerList;    // get은 필요없고 set만 필요

    void Awake()
    {
        towerList = new List<TowerController>();
    }
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        if (isOnTowerButton == true)    // 버튼 여러번 누르는 것 방지
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

        if (tile.IsBuildTower)  // 이미 세워진 곳
        {
            systemTextViewer.PrintText(Define.SystemMSGType.Build);
            return;
        }


        Vector3 position = tileTransform.position + Vector3.back;   // 타일보다 z축 -1의 위치에 배치 하여 우선 선택 이거때문에 3d 쓴거임

        GameObject tower = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);

        #region clone 제거
        // 클론 이름 제거 부분
        int index = tower.name.IndexOf("(Clone)");
        if (index > 0)
            tower.name = tower.name.Substring(0, index);
        #endregion
         
        tower.transform.parent = gameObject.transform;  // Instantiate 한 것을 따로 변수에 저장한 후에 부모 설정해야 오류 안남

        isOnTowerButton = false;

        tile.IsBuildTower = true;

        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;


        tower.GetComponent<TowerController>().Setup(this, enemySpawner, playerGold, tile);  // 타워에 enemySpawner 정보 전달
        OnBuffAllBuffTowers();  // 버프 갱신

        Destroy(followTowerClone);

        StopCoroutine("OnTowerCancelSystem");
        towerList.Add(tower.GetComponent<TowerController>());   // 타워리스트에 타워 추가
        TowerController towerObj = tower.GetComponent<TowerController>();
        towerObj.MultiplyAbility(WaveSystem.Type, WaveSystem.Ratio);
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) // esc 혹은 마우스 오른쪽 버튼
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }


            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()   // 버프 타워보다 나중에 배치된 타워들도 버프 주기 위함
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i =0; i < towers.Length; i++)
        {
            TowerController tower = towers[i].GetComponent<TowerController>();

            if (tower.WeaponType == Define.WeaponType.Buff)
                tower.OnBuffAroundTower();  // 버프 갱신
        }
    }
}
