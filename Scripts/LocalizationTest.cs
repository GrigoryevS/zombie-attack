using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTest : MonoBehaviour
{
    [SerializeField]
    private LocalizedText text;
    [SerializeField]
    private LozalizedDropdown dropdown; 

    public void LocalizedText()
    {
        text.Localize("NewGame_Key");
    }

    public void LocalizeDropdown()
    {
        var options = new List<string>() { "Options_Key", "Exit_Key"};
        dropdown.Localize(options);
    }
}
