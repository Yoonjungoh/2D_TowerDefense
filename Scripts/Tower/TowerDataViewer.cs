using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerController currentTower;

    void Awake()
    {
        OffPanel();
    }

    void Update()   // �̰͵� Ư�� ��Ȳ���� �ۿ��ϰ� �ϴ°� ��������..? ��� UPDATE�ϸ� ���� ����� TODO 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }
    public void OnPanel(Transform tower)   // ����Ϸ� ���� �÷��� ������ on/off ó���ϱ� TODO
    {
        currentTower = tower.GetComponent<TowerController>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }
    private void UpdateTowerData()
    {
        switch (currentTower.WeaponType)
        {
            case Define.WeaponType.Canon:
                imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
                textDamage.text = "Damage : " + currentTower.baseDamage + " + " + 
                                  "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
                break;
            case Define.WeaponType.Laser:
                imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
                textDamage.text = "Damage : " + currentTower.Damage + " + " +
                                  "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
                break;
            case Define.WeaponType.Slow:
                imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
                textDamage.text = $"Slow : {currentTower.baseSlow * 100}%";
                break;
            case Define.WeaponType.Buff:
                imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
                textDamage.text = $"Buff : {currentTower.baseBuff * 100}%";
                break;
        }

        imageTower.sprite = currentTower.TowerSprite;
        textRate.text = $"Rate : {currentTower.baseRate}";
        textRange.text = $"Range : {currentTower.baseRange}";
        textLevel.text = $"Level : {currentTower.Level}";
        textUpgradeCost.text = $"{currentTower.UpgradeCost}";
        textSellCost.text = $"{currentTower.SellCost}";

        // ���׷��̵� �Ұ��������� ��ư ��Ȱ��ȭ
        // ���� ���׷��̵尡 �ִ� �Դϴ� ��� UI ����ֱ� -> TODO
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true)
        {
            // PanelTower UI ������Ʈ
            UpdateTowerData();
            // TowerAttackRange UI ������Ʈ
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // ��尡 �����ϴ� ��� UI ����ֱ� -> TODO
            systemTextViewer.PrintText(Define.SystemMSGType.Money);

        }
    }

    public void OnClickEventTowerSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
