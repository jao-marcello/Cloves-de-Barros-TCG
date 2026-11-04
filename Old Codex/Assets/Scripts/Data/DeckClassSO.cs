using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDeckClass", menuName = "TGC/Deck Class")]
public class DeckClassSO : ScriptableObject
{
    public string className; // Ex: "Cultists", "Investigators"
    public List<CartaSO> classCards; // Mantive CartaSO caso você ainda não tenha renomeado esse arquivo
}