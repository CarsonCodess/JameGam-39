using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeScreen : MonoBehaviour
{
    public static UpgradeScreen instance;
    
    [SerializeField] private GameObject upgradeScreen;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private List<UpgradeStatUI> statUIs = new List<UpgradeStatUI>();
    [SerializeField] private List<UpgradeStat> stats = new List<UpgradeStat>();
    private List<UpgradeStat> _currentStats = new List<UpgradeStat>();

    private void Awake()
    {
        instance = this;
    }

    public void DisplayUpgradeScreen()
    {
        Player.instance.SetCanMove(false);
        upgradeScreen.SetActive(true);
        for(var i = 0; i < 3; i++)
        {
            var rand = Random.Range(0, stats.Count);
            var stat = stats[rand];
            if(!_currentStats.Contains(stat))
                _currentStats.Add(stat);
        }

        for (int i = 0; i < _currentStats.Count; i++)
            statUIs[i].Initialize(_currentStats[i]);
    }

    public void HideUpgradeScreen()
    {
        upgradeScreen.SetActive(false);
        Player.instance.SetCanMove(true);
        _currentStats.Clear();
        WaveManager.instance.NewWave();
    }
}
