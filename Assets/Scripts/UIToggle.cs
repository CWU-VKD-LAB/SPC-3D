using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    public GameObject UIButtons;
    public GameObject UIRules;

    private bool uiToggle = true;

    public void setUI()
    {
        uiToggle = !uiToggle;
        UIButtons.SetActive(uiToggle);
        UIRules.SetActive(uiToggle);
    }
}
