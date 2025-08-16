using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStats : MonoBehaviour
{
    public int population = 0;
    public int maxPopulation = 0;
    public float maxAge = -1;
    public int deaths = 0;
    public float totalLifetime = 0;


    public event Action<StatType> OnStatChanged;

    public void Start()
    {
        StatsConfiguration.Init(this);
    }

    private void OnEnable()
    {
        CreatureEventBus.OnCreatureEvent += HandleCreatureEvent;
    }

    private void OnDisable()
    {
        CreatureEventBus.OnCreatureEvent -= HandleCreatureEvent;
    }

    private void HandleCreatureEvent(CreatureEvent evt)
    {
        switch (evt.type)
        {
            case CreatureEventType.DEATH:
                HandleDeath(evt);
                break;
            case CreatureEventType.BORN:
                IncreasePopulation();
                break;
        }
    }

    public int GetPopulation()
    {
        return population;
    }

    public float GetAverageAgeAtDeath()
    {
        return totalLifetime / deaths;
    }

    private void HandleDeath(CreatureEvent evt)
    {
        deaths++;
        totalLifetime += evt.creature.age;
        DecreasePopulation();
        CalculateMaxAge(evt.creature);
        OnStatChanged?.Invoke(StatType.DeathCount);
        OnStatChanged?.Invoke(StatType.AverageAgeAtDeath);
    }

    public void DecreasePopulation()
    {
        population--;
        OnStatChanged?.Invoke(StatType.Population);
    }

    public void IncreasePopulation()
    {
        population++;
        if (population > maxPopulation)
        {
            maxPopulation = population;
        }
        OnStatChanged?.Invoke(StatType.Population);
        OnStatChanged?.Invoke(StatType.MaxPopulation);
    }

    public void CalculateMaxAge(Creature creature)
    {
        if (creature.age > maxAge)
        {
            maxAge = creature.age;
        }
        OnStatChanged?.Invoke(StatType.MaxAge);
    }

}