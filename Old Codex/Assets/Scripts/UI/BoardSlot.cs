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
    }
}