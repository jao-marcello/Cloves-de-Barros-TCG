using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeckSelectionManager : MonoBehaviour
{
    [Header("Dados Globais")]
    public SessaoAtualSO sessaoAtual;
    public int indiceCenaMenuPrincipal = 0; // Índice exato da cena do menu

    [Header("Lista de Grimórios Prontos")]
    public List<DeckClassSO> decksDisponiveis; 

    [Header("UI da Seleção")]
    public TMP_Dropdown dropdownDecks;
    public TextMeshProUGUI textoPreview;

    private DeckClassSO deckSelecionado;

    void Start()
    {
        ConfigurarDropdown();
    }

    private void ConfigurarDropdown()
    {
        if (dropdownDecks == null || decksDisponiveis.Count == 0) return;

        dropdownDecks.ClearOptions();
        List<string> nomesDecks = new List<string>();
        
        foreach (DeckClassSO deck in decksDisponiveis)
        {
            nomesDecks.Add(deck.name); 
        }

        dropdownDecks.AddOptions(nomesDecks);
        dropdownDecks.onValueChanged.AddListener(AoMudarDeck);
        AoMudarDeck(0);
    }

    public void AoMudarDeck(int index)
    {
        deckSelecionado = decksDisponiveis[index];
        AtualizarPreview();
    }

    private void AtualizarPreview()
    {
        if (textoPreview == null || deckSelecionado == null) return;

        string preview = $"<b>Grimório: {deckSelecionado.name}</b>\n\nCartas neste baralho:\n";
        
        // Substitua ".classCards" caso sua lista no DeckClassSO tenha outro nome
        foreach (CartaSO carta in deckSelecionado.classCards) 
        {
            preview += $"- {carta.name}\n"; 
        }

        textoPreview.text = preview;
    }

    // Conecte esta função no botão de "Confirmar e Voltar"
    public void BotaoConfirmarSelecao()
    {
        if (deckSelecionado != null)
        {
            // Salva a escolha no nosso pendrive global!
            sessaoAtual.deckEscolhido = deckSelecionado;
            
            // Volta para a cena do menu inicial
            SceneManager.LoadScene(indiceCenaMenuPrincipal
            );
        }
    }
}