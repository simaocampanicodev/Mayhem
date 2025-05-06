using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
    private Languages language;
    [SerializeField] private TMP_Dropdown dropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeLang()
    {
        if (dropdown.value == 0)
        {
            language = Languages.English;
        }
        if (dropdown.value == 1)
        {
            language = Languages.Portuguese;
        }
        PlayerPrefs.SetString("Language", language.ToString());
    }
}
