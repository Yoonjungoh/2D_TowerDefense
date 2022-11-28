using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerController : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Define.WeaponType weaponType;


    [Header("Canon")]
    [SerializeField]
    private GameObject projectilePrefab;


    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private LayerMask targetLayer;

    private int level = 0;
    private Define.WeaponState weaponState = Define.WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private EnemySpawner enemySpawner;
    private SpriteRenderer spriteRenderer;
    private TowerSpawner towerSpawner;
    private PlayerGold playerGold;
    private Tile ownerTile; // 현재 타워 배친된 타일

    private float addedDamage;
    private int buffLevel;  // 0이면 버프 안받음, 1~3 버프 받음

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;

    private float damage;
    //public float Damage => damage;
    public float Damage => damage;

    private float rate;
    //public float Rate => rate;
    public float Rate => rate;

    private float range;
    public float Range => range;
    //public float Range => range;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;

    private double slow;
    public double Slow => slow;
    //public float Slow => slow;

    private float buff;
    public float Buff => buff;
    //public float Buff => buff;

    public Define.WeaponType WeaponType => weaponType;
    public float AddedDamage
    {
        get => addedDamage;
        set => addedDamage = Mathf.Max(0, value);
    }
    public int BuffLevel
    {
        get => buffLevel;
        set => buffLevel = Mathf.Max(0, value);
    }

    // TowerDataViewer를 위한 변수들
    public float baseDamage;
    public float baseRate;
    public float baseRange;
    public float baseSlow;  
    public float baseBuff; 

    private void Awake()
    {
        UpdateAbility();

        UpdateDataViewerData();
    }
    public void UpdateAbility()
    {
        damage = towerTemplate.weapon[level].damage;
        rate = towerTemplate.weapon[level].rate;
        range = towerTemplate.weapon[level].range;
        slow = towerTemplate.weapon[level].slow;
        buff = towerTemplate.weapon[level].buff;
        MultiplyAbility(WaveSystem.Type, WaveSystem.Ratio);
    }
    public void UpdateDataViewerData()  // 데이터 뷰어에는 배속된 타워의 능력치가 아닌 기본 타워 능력치를 보여주기 위함
    {
        baseDamage = towerTemplate.weapon[level].damage;
        baseRate = towerTemplate.weapon[level].rate;
        baseRange = towerTemplate.weapon[level].range;
        baseSlow = towerTemplate.weapon[level].slow;
        baseBuff = towerTemplate.weapon[level].buff;
    }
    public void Setup(TowerSpawner towerSpawner, EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        if (weaponType == Define.WeaponType.Canon || weaponType == Define.WeaponType.Laser)  
            ChangeState(Define.WeaponState.SearchTarget);
    }

    public void ChangeState(Define.WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());  // 이전 코루틴 종료
    
        weaponState = newState;

        StartCoroutine(weaponState.ToString()); // 새로운 코루틴 실행
    }

    void Update()
    {
        if (attackTarget != null)
            RotateToTarget();
    }

    private void RotateToTarget()
    {
        // 각도 R = arctan(y/x)
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // 나온 단위가 라디안이기 때문에 도로 바꿔줌
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            attackTarget = FindClosestAttackTarget();

            if (attackTarget != null) 
            {
                switch (weaponType)
                {
                    case (Define.WeaponType.Canon):
                        ChangeState(Define.WeaponState.TryAttackCanon);
                        break;
                    case (Define.WeaponType.Laser):
                        ChangeState(Define.WeaponState.TryAttackLaser);
                        break;
                }
            }   

            yield return null;
        }
    }
    
    private IEnumerator TryAttackCanon()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(Define.WeaponState.SearchTarget);
                break;
            }

            // 딜레이 주기
            yield return new WaitForSeconds(Rate);

            // Projectile 생성후 공격
            SpawnProjectile();
        }
    }

    private IEnumerator TryAttackLaser()
    {
        EnableLaser();  // 레이저, 타격 효과 활성화

        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(Define.WeaponState.SearchTarget);
                break;
            }

            // 레이저 공격
            SpawnLaser();

            yield return null;
        }
    }
    public void OnBuffAroundTower() 
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i = 0; i < towers.Length; i++)
        {
            TowerController tower = towers[i].GetComponent<TowerController>();

            // 이미 버프를 받고 있거나 현재 버프타워 보다 렙 낮으면 패스
            if (tower.BuffLevel > Level)
                continue;

            // 거리 안에 있는 것들만 계산
            if (Vector3.Distance(tower.transform.position, transform.position) <= towerTemplate.weapon[level].range) 
            {
                switch (tower.WeaponType)
                {
                    case Define.WeaponType.Canon:
                        tower.AddedDamage = tower.Damage * towerTemplate.weapon[level].buff;    // 버프 주기
                        tower.buffLevel = Level;    // 버프 레벨 설정
                        break;
                    case Define.WeaponType.Laser:
                        tower.AddedDamage = tower.Damage * towerTemplate.weapon[level].buff;
                        tower.buffLevel = Level;
                        break;
                }
            }
        }
    }
    public void OnDeBuffAroundTower() // 버프 타워 없어질 때 버프 해제
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; i++)
        {
            TowerController tower = towers[i].GetComponent<TowerController>();


            // 거리 안에 있는 것들만 계산
            if (Vector3.Distance(tower.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                switch (tower.WeaponType)
                {
                    case Define.WeaponType.Canon:
                        tower.AddedDamage = 0;  
                        tower.buffLevel = 0;    
                        break;
                    case Define.WeaponType.Laser:
                        tower.AddedDamage = 0;
                        tower.buffLevel = 0;
                        break;
                }
            }
        }
    }
    private Transform FindClosestAttackTarget()
    {
        float closestDistSqr = Mathf.Infinity;  // 가장 가까운 적 위치 찾기(맵 최대 사이즈로 셋팅해도 되지만 일단 귀찮으니까 TODO)

        for (int i = 0; i < enemySpawner.EnemyList.Count; i++)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

            if (distance <= Range && distance <= closestDistSqr)   // 최소 공격 범위보단 거리가 짧아야 하므로
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }

        return attackTarget;

    }
    private bool IsPossibleToAttackTarget()
    {
        if (attackTarget == null)
            return false;

        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if(distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }
    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        projectile.transform.parent = gameObject.transform;
        projectile.GetComponent<ProjectileController>().Setup(attackTarget, damage);
    }
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }
    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        // 광선에 맞은 적들 정보
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction,
                                                  towerTemplate.weapon[level].range, targetLayer);
        for(int i= 0; i < hit.Length; i++)
        {
            if (hit[i].transform == attackTarget)
            {
                // 선 시작 지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                // 선 끝 지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // 타격 효과 위치 설정, 몬스터의 중심부가 아닌 정말로 레이저가 타격 받은 테두리 부분에 이펙트 생성
                hitEffect.position = hit[i].point;
                // 공격력 버프
                float buffDamage = damage + AddedDamage;
                // 체력 감소
                attackTarget.GetComponent<EnemyHP>().OnDamage(buffDamage * Time.deltaTime);
            }
        }
    }
    public bool Upgrade()
    {
        if (playerGold.CurrentGold < towerTemplate.weapon[level].cost)
            return false;

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite; // 타워 이미지 교체
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost; // 골드 차감

        switch (weaponType)
        {
            case (Define.WeaponType.Laser):
                lineRenderer.startWidth = 0.05f + level * 0.05f;
                lineRenderer.endWidth = 0.05f;
                break;
        }

        towerSpawner.OnBuffAllBuffTowers(); // 타워 업그레이드시 버프 뿌려줘야 함
        UpdateAbility();
        UpdateDataViewerData();
        return true;
    }
    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.IsBuildTower = false;

        if(weaponType == Define.WeaponType.Buff)
            OnDeBuffAroundTower();

        Destroy(gameObject);
    }
    public void MultiplyAbility(Define.MultiplySpeedType type, float ratio)
    {
        switch (WeaponType)
        {
            case (Define.WeaponType.Canon):
                MultiplyCanon(type, ratio);
                break;
            case (Define.WeaponType.Laser):
                MultiplyLaser(type, ratio);
                break;
            //case (Define.WeaponType.Slow):
            //    MultiplySlow(type, ratio);
            //    break;
            case (Define.WeaponType.Buff):
                MultiplyBuff(type, ratio);
                break;
        }
    }
    private void MultiplyCanon(Define.MultiplySpeedType type, float ratio)
    {
        switch (type)
        {
            case (Define.MultiplySpeedType.X1):
                rate = towerTemplate.weapon[level].rate;
                break;
            case (Define.MultiplySpeedType.X2):
                rate = towerTemplate.weapon[level].rate / ratio;
                break;
            case (Define.MultiplySpeedType.X3):
                rate = towerTemplate.weapon[level].rate / ratio;
                break;
        }

    }
    private void MultiplyLaser(Define.MultiplySpeedType type, float ratio)
    {
        switch (type)
        {
            case (Define.MultiplySpeedType.X1):
                damage = towerTemplate.weapon[level].damage;
                break;
            case (Define.MultiplySpeedType.X2):
                damage = towerTemplate.weapon[level].damage * ratio;
                break;
            case (Define.MultiplySpeedType.X3):
                damage = towerTemplate.weapon[level].damage * ratio;
                break;
        }
    }
    private void MultiplySlow(Define.MultiplySpeedType type, float ratio)   // 버그 많음 TODO
    {
        switch (type)
        {
            case (Define.MultiplySpeedType.X1):
                slow = towerTemplate.weapon[level].slow;
                break;
            case (Define.MultiplySpeedType.X2):
                slow = towerTemplate.weapon[level].slow * (ratio * 0.5);
                break;
            case (Define.MultiplySpeedType.X3):
                slow = towerTemplate.weapon[level].slow * (ratio * 0.5);
                break;
        }
    }
    private void MultiplyBuff(Define.MultiplySpeedType type, float ratio)
    {
        switch (type)
        {
            case (Define.MultiplySpeedType.X1):
                buff = towerTemplate.weapon[level].buff;
                break;
            case (Define.MultiplySpeedType.X2):
                buff = towerTemplate.weapon[level].buff * ratio;
                break;
            case (Define.MultiplySpeedType.X3):
                buff = towerTemplate.weapon[level].buff * ratio;
                break;
        }
    }
    public void IntializeAbility(Define.MultiplySpeedType type, float ratio)    // 건드려진 Rate 다시 초기화
    {
        rate = towerTemplate.weapon[level].rate;
        damage = towerTemplate.weapon[level].damage;
        slow = towerTemplate.weapon[level].slow;
        buff = towerTemplate.weapon[level].buff;

        MultiplyAbility(type, ratio);
    }
}
 