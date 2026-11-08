using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Dados Globais")]
    public SessaoAtualSO sessaoAtual;

    [Header("UI do Menu")]
    public TextMeshProUGUI textoBotaoDeck; // Arraste o Texto que fica DENTRO do botão de Selecionar Deck

    [Header("Navegação")]
    public int indiceDaCenaBatalha = 1;
    public int indiceDaCenaDeck = 2; // Nome da nova cena que criaremos

    void Start()
    {
        // Quando o Menu abre, ele checa se o jogador já escolheu um deck lá na outra cena
        if (sessaoAtual != null && sessaoAtual.deckEscolhido != null)
        {
            textoBotaoDeck.text = "Deck: " + sessaoAtual.deckEscolhido.name;
        }
        else
        {
            textoBotaoDeck.text = "Escolher Deck";
        }
    }

    // Função para o Botão "Jogar"
    public void BotaoJogar()
    {
        if (sessaoAtual.deckEscolhido != null)
        {
            SceneManager.LoadScene(indiceDaCenaBatalha); // Carrega a cena de batalha
        }
        else
        {
            Debug.LogWarning("O jogador tentou iniciar sem um deck! Vá escolher um primeiro.");
            // Opcional: Você pode fazer o texto piscar em vermelho aqui no futuro
        }
    }

    // Função para o Botão "Selecionar Deck"
    public void BotaoIrParaSelecao()
    {
        SceneManager.LoadScene(indiceDaCenaDeck); // Carrega a cena de seleção de deck
    }
}