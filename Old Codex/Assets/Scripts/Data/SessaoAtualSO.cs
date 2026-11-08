using UnityEngine;

[CreateAssetMenu(fileName = "NovaSessao", menuName = "TGC/Sessao Atual")]
public class SessaoAtualSO : ScriptableObject
{
    [Header("Dados da Campanha")]
    public DeckClassSO deckEscolhido; // O deck que o jogador escolheu no menu
    
    // No futuro, você pode colocar aqui:
    // public int vidaDoJogador;
    // public int faseAtual;
}