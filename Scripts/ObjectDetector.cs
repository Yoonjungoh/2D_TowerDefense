using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour // ���콺 Ŭ���� ó�� 
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ���콺�� UI�� �� ���� ��
        if (EventSystem.current.IsPointerOverGameObject() == true)
            return;

        if (Input.GetMouseButtonDown(0))    // ���콺 ����, ���� �浹�Ϸ��� BoxCollider �ʼ�!!!!! �̰� ���� �̳� �ظ̳� ���̵�
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D ����͸� ���� 3D ������ ������Ʈ�� ���콺 Ŭ������ �����ϴ� ���
            // hit�� ���� ���� 
            // ī�޶󿡼� �߻�� ����(�ִ����)�� ���� ������ hit�� ����
            // Physics�� ������ Collider�� 2D�� �ƴ� 3D ���

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                // ������ �ε��� ������Ʈ�� Tile�̸� Ÿ������
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }

                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);     
                    // �̰� �� ��̴µ� hit.transform�� �ε��� ���� transform�� �ƴ϶� �ε��� ������Ʈ�� transform��!!! �߿�!!
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // ���콺�� hit�� ������Ʈ�� ���ų� hit�� ������Ʈ�� tower�� �ƴϸ� �г� UI ����
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false) 
            {
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}
