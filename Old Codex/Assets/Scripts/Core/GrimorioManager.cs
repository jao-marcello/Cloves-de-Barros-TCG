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

    [Header("Deck Atual")]
    public DeckClassSO grimorioSelecionado;

    [Header("Dados Recebidos do Menu")]
    public SessaoAtualSO sessaoAtual; // Arraste o mesmo arquivo SessaoAtualData aqui no Inspector


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
        // Assim que a cena de batalha começa, ele verifica o que o jogador escolheu no menu
        if (sessaoAtual != null && sessaoAtual.deckEscolhido != null)
        {
            IniciarCampanha(sessaoAtual.deckEscolhido);
        }
        else
        {
            Debug.LogError("Nenhum deck foi recebido do Menu! Inicie o jogo pela cena do Menu Principal.");
        }
    }

    public void IniciarCampanha(DeckClassSO deckEscolhido)
    {
        grimorioSelecionado = deckEscolhido;

        // Limpa o baralho atual (caso o jogador esteja reiniciando a campanha)
        deckDeBatalha.Clear(); 

        // Copia as cartas do Grimório selecionado para o baralho da partida
        foreach (CartaSO carta in grimorioSelecionado.classCards)
        {
            deckDeBatalha.Add(carta);
        }

        // Embaralha as cartas (chame a sua função de embaralhar aqui)
        EmbaralharDeck(); 
        
        // Atualiza a UI do contador que fizemos no passo anterior
        AtualizarContadorUI();

        // Se você tiver uma função de comprar a mão inicial (ex: puxar 4 cartas), chame-a aqui:
        ComprarCartasIniciais(4); // Exemplo: compra 4 cartas para a mão inicial

        Debug.Log($"Campanha iniciada com o Grimório: {grimorioSelecionado.name}");
    }
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