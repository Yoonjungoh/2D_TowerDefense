using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour // 마우스 클릭시 처리 
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
        // 마우스가 UI에 가 있을 때
        if (EventSystem.current.IsPointerOverGameObject() == true)
            return;

        if (Input.GetMouseButtonDown(0))    // 마우스 왼쪽, 광선 충돌하려면 BoxCollider 필수!!!!! 이거 땜에 겁나 해맸네 기촌데
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스 클릭으로 선택하는 방법
            // hit로 정보 추출 
            // 카메라에서 발사된 광선(최대길이)이 때린 정보를 hit에 추출
            // Physics기 때문에 Collider도 2D가 아닌 3D 사용

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                // 광선에 부딪힌 오브젝트가 Tile이면 타워스폰
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }

                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);     
                    // 이것 좀 헤맸는데 hit.transform가 부딪힌 곳의 transform이 아니라 부딪힌 오브젝트의 transform임!!! 중요!!
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 마우스에 hit한 오브젝트가 없거나 hit한 오브젝트가 tower가 아니면 패널 UI 끄기
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false) 
            {
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}
