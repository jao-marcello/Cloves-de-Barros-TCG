using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GrimorioManager : MonoBehaviour
{
    [Header("Configuração da Classe")]
    public DeckClassSO classeDoJogador; // O arquivo da classe (Ex: Classe_MagoDeSangue)

    [Header("Listas de Cartas")]
    public List<CartaSO> deckDeBatalha; // Começa vazia no Inspector
    public List<CartaSO> maoAtual;      
    public List<CartaSO> descarte;      

    [Header("Interface do Livro")]
    public Transform localDasPaginas;   
    public GameObject prefabPagina2D;   

    [Header("UI do Baralho")]
    public TextMeshProUGUI contadorCartasText;

    // Função que atualiza o número na tela
    public void AtualizarContadorUI()
    {
        if (contadorCartasText != null)
        {
            contadorCartasText.text = deckDeBatalha.Count.ToString();
        }
    }

    void Start()
    {
        // 1. O jogo pega as cartas do arquivo de Classe e joga pro seu baralho ativo
        if (classeDoJogador != null)
        {
            deckDeBatalha = new List<CartaSO>(classeDoJogador.classCards); // ou cartasDaClasse
        }

        // 2. Embaralha as cartas e depois compra 3 iniciais
        EmbaralharDeck();
        ComprarCartasIniciais(3);
        AtualizarContadorUI();
    }

    // ==========================================
    // MÉTODO FALTANDO QUE CAUSAVA O ERRO:
    // ==========================================
    public void EmbaralharDeck()
    {
        for (int i = 0; i < deckDeBatalha.Count; i++)
        {
            CartaSO temp = deckDeBatalha[i];
            int randomIndex = Random.Range(i, deckDeBatalha.Count);
            deckDeBatalha[i] = deckDeBatalha[randomIndex];
            deckDeBatalha[randomIndex] = temp;
        }
    }

    // Chamado na "Fase de Compra" do seu fluxograma
    public void ComprarCartasIniciais(int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
        {
            if (deckDeBatalha.Count > 0)
            {
                // Como o deck já foi embaralhado, pegamos sempre a carta do topo (posição 0)
                CartaSO cartaComprada = deckDeBatalha[0];

                // Remove a carta do topo do baralho
                deckDeBatalha.RemoveAt(0);

                // Adiciona na mão e renderiza a página
                maoAtual.Add(cartaComprada);
                AdicionarPaginaNoLivro(cartaComprada);
                AtualizarContadorUI();
            }
        }
    }

    // Método público para ser conectado a um botão na Unity
    public void BotaoComprarUmaCarta()
    {
        // Chama a função de compra pedindo 1 carta
        ComprarCartasIniciais(1);
    }

    private void AdicionarPaginaNoLivro(CartaSO dadosDaCarta)
    {
        // Instancia o prefab da carta 2D dentro do Canvas da Mão
        GameObject novaPagina = Instantiate(prefabPagina2D, localDasPaginas);

        // Passa os dados para o visual
        CardDisplay display = novaPagina.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.CarregarCarta(dadosDaCarta);
        }
    }

    // Função para chamar quando jogar a carta na mesa
    public void JogarCartaDaMao(CartaSO cartaJogada)
    {
        maoAtual.Remove(cartaJogada);
        // Aqui você pode adicionar lógica de ir pro descarte, etc.
    }
}