using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NovaCarta", menuName = "TGC/Carta")]
public class CartaSO : ScriptableObject
{
    [Header("Informações Básicas")]
    public string idCarta; //[cite: 2]
    public string nome; //[cite: 2]
    [TextArea(3, 10)]
    public string descricao;
    public Sprite arte;

    [Header("Custos")]
    public int custoMana;
    public int custoSangue;

    [Header("Status de Combate")]
    public int atPoints; // Pontos de Ataque[cite: 2]
    public int lfPoints; // Pontos de Vida (Life)[cite: 2]

    [Header("Mecânicas")]
    public bool isSpecial; //[cite: 2]
    public int levelInvoc; // 1 a 10[cite: 2]
    public int levelEvolution; // 1 a 5[cite: 2]
    public string specialization; // Opcional[cite: 2]

    [Header("Habilidades Especiais")]
    public List<CardAbility> abilities;
}