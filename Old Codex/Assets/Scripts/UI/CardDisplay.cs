using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI nomeText;
    public TextMeshProUGUI descricaoText;
    public TextMeshProUGUI custoText;
    public TextMeshProUGUI valorText;
    public Image arteImage;

    [Header("Dados da Carta")]
    public CartaSO cartaDados;

    void Start()
    {
        if (cartaDados != null)
        {
            AtualizarVisual();
        }
    }

    public void AtualizarVisual()
    {
        nomeText.text = cartaDados.nome;
        descricaoText.text = cartaDados.descricao;
        custoText.text = cartaDados.custo.ToString();
        valorText.text = cartaDados.valorEfeito.ToString();
        arteImage.sprite = cartaDados.arte;

        // Opcional: Mudar cor baseada no tipo de efeito
        // Ex: Vermelho para Ataque, Azul para Defesa
    }

    // Método para carregar uma nova carta dinamicamente
    public void CarregarCarta(CartaSO novaCarta)
    {
        cartaDados = novaCarta;
        AtualizarVisual();
    }
}
