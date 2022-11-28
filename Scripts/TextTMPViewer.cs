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

    void Update()   // 항상 업데이트가 아니라 체력이 깎일때만 호출하게 하든가 하는게 효율적일듯 TODO
    {
        textPlayerHP.text = playerHP.CurrentHP + " / " + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + " / " + waveSystem.MaxWave.ToString();
        textEnemyCount.text = enemySpawner.CurrentEnemyCount.ToString() + " / " + enemySpawner.MaxEnemyCount.ToString();
    }
}
 