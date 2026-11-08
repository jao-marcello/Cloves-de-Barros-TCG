using System.Collections.Generic;
using UnityEngine;
using TMPro; // Para a UI de Sangue

public class BattleManager : MonoBehaviour
{
    // Padrão Singleton para acesso fácil de outros scripts
    public static BattleManager Instance;

    [Header("Recursos do Turno")]
    public int sangueAtual = 0;
    public bool modoSacrificioAtivo = false;
    public TextMeshProUGUI textoSangueUI; // Arraste um texto da UI para cá

    [Header("Board Configuration")]
    public List<BoardSlot> playerSlots; 
    public List<BoardSlot> enemySlots;

    void Awake()
    {
        Instance = this; // Inicializa o Singleton
    }

    // --- FUNÇÕES DE RECURSOS ---
    public void AdicionarSangue(int quantidade)
    {
        sangueAtual += quantidade;
        AtualizarUI();
    }

    public void GastarSangue(int quantidade)
    {
        sangueAtual -= quantidade;
        AtualizarUI();
    }

    private void AtualizarUI()
    {
        if (textoSangueUI != null)
        {
            textoSangueUI.text = "Sangue: " + sangueAtual;
        }
    }

    // --- CONECTE ESTA FUNÇÃO AO SEU NOVO BOTÃO DE SACRIFÍCIO (A FACA) ---
    public void ToggleModoSacrificio()
    {
        modoSacrificioAtivo = !modoSacrificioAtivo;
        Debug.Log("Modo Sacrifício Ativo: " + modoSacrificioAtivo);
        // Opcional: Aqui você pode mudar a cor do botão ou do cursor do mouse!
    }

    public void TestPlayerCombat()
    {
        Debug.Log("--- INICIANDO FASE DE COMBATE DO JOGADOR ---");

        if (playerSlots.Count == 0)
        {
            Debug.LogError("ERRO: A lista Player Slots no BattleManager está vazia!");
            return;
        }

        for (int i = 0; i < playerSlots.Count; i++)
        {
            BoardSlot pSlot = playerSlots[i];
            BoardSlot eSlot = enemySlots[i];

            Debug.Log($"Analisando Slot do Jogador {i} ({pSlot.gameObject.name}). Está ocupado? {pSlot.isOccupied}");

            if (pSlot.isOccupied && pSlot.cardInSlot != null)
            {
                CardBattler attacker = pSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                
                if (attacker != null)
                {
                    Debug.Log($"Carta {attacker.baseData.name} pronta para atacar do slot {i}!");

                    if (eSlot.isOccupied && eSlot.cardInSlot != null)
                    {
                        CardBattler defender = eSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                        
                        if (defender != null)
                        {
                            Debug.Log($"{attacker.baseData.name} ataca {defender.baseData.name}!");
                            defender.TakeDamage(attacker.currentAttack);
                        }
                        else
                        {
                            Debug.LogError($"O slot inimigo {i} tem uma carta, mas ela não tem o script CardBattler!");
                        }
                    }
                    else
                    {
                        Debug.Log($"{attacker.baseData.name} ataca o Inimigo DIRETAMENTE! (Dano: {attacker.currentAttack})");
                    }
                }
                else
                {
                    Debug.LogError($"ERRO: A carta no slot {i} não possui o script CardBattler anexado nela!");
                }
            }
        }
    }

    // Método para testar o ataque do inimigo
    public void TestEnemyCombat()
    {
        Debug.Log("--- INICIANDO FASE DE COMBATE DO INIMIGO ---");

        if (enemySlots.Count == 0) return;

        // Passa por todos os slots do inimigo
        for (int i = 0; i < enemySlots.Count; i++)
        {
            BoardSlot eSlot = enemySlots[i];
            BoardSlot pSlot = playerSlots[i]; // Pega o slot do jogador exatamente em frente

            // Verifica se o INIMIGO tem uma carta neste slot
            if (eSlot.isOccupied && eSlot.cardInSlot != null)
            {
                CardBattler attacker = eSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                
                if (attacker != null)
                {
                    Debug.Log($"Carta inimiga {attacker.baseData.name} pronta para atacar do slot {i}!");

                    // Verifica se TEM um jogador na frente para receber o golpe
                    if (pSlot.isOccupied && pSlot.cardInSlot != null)
                    {
                        CardBattler defender = pSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                        
                        if (defender != null)
                        {
                            Debug.Log($"{attacker.baseData.name} (Inimigo) ataca {defender.baseData.name} (Jogador)!");
                            defender.TakeDamage(attacker.currentAttack);
                        }
                    }
                    else
                    {
                        // Não tem carta do jogador na frente. Atacará a Vida do Jogador!
                        Debug.Log($"{attacker.baseData.name} ataca o JOGADOR DIRETAMENTE! (Dano: {attacker.currentAttack})");
                    }
                }
            }
        }
    }
}