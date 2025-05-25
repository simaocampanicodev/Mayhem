using UnityEngine;

public class MarioCabineController : MonoBehaviour
{
    private Animator animator;
    
    private static readonly int EntregandoParam = Animator.StringToHash("Entregando");
    private static readonly int ProntoParam = Animator.StringToHash("Pronto");
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado na CabineDoMario");
        }
    }
    
    public void StartDelivery()
    {
        if (animator != null)
        {
            animator.SetBool(EntregandoParam, true);
            animator.SetBool(ProntoParam, false);
        }
    }
    
    public void FinishDelivery()
    {
        if (animator != null)
        {
            animator.SetBool(EntregandoParam, false);
            animator.SetBool(ProntoParam, true);
        }
    }
    
    public void ResetToIdle()
    {
        if (animator != null)
        {
            animator.SetBool(EntregandoParam, false);
            animator.SetBool(ProntoParam, false);
        }
    }
}