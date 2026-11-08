using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GrimorioManager : MonoBehaviour
{
    [Header("Dados Globais")]
    public SessaoAtualSO sessaoAtual; // O "pendrive" que vem do Menu

    [Header("Deck de Sacrifício (Fixo)")]
    public DeckClassSO deckSacrificioConfig; // Arraste o arquivo do Deck de Sacrifício aqui

    [Header("Baralhos da Partida (Dinâmicos)")]
    public List<CartaSO> deckDeBatalha; // Cartas principais
    private List<CartaSO> baralhoSacrificioAtual = new List<CartaSO>(); // Cartas de sacrifício
    public List<CartaSO> maoAtual;      
    public List<CartaSO> descarte;      

    [Header("Interface do Livro (Mão)")]
    public Transform localDasPaginas;   
    public GameObject prefabPagina2D;   

    [Header("UI dos Contadores")]
    public TextMeshProUGUI contadorDeckPrincipalText;
    public TextMeshProUGUI contadorDeckSacrificioText;

    void Start()
    {
        // Ao iniciar a batalha, puxa os dados do menu
        if (sessaoAtual != null && sessaoAtual.deckEscolhido != null)
        {
            IniciarCampanha(sessaoAtual.deckEscolhido);
        }
        else
        {
            Debug.LogError("Nenhum deck recebido! Inicie pela cena do Menu Principal.");
        }
    }

    public void IniciarCampanha(DeckClassSO deckEscolhido)
    {
        // 1. Prepara o Baralho Principal
        deckDeBatalha.Clear(); 
        foreach (CartaSO carta in deckEscolhido.classCards)
        {
            deckDeBatalha.Add(carta);
        }
        EmbaralharDeck(deckDeBatalha); 
        
        GarantirCartaCustoUmNoTopo();

        // 2. Prepara o Baralho de Sacrifício
        baralhoSacrificioAtual.Clear();
        if (deckSacrificioConfig != null)
        {
            foreach (CartaSO carta in deckSacrificioConfig.classCards)
            {
                baralhoSacrificioAtual.Add(carta);
            }
        }

        AtualizarContadoresUI();

        // Compra a mão inicial (Apenas do principal. Sacrifícios o jogador decide puxar depois)
        ComprarCartasIniciais(3); 
        ComprarCartaSacrificio(); // Puxa a primeira carta de sacrifício automaticamente
        
        Debug.Log($"Campanha iniciada com o Grimório: {deckEscolhido.name}");
    }

    // Função genérica que embaralha qualquer baralho que você mandar pra ela
    public void EmbaralharDeck(List<CartaSO> deckParaEmbaralhar)
    {
        for (int i = 0; i < deckParaEmbaralhar.Count; i++)
        {
            CartaSO temp = deckParaEmbaralhar[i];
            int randomIndex = Random.Range(i, deckParaEmbaralhar.Count);
            deckParaEmbaralhar[i] = deckParaEmbaralhar[randomIndex];
            deckParaEmbaralhar[randomIndex] = temp;
        }
    }

    private void GarantirCartaCustoUmNoTopo()
    {
        // Procura uma carta que custe exatamente 1 de sangue e tenha ataque maior que 0
        for (int i = 0; i < deckDeBatalha.Count; i++)
        {
            if (deckDeBatalha[i].custoSangue == 1 && deckDeBatalha[i].atPoints > 0)
            {
                // Salva a carta encontrada
                CartaSO cartaIdeal = deckDeBatalha[i];
                
                // Remove ela da posição aleatória em que caiu
                deckDeBatalha.RemoveAt(i);
                
                // Insere ela na posição 0 (o topo do baralho!)
                deckDeBatalha.Insert(0, cartaIdeal);
                
                // Interrompe o loop, pois já garantimos a carta
                return; 
            }
        }
        
        Debug.LogWarning("Nenhuma carta de custo 1 de Sangue com ataque foi encontrada neste baralho!");
    }

    public void AtualizarContadoresUI()
    {
        if (contadorDeckPrincipalText != null)
            contadorDeckPrincipalText.text = "Compras: " + deckDeBatalha.Count.ToString();
            
        if (contadorDeckSacrificioText != null)
            contadorDeckSacrificioText.text = "Compras: " + baralhoSacrificioAtual.Count.ToString();
    }

    public void ComprarCartasIniciais(int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
        {
            if (deckDeBatalha.Count > 0)
            {
                CartaSO cartaComprada = deckDeBatalha[0];
                deckDeBatalha.RemoveAt(0);
                AdicionarNaMao(cartaComprada);
            }
        }
        AtualizarContadoresUI();
    }

    // --- CONECTE ESTA FUNÇÃO NO BOTÃO DE COMPRAR CARTA DO DECK PRINCIPAL ---
    public void ComprarCartaPrincipal()
    {
        if (deckDeBatalha.Count > 0)
        {
            CartaSO cartaComprada = deckDeBatalha[0];
            deckDeBatalha.RemoveAt(0);
            AdicionarNaMao(cartaComprada);
            AtualizarContadoresUI();
        }
        else
        {
            Debug.Log("O deck principal acabou!");
        }
    }

    // --- CONECTE ESTA FUNÇÃO NO BOTÃO DE COMPRAR CARTA DE SACRIFÍCIO ---
    public void ComprarCartaSacrificio()
    {
        if (baralhoSacrificioAtual.Count > 0)
        {
            CartaSO cartaComprada = baralhoSacrificioAtual[0];
            baralhoSacrificioAtual.RemoveAt(0);
            AdicionarNaMao(cartaComprada);
            AtualizarContadoresUI();
        }
        else
        {
            Debug.Log("O deck de sacrifícios acabou!");
        }
    }

    // Lógica interna para renderizar a carta
    private void AdicionarNaMao(CartaSO carta)
    {
        maoAtual.Add(carta);
        GameObject novaPagina = Instantiate(prefabPagina2D, localDasPaginas);
        
        CardDisplay display = novaPagina.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.CarregarCarta(carta);
        }
    }

    public void JogarCartaDaMao(CartaSO cartaJogada)
    {
        maoAtual.Remove(cartaJogada);
    }
}