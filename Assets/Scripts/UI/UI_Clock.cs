using UnityEngine;
using TMPro;
using System;
public class UI_Clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Update()
    {
        timeText.text = TimeUtils.GetFormatedDateFromSeconds(Time.time);
    }
}
