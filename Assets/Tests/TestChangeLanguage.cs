using SmartLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChangeLanguage : MonoBehaviour
{
    public void SetLang(int lang)
    {
        if (lang == 0)
        {
            LanguageManager.Instance.ChangeLanguage("en");
        } else {
            LanguageManager.Instance.ChangeLanguage("es");
        }
    }
}
