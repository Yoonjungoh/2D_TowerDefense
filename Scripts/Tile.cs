using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsBuildTower { get; set; }  // ���� Ÿ�� �����ǿ� �ߺ� ���� ����

    void Awake()
    {
        IsBuildTower = false;
    }
}
