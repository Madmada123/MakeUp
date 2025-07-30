using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LipstickDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Какой индекс губ включать (0..5)")]
    public int lipstickIndex;

    private Canvas canvas;
    private Image image;
    private Vector3 startPos;
    private LipsManager lipsManager;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        lipsManager = FindObjectOfType<LipsManager>();
    }

    private void Start()
    {
        startPos = transform.position;
        if (lipsManager == null)
            Debug.LogError("LipstickDrag: LipsManager не найден в сцене!");
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPos
        );
        transform.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"🎯 OnEndDrag у {gameObject.name}, индекс {lipstickIndex}");

        // Проверяем FaceZone
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        // после worldPos:
        Debug.Log("📍 Мировая позиция пальца: " + worldPos);
        Debug.DrawRay(worldPos, Vector3.forward * 5, Color.red, 2f);



        if (hit == null)
        {
            Debug.Log("🚫 Ни один объект не найден под пальцем");
            ReturnBack();
            return;
        }

        Debug.Log($"🎯 Найден коллайдер: {hit.name}");
        if (!hit.CompareTag("FaceZone"))
        {
            Debug.Log($"⛔ Это не FaceZone, а: {hit.tag}");
            ReturnBack();
            return;
        }

        Debug.Log("😎 FaceZone пойман — включаем нужные губы");
        if (lipsManager != null)
            lipsManager.ActivateByIndex(lipstickIndex);
        else
            Debug.LogError("❌ lipsManager = null!");

        ReturnBack();
    }

    private void ReturnBack()
    {
        transform.position = startPos;
    }
}
