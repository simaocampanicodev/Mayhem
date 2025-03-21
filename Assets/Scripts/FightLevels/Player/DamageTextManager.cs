using UnityEngine;
using TMPro;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;
    public GameObject damageTextPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Se não tiver prefab configurado, criar um em runtime
        if (damageTextPrefab == null)
        {
            CreateDamageTextPrefab();
        }
    }

    private void CreateDamageTextPrefab()
    {
        damageTextPrefab = new GameObject("DamageTextPrefab");
        damageTextPrefab.AddComponent<RectTransform>();

        TextMeshPro textComponent = damageTextPrefab.AddComponent<TextMeshPro>();
        textComponent.fontSize = 5;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.red;

        // Adicionar o componente de display de dano
        damageTextPrefab.AddComponent<DamageDisplay>();

        // Tornar o prefab persistente entre cenas
        DontDestroyOnLoad(damageTextPrefab);
        damageTextPrefab.SetActive(false);
    }

    public DamageDisplay SpawnDamageText(Vector3 position, int damage, bool isCritical = false)
    {
        if (damageTextPrefab == null)
        {
            CreateDamageTextPrefab();
        }

        GameObject newText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        newText.SetActive(true);

        DamageDisplay display = newText.GetComponent<DamageDisplay>();
        display.SetDamageText(damage, isCritical);

        return display;
    }
}