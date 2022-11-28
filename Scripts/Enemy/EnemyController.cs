using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int wayPointCount;  // 이동 경로 개수
    private Transform[] wayPoints;  // 이동 경로 정보
    private int currentIndex = 0;   // 현재 목표지점 인덱스
    private Movement2D movement2D;  // 오브젝트 이동 제어
    private EnemySpawner enemySpawner;
    [SerializeField]
    private int gold = 10;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // 적 이동 경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동/목표 지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // 다음 이동 방향 설정
        NextMoveTo();

        while (true)
        {
            // 적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            // 거리 근접시 다음 이동 방향 재설정
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()   // 이동 방향 재설정
    {
        // 아직 이동한 wayPoints 남아있을 때
        if(currentIndex < wayPointCount - 1)
        {
            // enemy 위치를 정확히 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            
            // 이동 방향 설정 => 다음 목표지점(wayPoints)
            currentIndex++;

            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 wayPoints일때
        else
        {
            gold = 0;
            OnDie(Define.EnemyDestroyType.Arrive);
        }
    }
    public void OnDie(Define.EnemyDestroyType type)
    {
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}
