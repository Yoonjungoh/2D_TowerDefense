using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsBuildTower { get; set; }  // 같은 타일 포지션에 중복 생성 방지

    void Awake()
    {
        IsBuildTower = false;
    }
}
