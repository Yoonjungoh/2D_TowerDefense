using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int wayPointCount;  // �̵� ��� ����
    private Transform[] wayPoints;  // �̵� ��� ����
    private int currentIndex = 0;   // ���� ��ǥ���� �ε���
    private Movement2D movement2D;  // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner;
    [SerializeField]
    private int gold = 10;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // �� �̵� ��� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            // �� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            // �Ÿ� ������ ���� �̵� ���� �缳��
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()   // �̵� ���� �缳��
    {
        // ���� �̵��� wayPoints �������� ��
        if(currentIndex < wayPointCount - 1)
        {
            // enemy ��ġ�� ��Ȯ�� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            
            // �̵� ���� ���� => ���� ��ǥ����(wayPoints)
            currentIndex++;

            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ wayPoints�϶�
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
