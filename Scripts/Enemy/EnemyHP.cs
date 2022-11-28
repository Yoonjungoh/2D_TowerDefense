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
    private IEnumerator HitAlphaAnimation() // enemy ���� ����
    {
        Color color = sprite.color;

        // ���� �� ǥ�� -> ���� �����Ǹ� �ִϸ��̼� �־ �ǰ�  TODO
        color.a = 0.4f;
        sprite.color = color;

        yield return new WaitForSeconds(0.05f); // 0.05�� ����� ���� 100���� ����

        color.a = 1.0f;
        sprite.color = color;
    }
}
