using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float destroyTime = 1f;

    private TextMeshPro textMesh;
    private Color textColor;

    public void SetDamageText(int damageAmount, bool isCritical = false)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        textMesh.text = damageAmount.ToString();

        // Define cor baseada no tipo de dano (crítico = vermelho mais forte)
        if (isCritical)
        {
            textMesh.fontSize += 2;
            textMesh.color = new Color(1f, 0.2f, 0.2f, 1f);
        }
        else
        {
            // Quanto maior o dano, mais vermelho intenso
            float intensity = Mathf.Clamp(damageAmount / 20f, 0.5f, 1f);
            textMesh.color = new Color(1f, 1f - intensity, 1f - intensity, 1f);
        }

        textColor = textMesh.color;
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            textMesh = gameObject.AddComponent<TextMeshPro>();
        }

        textColor = textMesh.color;
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Move o texto para cima
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Aplica fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;
    }

    public static DamageDisplay Create(Vector3 position, int damageAmount, bool isCritical = false)
    {
        try
        {
            // Cria um texto simples (abordagem direta sem tentar usar o manager)
            GameObject damageTextObj = new GameObject("DamageText");
            damageTextObj.transform.position = position;

            // Configurar componentes
            DamageDisplay display = damageTextObj.AddComponent<DamageDisplay>();
            TextMeshPro textMesh = damageTextObj.AddComponent<TextMeshPro>();

            if (textMesh != null)
            {
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.fontSize = 5;
                textMesh.isOrthographic = true;

                // Definir o dano
                display.SetDamageText(damageAmount, isCritical);
                return display;
            }
            return null;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao criar display de dano: " + e.Message);
            return null;
        }
    }
}
