using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Gerenciadores Conectados")]
    public EnemyManager enemyManager;       
    public GrimorioManager grimorioManager; 

    [Header("Vida dos Jogadores")]
    public int vidaJogador = 15;
    public int vidaInimigo = 15;
    public TextMeshProUGUI textoVidaJogadorUI;
    public TextMeshProUGUI textoVidaInimigoUI;

    [Header("Controle de Turnos")]
    public bool turnoDoJogador = true;
    public int turnoAtual = 1;
    public bool jogoFinalizado = false; // Trava o jogo se alguém morrer

    [Header("Navegação")]
    public int indiceCenaMenu = 0;

    [Header("Recursos do Jogador")]
    public int sangueAtual = 0;
    public bool modoSacrificioAtivo = false;
    
    public int manaAtual = 1;
    public int manaMaxima = 1;
    public int limiteMana = 5;

    [Header("UI dos Recursos")]
    public TextMeshProUGUI textoSangueUI;
    public TextMeshProUGUI textoManaUI;

    [Header("Board Configuration")]
    public List<BoardSlot> playerSlots; 
    public List<BoardSlot> enemySlots;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AtualizarUI();
    }

    // --- FUNÇÕES DE RECURSOS E VIDA ---
    public void AdicionarSangue(int quantidade) { sangueAtual += quantidade; AtualizarUI(); }
    public void GastarSangue(int quantidade) { sangueAtual -= quantidade; AtualizarUI(); }
    public void GastarMana(int quantidade) { manaAtual -= quantidade; AtualizarUI(); }

    public void ToggleModoSacrificio() { modoSacrificioAtivo = !modoSacrificioAtivo; }

    private void AtualizarUI()
    {
        if (textoSangueUI != null) textoSangueUI.text = "Blood: " + sangueAtual;
        if (textoManaUI != null) textoManaUI.text = "Sanity: " + manaAtual + "/" + manaMaxima;
        if (textoVidaJogadorUI != null) textoVidaJogadorUI.text = "Vida: " + vidaJogador;
        if (textoVidaInimigoUI != null) textoVidaInimigoUI.text = "Vida Inimigo: " + vidaInimigo;
    }

    // --- SISTEMA DE DANO DIRETO E GAME OVER ---
    public void CausarDanoAoInimigo(int dano)
    {
        vidaInimigo -= dano;
        AtualizarUI();
        Debug.Log($"Inimigo tomou {dano} de dano! Vida restante: {vidaInimigo}");
        
        if (vidaInimigo <= 0) FinalizarJogo(true);
    }

    public void CausarDanoAoJogador(int dano)
    {
        vidaJogador -= dano;
        AtualizarUI();
        Debug.Log($"Jogador tomou {dano} de dano! Vida restante: {vidaJogador}");
        
        if (vidaJogador <= 0) FinalizarJogo(false);
    }

    private void FinalizarJogo(bool jogadorVenceu)
    {
        jogoFinalizado = true;
        if (jogadorVenceu)
        {
            Debug.Log("🏆 VITÓRIA! O Horror Cósmico foi derrotado (por enquanto...)");
        }
        else
        {
            Debug.Log("💀 DERROTA! Sua mente sucumbiu à loucura...");
        }

        // Chama a rotina para esperar um pouco e voltar ao menu
        StartCoroutine(TransitarParaMenu());
    }

    private IEnumerator TransitarParaMenu()
    {
        // Espera 3 segundos para o jogador ver o golpe final e ler o Console
        yield return new WaitForSeconds(3f); 
        
        // Carrega a cena do Menu Principal
        SceneManager.LoadScene(indiceCenaMenu);
    }

    // --- SISTEMA DE TURNOS ---
    public void BotaoPassarTurno()
    {
        // Se o jogo acabou ou não for seu turno, o botão não faz nada
        if (turnoDoJogador && !jogoFinalizado) 
        {
            StartCoroutine(RotinaDePassagemDeTurno());
        }
    }

    private IEnumerator RotinaDePassagemDeTurno()
    {
        turnoDoJogador = false; 

        // 1. Jogador ataca
        TestPlayerCombat();
        yield return new WaitForSeconds(1.5f); 
        if (jogoFinalizado) yield break; // Para a corrotina se o inimigo morreu aqui

        // 2. Inimigo ataca
        TestEnemyCombat();
        yield return new WaitForSeconds(1.5f); 
        if (jogoFinalizado) yield break; // Para a corrotina se o jogador morreu aqui

        // 3. Inimigo joga carta
        if (enemyManager != null) enemyManager.PlayRandomCard();
        yield return new WaitForSeconds(1f); 

        IniciarNovoTurnoJogador();
    }

    private void IniciarNovoTurnoJogador()
    {
        turnoAtual++;
        turnoDoJogador = true;

        if (manaMaxima < limiteMana) manaMaxima++;
        manaAtual = manaMaxima;

        if (grimorioManager != null) grimorioManager.ResetarCompraDoTurno();

        AtualizarUI();
    }

    // --- COMBATE ---
    public void TestPlayerCombat()
    {
        if (playerSlots.Count == 0) return;

        for (int i = 0; i < playerSlots.Count; i++)
        {
            BoardSlot pSlot = playerSlots[i];
            BoardSlot eSlot = enemySlots[i];

            if (pSlot.isOccupied && pSlot.cardInSlot != null)
            {
                CardBattler attacker = pSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                if (attacker != null && attacker.baseData != null)
                {
                    if (eSlot.isOccupied && eSlot.cardInSlot != null)
                    {
                        CardBattler defender = eSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                        if (defender != null)
                        {
                            defender.TakeDamage(attacker.currentAttack);
                        }
                    }
                    else
                    {
                        // NOVO: Aplica o dano direto na vida do inimigo!
                        CausarDanoAoInimigo(attacker.currentAttack); 
                    }
                }
            }
        }
    }

    public void TestEnemyCombat()
    {
        if (enemySlots.Count == 0) return;

        for (int i = 0; i < enemySlots.Count; i++)
        {
            BoardSlot eSlot = enemySlots[i];
            BoardSlot pSlot = playerSlots[i]; 

            if (eSlot.isOccupied && eSlot.cardInSlot != null)
            {
                CardBattler attacker = eSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                if (attacker != null && attacker.baseData != null)
                {
                    if (pSlot.isOccupied && pSlot.cardInSlot != null)
                    {
                        CardBattler defender = pSlot.cardInSlot.GetComponentInChildren<CardBattler>();
                        if (defender != null)
                        {
                            defender.TakeDamage(attacker.currentAttack);
                        }
                    }
                    else
                    {
                        // NOVO: Aplica o dano direto na vida do jogador!
                        CausarDanoAoJogador(attacker.currentAttack); 
                    }
                }
            }
        }
    }
}