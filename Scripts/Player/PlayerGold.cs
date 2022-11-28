using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 100;

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);   // 굳이 이렇게 쓴 이유는..?
        get => currentGold;
    }
}
