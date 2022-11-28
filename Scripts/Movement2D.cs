using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f; // UI설정은 1로 함
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value); // 음수 안되게 조심
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
    public void MultiplymoveSpeed(Define.MultiplySpeedType type, float ratio)
    {
        switch (type)
        {
            case (Define.MultiplySpeedType.X1):
                moveSpeed = baseMoveSpeed;
                break;
            case (Define.MultiplySpeedType.X2):
                moveSpeed = baseMoveSpeed * (ratio + 0.8f);
                break;
            case (Define.MultiplySpeedType.X3):
                moveSpeed = baseMoveSpeed * (ratio + 1.2f);
                break;
        }
    }
}
