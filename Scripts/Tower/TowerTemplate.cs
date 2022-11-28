using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   // Ÿ�� ���� ����ȭ -> �������� ������ ������ ���� ���۰���
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
        public float slow;  // 0.1 = 10% ����
        public float buff; // 0.1 = 10% ����
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
