using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject entityInfoPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject selectionTag;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameEventBus.OnSelectedEntity += HandleSelectedEntity;
            GameEventBus.OnDeselectedEntity += HandleDeselectedEntity;
            selectionTag?.SetActive(false);
        }
        else Destroy(gameObject);
    }

    private void HandleSelectedEntity(GameEvent evt)
    {
        if (evt.data is Entity entity)
        {
            ShowEntityInfo(entity);
            SelectionIndicator indicator = (SelectionIndicator)selectionTag.GetComponent<SelectionIndicator>();
            if (indicator == null)
            {
                Debug.LogWarning("SelectionIndicator component not found on Selection Tag.");
            }
            indicator?.SetSelectedEntity(entity);
            selectionTag?.SetActive(true);
        }
    }

    private void HandleDeselectedEntity(GameEvent evt)
    {
        HideEntityInfo();
        selectionTag.SetActive(false);
    }
    
    public void ShowEntityInfo(Entity entity)
    {
        if (entity == null) return;
        entityInfoPanel.SetActive(true);
    }

    public void HideEntityInfo()
    {
        entityInfoPanel.SetActive(false);
    }

    public void ShowStatsPanel()
    {
        statsPanel.SetActive(true);
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }
    
}