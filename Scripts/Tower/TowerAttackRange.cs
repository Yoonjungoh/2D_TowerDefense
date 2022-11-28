using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    public void OnAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);

        float diameter = range * 2.0f;  // 지름
        transform.localScale = Vector3.one * diameter;  
        transform.position = position;  // 공격범위 = 타워범위
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
