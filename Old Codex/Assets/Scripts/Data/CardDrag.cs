using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 posicaoInicial;
    private Transform parentOriginal;
    private Canvas canvasPrincipal;
    private RectTransform rectTransform;

    [Header("Instanciação na Mesa")]
    public GameObject prefabCartaMesa; // O prefab 3D que criamos no Passo 1

    void Start()
    {
        canvasPrincipal = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posicaoInicial = rectTransform.position;
        parentOriginal = transform.parent;

        transform.SetParent(canvasPrincipal.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasPrincipal.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 posicaoGlobal))
        {
            rectTransform.position = posicaoGlobal;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // SE NÃO FOR O TURNO DO JOGADOR, ELE NÃO PODE JOGAR CARTAS!
        if (!BattleManager.Instance.turnoDoJogador)
        {
            rectTransform.position = posicaoInicial;
            transform.SetParent(parentOriginal);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Slot")))
        {
            BoardSlot slot = hit.collider.GetComponent<BoardSlot>();

            if (slot != null && !slot.isOccupied && slot.isPlayerSlot)
            {
                CardDisplay displayMao = GetComponent<CardDisplay>();
                
                // Pegamos os DOIS custos da carta
                int custoSangue = displayMao.cartaDados.custoSangue;
                int custoMana = displayMao.cartaDados.custoMana;

                // Verifica se temos recursos suficientes para os dois!
                if (BattleManager.Instance.sangueAtual >= custoSangue && BattleManager.Instance.manaAtual >= custoMana)
                {
                    // Paga os custos
                    if (custoSangue > 0) BattleManager.Instance.GastarSangue(custoSangue);
                    if (custoMana > 0) BattleManager.Instance.GastarMana(custoMana);

                    // Instancia a carta
                    Vector3 posicaoSpawn = hit.collider.transform.position + new Vector3(0, 0.1f, 0);
                    GameObject cartaNaMesa = Instantiate(prefabCartaMesa, posicaoSpawn, hit.collider.transform.rotation);
                    
                    CardDisplay displayMesa = cartaNaMesa.GetComponentInChildren<CardDisplay>();
                    if (displayMao != null && displayMesa != null)
                    {
                        displayMesa.CarregarCarta(displayMao.cartaDados);
                    }

                    slot.OccupySlot(cartaNaMesa);
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log($"Recursos Insuficientes! Sangue: {BattleManager.Instance.sangueAtual}/{custoSangue} | Mana: {BattleManager.Instance.manaAtual}/{custoMana}");
                    rectTransform.position = posicaoInicial;
                    transform.SetParent(parentOriginal);
                }
            }
            else
            {
                rectTransform.position = posicaoInicial;
                transform.SetParent(parentOriginal);
            }
        }
        else
        {
            rectTransform.position = posicaoInicial;
            transform.SetParent(parentOriginal);
        }
    }
}