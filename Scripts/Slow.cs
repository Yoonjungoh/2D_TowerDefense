using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerController tower;

    void Awake()
    {
        tower = GetComponentInParent<TowerController>();    // 부모 컴포넌트 가져오는 함수도 있다!
    }

    private void OnTriggerEnter2D(Collider2D collision) // isTrigger 필수, 중복 적용 가능하지만 이미 slow 된 속도에서 slow 시키는 거라 0은 불가능
    {
        if (!collision.CompareTag("Enemy"))
            return;

        Movement2D movement2D = collision.GetComponent<Movement2D>();

        // 이동속도 = 이동속도 - 이동속도 * 감속률
        movement2D.MoveSpeed -= movement2D.MoveSpeed * (float)tower.Slow;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
