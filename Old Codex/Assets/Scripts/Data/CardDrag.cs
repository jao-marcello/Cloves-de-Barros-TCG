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
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Slot")))
        {
            BoardSlot slot = hit.collider.GetComponent<BoardSlot>();

            // Verifica se o slot existe, está livre e é do jogador
            if (slot != null && !slot.isOccupied && slot.isPlayerSlot)
            {
                // NOVO: Pegamos os dados da carta da mão antes de instanciar
                CardDisplay displayMao = GetComponent<CardDisplay>();
                int custoDaCarta = displayMao.cartaDados.custoSangue;

                // NOVO: Verifica se temos sangue para pagar!
                if (BattleManager.Instance.sangueAtual >= custoDaCarta)
                {
                    // Paga o custo
                    BattleManager.Instance.GastarSangue(custoDaCarta);

                    // A partir daqui, é o seu código original de invocação
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
                    Debug.Log($"Sangue Insuficiente! Custa {custoDaCarta}, mas você tem {BattleManager.Instance.sangueAtual}");
                    // Volta para a mão
                    rectTransform.position = posicaoInicial;
                    transform.SetParent(parentOriginal);
                }
            }
            else
            {
                // Slot ocupado ou inválido
                rectTransform.position = posicaoInicial;
                transform.SetParent(parentOriginal);
            }
        }
        else
        {
            // Soltou fora do tabuleiro
            rectTransform.position = posicaoInicial;
            transform.SetParent(parentOriginal);
        }
    }
}