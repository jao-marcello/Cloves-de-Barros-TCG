using UnityEngine;

[CreateAssetMenu(fileName = "NovaCarta", menuName = "TGC/Carta")]
public class CartaSO : ScriptableObject
{
    [Header("Informações Básicas")]
    public string idCarta;
    public string nome;
    [TextArea(3, 10)]
    public string descricao;
    public Sprite arte;

    [Header("Valores de Jogo")]
    public int custo;
    public int valorEfeito; // Pode ser dano, cura, defesa, etc.
    public TipoEfeito tipoEfeito;

    [Header("Progressão")]
    public int nivelEvolucao = 1;
}

public enum TipoEfeito
{
    Ataque,
    Defesa,
    Cura,
    EfeitoEspecial // Para buffs/debuffs complexos futuramente
}
