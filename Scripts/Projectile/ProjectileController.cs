using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    
    public void Setup(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
    }


    void Update()
    {
        if(target != null)
        {
            // target ��ġ�� �߻�ü �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;
        if (collision.transform != target)  // target�� �ƴ� ���� �������ٰ� �´��� ������ �� ����
            return;

        collision.GetComponent<EnemyHP>().OnDamage(damage);
        Destroy(gameObject);    // 3�� ���󰡴µ� ù�߿� ���� ������ �ڿ� 2���� �ٷ� destory��
    }
}
