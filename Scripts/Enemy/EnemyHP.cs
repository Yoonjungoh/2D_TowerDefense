using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;
    private EnemyController enemy;
    private SpriteRenderer sprite;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<EnemyController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnDamage(float damage)
    {
        if (isDie == true)
            return;

        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if(currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(Define.EnemyDestroyType.Kill);
        }
    }
    private IEnumerator HitAlphaAnimation() // enemy 투명도 제어
    {
        Color color = sprite.color;

        // 맞은 걸 표시 -> 따로 여유되면 애니메이션 넣어도 되고  TODO
        color.a = 0.4f;
        sprite.color = color;

        yield return new WaitForSeconds(0.05f); // 0.05초 대기후 투명도 100으로 변경

        color.a = 1.0f;
        sprite.color = color;
    }
}
