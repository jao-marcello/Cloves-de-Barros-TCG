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

    private void OnMouseDown()
    {
        // Só permite sacrifício se o botão da faca estiver ativo, se tiver carta, e se for do jogador
        if (BattleManager.Instance.modoSacrificioAtivo && isOccupied && isPlayerSlot)
        {
            CardBattler battler = cardInSlot.GetComponentInChildren<CardBattler>();
            if (battler != null)
            {
                battler.SerSacrificada();
                
                // Desliga o modo sacrifício automaticamente após 1 abate (Opcional, comente se quiser sacrificar várias seguidas)
                BattleManager.Instance.ToggleModoSacrificio();
            }
        }
    }
}