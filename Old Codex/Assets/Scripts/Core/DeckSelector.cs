using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    public GrimorioManager grimorioManager;
    
    // Arraste os seus ScriptableObjects (Grimorios) para cá no Inspector
    public DeckClassSO deckFogo;
    public DeckClassSO deckSombrio;

    // Conecte esta função no evento OnClick do Botão do Deck de Fogo
    public void SelecionarDeckFogo()
    {
        grimorioManager.IniciarCampanha(deckFogo);
        EsconderBotoes(); // Função opcional para sumir com os botões de seleção da tela
    }

    // Conecte esta função no evento OnClick do Botão do Deck Sombrio
    public void SelecionarDeckSombrio()
    {
        grimorioManager.IniciarCampanha(deckSombrio);
        EsconderBotoes();
    }
    
    private void EsconderBotoes()
    {
        gameObject.SetActive(false);
    }
}