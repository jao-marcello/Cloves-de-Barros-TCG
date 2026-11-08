using UnityEngine;
using UnityEngine.SceneManagement;

public class NavegadorDeCenas : MonoBehaviour
{
    [Header("Apenas para referência rápida")]
    public int nomeMenu = 0;
    public int nomeDeck = 1;
    public int nomeBatalha = 2;

    // 1. Método genérico: você digita o nome da cena direto no Botão da Unity
    public void CarregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    // 2. Métodos diretos: se preferir apenas selecionar na lista do botão
    public void IrParaMenu()
    {
        SceneManager.LoadScene(nomeMenu);
    }

    public void IrParaDeck()
    {
        SceneManager.LoadScene(nomeDeck);
    }

    public void IrParaBatalha()
    {
        SceneManager.LoadScene(nomeBatalha);
    }
}