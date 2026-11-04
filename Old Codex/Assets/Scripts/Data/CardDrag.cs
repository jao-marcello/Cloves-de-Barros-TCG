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
            // 1. Acessa o script do Slot usando o novo nome (BoardSlot)
            BoardSlot slot = hit.collider.GetComponent<BoardSlot>();

            // 2. Verifica se o slot existe e se está livre usando a nova variável (isOccupied)
            if (slot != null && !slot.isOccupied && slot.isPlayerSlot)
            {
                Vector3 posicaoSpawn = hit.collider.transform.position + new Vector3(0, 0.1f, 0);

                // 3. Instancia a carta 3D
                GameObject cartaNaMesa = Instantiate(prefabCartaMesa, posicaoSpawn, hit.collider.transform.rotation);

                // 4. Transfere os dados
                CardDisplay displayMao = GetComponent<CardDisplay>();
                CardDisplay displayMesa = cartaNaMesa.GetComponentInChildren<CardDisplay>();

                if (displayMao != null && displayMesa != null)
                {
                    displayMesa.CarregarCarta(displayMao.cartaDados);
                }

                // 5. Avisa ao slot que ele agora está ocupado usando o novo método (OccupySlot)
                slot.OccupySlot(cartaNaMesa);

                // 6. Desativa a carta da mão
                gameObject.SetActive(false);
            }
            else
            {
                // Se o slot estiver ocupado, volta para a mão
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