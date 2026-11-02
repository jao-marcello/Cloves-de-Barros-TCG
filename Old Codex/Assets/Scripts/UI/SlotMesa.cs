using UnityEngine;

public class SlotMesa : MonoBehaviour
{
    public bool estaOcupado = false;
    public GameObject cartaNoSlot;

    // Função para registrar que a carta caiu aqui
    public void OcuparSlot(GameObject novaCarta)
    {
        estaOcupado = true;
        cartaNoSlot = novaCarta;
    }
}