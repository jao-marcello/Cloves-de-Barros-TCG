using System.Collections.Generic;
using UnityEngine;

public class CardBattler : MonoBehaviour
{
    [Header("Status Atuais")]
    public CartaSO baseData;          
    public int currentHealth;         
    public int currentAttack;         
    public List<CardAbility> activeAbilities; 

    private CardDisplay display;
    private BoardSlot mySlot; 

    void Awake()
    {
        display = GetComponent<CardDisplay>();
    }

    // Método universal chamado automaticamente ou manualmente
    public void InitializeCard(CartaSO data, BoardSlot slot)
    {
        baseData = data;
        
        // NOVO: Trava de segurança. Só atualiza se o slot não for nulo!
        if (slot != null)
        {
            mySlot = slot;
        }
        
        if (baseData != null)
        {
            // Ajuste aqui para o nome exato da variável de vida e ataque no seu CartaSO se necessário
            currentHealth = baseData.lfPoints; 
            currentAttack = baseData.atPoints; 
            
            if (baseData.abilities != null)
            {
                activeAbilities = new List<CardAbility>(baseData.abilities);
            }
        }

        UpdateVisuals();
    }

    // NOVO: Se por acaso os dados já vierem preenchidos pelo CardDisplay, ele se auto-inicializa no Start!
    void Start()
    {
        if (baseData == null && display != null && display.cartaDados != null)
        {
            InitializeCard(display.cartaDados, null);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{baseData.name} tomou {damageAmount} de dano! Vida restante: {currentHealth}");
        
        UpdateVisuals();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateVisuals()
    {
        if (display != null)
        {
            display.vidaText.text = currentHealth.ToString();
            display.ataqueText.text = currentAttack.ToString();
        }
    }

    private void Die()
    {
        Debug.Log($"{baseData.name} morreu!");
        
        if (mySlot != null)
        {
            mySlot.isOccupied = false;
            mySlot.cardInSlot = null;
        }

        Destroy(gameObject);
    }

    public void SetSlot(BoardSlot slot)
    {
        mySlot = slot;
    }
}