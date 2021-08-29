using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class LozalizedDropdown : MonoBehaviour
{

    private TMP_Dropdown dropdown;
    private List<string> keys;

    private void Start()
    {
        Localize();
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    private void Init()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        keys = new List<string>();

        foreach (var option in dropdown.options)
            keys.Add(option.text);
    }

    public void Localize(List<string> newKeys = null)
    {
        if (dropdown == null)
            Init();

        if (newKeys != null)
            keys = newKeys;

        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var key in keys)
            options.Add(new TMP_Dropdown.OptionData(LocalizationManager.GetTranslate(key)));

        dropdown.options = options;
        dropdown.RefreshShownValue();
    }
}
