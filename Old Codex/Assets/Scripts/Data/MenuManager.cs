using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Configurações do Jogo")]
    public SessaoAtualSO sessaoAtual;
    public int indiceDaCena = 1;
    public string nomeDaCenaBatalha = "SampleScene";

    [Header("Lista de Grimórios")]
    // Lista dinâmica! Você pode adicionar quantos Decks quiser no Inspector.
    public List<DeckClassSO> decksDisponiveis; 

    [Header("UI do Menu")]
    public TMP_Dropdown dropdownDecks;    // A caixinha de seleção
    public TextMeshProUGUI textoPreview;  // O texto que vai mostrar as cartas

    private DeckClassSO deckSelecionado;

    void Start()
    {
        ConfigurarDropdown();
    }

    private void ConfigurarDropdown()
    {
        if (dropdownDecks == null || decksDisponiveis.Count == 0) return;

        // Limpa opções antigas
        dropdownDecks.ClearOptions();
        
        // Cria uma lista de textos com os nomes dos decks
        List<string> nomesDecks = new List<string>();
        foreach (DeckClassSO deck in decksDisponiveis)
        {
            nomesDecks.Add(deck.name); 
        }

        // Adiciona os nomes no Dropdown da Unity
        dropdownDecks.AddOptions(nomesDecks);

        // Faz com que a Unity chame o AtualizarPreview automaticamente quando o jogador trocar de deck
        dropdownDecks.onValueChanged.AddListener(AoMudarDeck);

        // Força a atualização do primeiro deck logo que o jogo abre
        AoMudarDeck(0);
    }

    // Essa função roda toda vez que o jogador clica no Dropdown e escolhe outro deck
    public void AoMudarDeck(int index)
    {
        deckSelecionado = decksDisponiveis[index];
        AtualizarPreview();
    }

    private void AtualizarPreview()
    {
        if (textoPreview == null || deckSelecionado == null) return;

        // Monta o texto do zero
        string preview = $"<b>Grimório: {deckSelecionado.name}</b>\n";
        preview += "Cartas contidas:\n\n";
        
        // ATENÇÃO: Substitua '.cartas' pelo nome da lista que está no seu DeckClassSO!
        foreach (CartaSO carta in deckSelecionado.classCards)
        {
            // O \n no final faz pular uma linha para a próxima carta
            preview += $"- {carta.name}\n"; 
        }

        textoPreview.text = preview;
    }

    // Essa função vai direto no seu único botão de "Começar Jogo"
    public void BotaoIniciarPartida()
    {
        if (deckSelecionado != null)
        {
            sessaoAtual.deckEscolhido = deckSelecionado;
            SceneManager.LoadScene(indiceDaCena); // Carrega a cena de batalha
        }
    }
}