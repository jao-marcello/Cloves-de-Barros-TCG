using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Class Configuration")]
    public DeckClassSO enemyClass; // O baralho específico do inimigo
    private List<CartaSO> currentDeck;

    [Header("Board & Instantiation")]
    public GameObject boardCardPrefab;   // O prefab 3D da carta na mesa
    public List<BoardSlot> enemySlots;   // Os cubos/slots que pertencem ao inimigo

    void Start()
    {
        // Copia as cartas da Classe Base para o baralho ativo da partida
        if (enemyClass != null)
        {
            currentDeck = new List<CartaSO>(enemyClass.classCards);
        }
    }

    // Função chamada para o inimigo jogar uma carta aleatória
    public void PlayRandomCard()
    {
        if (currentDeck.Count == 0) return; // Sem cartas no baralho

        // 1. Busca um slot livre
        BoardSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            // 2. Sorteia uma carta
            int index = Random.Range(0, currentDeck.Count);
            CartaSO drawnCard = currentDeck[index];

            // 3. Posiciona e Instancia
            Vector3 spawnPosition = emptySlot.transform.position + new Vector3(0, 0.1f, 0);

            // Rotação de 180 graus no eixo Y para a carta ficar de frente para o jogador
            // Usa exatamente a mesma rotação do cubo, sem girar 180 graus
            GameObject cardOnBoard = Instantiate(boardCardPrefab, spawnPosition, emptySlot.transform.rotation);

            // 4. Carrega os dados na carta instanciada
            CardDisplay display = cardOnBoard.GetComponentInChildren<CardDisplay>();
            if (display != null)
            {
                // Se quiser, no futuro renomeie CarregarCarta para LoadCard no CardDisplay.cs
                display.CarregarCarta(drawnCard);
            }

            // 5. Ocupa o slot
            emptySlot.OccupySlot(cardOnBoard);

            // 6. Remove do baralho atual
            currentDeck.RemoveAt(index);
        }
        else
        {
            Debug.Log("Enemy tried to play, but all slots are full!");
        }
    }

    // Retorna o primeiro slot vazio encontrado
    private BoardSlot GetEmptySlot()
    {
        foreach (BoardSlot slot in enemySlots)
        {
            if (!slot.isOccupied)
                return slot;
        }
        return null;
    }
}