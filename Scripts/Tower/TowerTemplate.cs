using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   // 타워 정보 에셋화 -> 레벨별로 나뉘는 정보를 쉽게 조작가능
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;
        public float damage;
        public float slow;  // 0.1 = 10% 감속
        public float buff; // 0.1 = 10% 증가
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
