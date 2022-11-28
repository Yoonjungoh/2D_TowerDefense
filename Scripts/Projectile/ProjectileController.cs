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
            // target 위치로 발사체 이동
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
        if (collision.transform != target)  // target이 아닌 적이 지나가다가 맞더라도 데미지 안 받음
            return;

        collision.GetComponent<EnemyHP>().OnDamage(damage);
        Destroy(gameObject);    // 3발 날라가는데 첫발에 적이 죽으면 뒤에 2발은 바로 destory됨
    }
}
