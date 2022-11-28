using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

   void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(Define.SystemMSGType type)
    {
        switch (type)
        {
            case Define.SystemMSGType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case Define.SystemMSGType.Build:
                textSystem.text = "System : Invalid build tower...";
                break;
        }

        tmpAlpha.FadeOut();
    }
}
