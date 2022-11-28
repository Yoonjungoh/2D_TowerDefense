using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerController tower;

    void Awake()
    {
        tower = GetComponentInParent<TowerController>();    // �θ� ������Ʈ �������� �Լ��� �ִ�!
    }

    private void OnTriggerEnter2D(Collider2D collision) // isTrigger �ʼ�, �ߺ� ���� ���������� �̹� slow �� �ӵ����� slow ��Ű�� �Ŷ� 0�� �Ұ���
    {
        if (!collision.CompareTag("Enemy"))
            return;

        Movement2D movement2D = collision.GetComponent<Movement2D>();

        // �̵��ӵ� = �̵��ӵ� - �̵��ӵ� * ���ӷ�
        movement2D.MoveSpeed -= movement2D.MoveSpeed * (float)tower.Slow;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
