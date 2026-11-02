using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI nomeText;
    public TextMeshProUGUI descricaoText;
    public TextMeshProUGUI custoManaText; // Atualizado
    public TextMeshProUGUI custoSangueText; // Atualizado
    public TextMeshProUGUI ataqueText; // Novo (antigo valorText)
    public TextMeshProUGUI vidaText; // Novo
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
        
        // Lendo os novos dados do CartaSO atualizado
        custoManaText.text = cartaDados.custoMana.ToString();
        custoSangueText.text = cartaDados.custoSangue.ToString();
        ataqueText.text = cartaDados.atPoints.ToString();
        vidaText.text = cartaDados.lfPoints.ToString();
        
        arteImage.sprite = cartaDados.arte;
    }

    public void CarregarCarta(CartaSO novaCarta)
    {
        cartaDados = novaCarta;
        AtualizarVisual();
    }
}