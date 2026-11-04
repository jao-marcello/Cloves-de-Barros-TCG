using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public bool isPlayerSlot; 
    public bool isOccupied = false;
    public GameObject cardInSlot;

    public void OccupySlot(GameObject newCard)
    {
        isOccupied = true;
        cardInSlot = newCard;

        // NOVO: Quando o slot recebe uma carta, ele procura o script de batalha dela
        // e avisa: "Eu sou o seu slot atual!"
        CardBattler battler = newCard.GetComponentInChildren<CardBattler>();
        if (battler != null)
        {
            battler.SetSlot(this);
        }
    }
}