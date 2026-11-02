using System.Collections.Generic;
using UnityEngine;

public class GrimorioManager : MonoBehaviour
{
    [Header("Listas de Cartas")]
    public List<CartaSO> deckDeBatalha; // As cartas que você trouxe para a Jornada
    public List<CartaSO> maoAtual;      // As cartas que estão na sua mão agora
    public List<CartaSO> descarte;      // Cartas usadas ou sacrificadas

    [Header("Interface do Livro")]
    public Transform localDasPaginas;   // Onde os prefabs 2D serão gerados na UI
    public GameObject prefabPagina2D;   // O seu Card_Prefab da mão

    void Start()
    {
        // Embaralha o deck no início da partida (lógica de embaralhar omitida para simplificar)
        ComprarCartasIniciais(3); // Compra 3 cartas no turno 1
    }

    // Chamado na "Fase de Compra" do seu fluxograma
    public void ComprarCartasIniciais(int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
        {
            if (deckDeBatalha.Count > 0)
            {
                // 1. Tira a carta do topo do deck
                CartaSO cartaComprada = deckDeBatalha[0];
                deckDeBatalha.RemoveAt(0);

                // 2. Adiciona na lista da mão
                maoAtual.Add(cartaComprada);

                // 3. Cria a página (Visual) no Livro
                AdicionarPaginaNoLivro(cartaComprada);
            }
        }
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