using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Stats : MonoBehaviour
{
    public Transform statPanel;
    public Transform statTemplate;
    public GameStats gameStats;
    private Dictionary<StatType, StatSlot> statSlots;

    private void Awake()
    {
        statSlots = new Dictionary<StatType, StatSlot>();
    }

    void Start()
    {
        if (gameStats != null)
        {
            gameStats.OnStatChanged += InformChange;

            foreach (var statConfig in StatsConfiguration.Configs)
            {
                CreateStat(statConfig);
            }
        }
    }

    public void InformChange(StatType statName)
    {
        if (statSlots != null)
        {
            StatSlot stat = statSlots[statName];
            stat?.SetValue(stat.updateFunction());
        }
    }

    private void OnDestroy()
    {
        if (gameStats != null)
            gameStats.OnStatChanged -= InformChange;
    }

    private void CreateStat(StatConfig config)
    {
        StatSlot statSlot = Instantiate(statTemplate, statPanel).GetComponent<StatSlot>();
        statSlot.SetLabel(config.label);
        statSlot.SetValue(config.getValue());
        statSlot.updateFunction = config.getValue;
        statSlots.Add(config.type, statSlot);
    }
}
