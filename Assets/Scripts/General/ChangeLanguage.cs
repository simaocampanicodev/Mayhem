using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
    private Languages language;
    [SerializeField] private TMP_Dropdown dropdown;

    void Start()
    {
        LoadSavedLanguage();
    }

    void LoadSavedLanguage()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            string savedLang = PlayerPrefs.GetString("Language");
            System.Enum.TryParse(savedLang, out Languages parsedLanguage);
            language = parsedLanguage;
            dropdown.value = (language == Languages.English) ? 0 : 1;
        }
        else
        {
            language = Languages.English;
            dropdown.value = 0;
            PlayerPrefs.SetString("Language", language.ToString());
            PlayerPrefs.Save();
        }
    }

    public void ChangeLang()
    {
        if (dropdown.value == 0)
        {
            language = Languages.English;
        }
        else if (dropdown.value == 1)
        {
            language = Languages.Portuguese;
        }

        PlayerPrefs.SetString("Language", language.ToString());
        PlayerPrefs.Save();
    }
}