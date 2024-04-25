using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TMP_Text enemiesKilledText;
    [SerializeField] private TMP_Text timeText;
    private float _timer;
    private bool _stopTimer;

    private void Awake()
    {
        _timer = 0;
        instance = this;
    }

    private void Update()
    {
        if (!_stopTimer)
            _timer += Time.deltaTime;
    }

    public void ShowDeathScreen(int enemies)
    {
        deathScreen.SetActive(true);
        enemiesKilledText.text = "ENEMIES KILLED: " + enemies;
        timeText.text = "TIME: " + _timer.ToString("F") + "s";
    }
}
