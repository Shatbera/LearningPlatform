using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageChanger : MonoBehaviour
{
    private const string LanguageKey = "selected_language";

    private void OnEnable()
    {
        LanguageEntry.LanguageSelected += ChangeLanguage;
    }

    private void OnDisable()
    {
        LanguageEntry.LanguageSelected -= ChangeLanguage;
    }
    private void Start()
    {
        string savedCode = PlayerPrefs.GetString(LanguageKey, "");
        if (!string.IsNullOrEmpty(savedCode))
        {
            StartCoroutine(SetLocale(savedCode));
        }
    }

    public void ChangeLanguage(string localeCode)
    {
        PlayerPrefs.SetString(LanguageKey, localeCode);
        PlayerPrefs.Save();
        StartCoroutine(SetLocale(localeCode));
    }

    private IEnumerator SetLocale(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }
}
