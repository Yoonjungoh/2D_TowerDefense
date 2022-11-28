using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;

    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private EnemySpawner enemySpawner;

    void Update()   // �׻� ������Ʈ�� �ƴ϶� ü���� ���϶��� ȣ���ϰ� �ϵ簡 �ϴ°� ȿ�����ϵ� TODO
    {
        textPlayerHP.text = playerHP.CurrentHP + " / " + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + " / " + waveSystem.MaxWave.ToString();
        textEnemyCount.text = enemySpawner.CurrentEnemyCount.ToString() + " / " + enemySpawner.MaxEnemyCount.ToString();
    }
}
 