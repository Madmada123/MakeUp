using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BrushDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки")]
    public RectTransform[] paletteRects;     // Палитра цветов (UI элементы)
    public RectTransform faceZoneRect;       // Зона лица (UI элемент)
    public GameObject[] faceVariants;        // 9 лиц, изначально все выключены

    private int selectedIndex = -1;          // Выбранный цвет
    private Vector3 startLocalPosition;      // Для возврата кисти

    public void OnBeginDrag(PointerEventData eventData)
    {
        selectedIndex = -1;
        startLocalPosition = transform.localPosition; // Сохраняем стартовую позицию кисти
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Переводим позицию из экрана в локальную внутри канваса
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        transform.localPosition = localPoint;

        // Выбор цвета, если ещё не выбран
        if (selectedIndex == -1)
        {
            for (int i = 0; i < paletteRects.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(paletteRects[i], eventData.position, eventData.pressEventCamera))
                {
                    selectedIndex = i;
                    Debug.Log($"🎨 Цвет {i} выбран");
                    break;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (selectedIndex == -1)
        {
            Debug.Log("❗ Цвет не выбран — возврат кисти");
            StartCoroutine(ReturnWithBounce());
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(faceZoneRect, eventData.position, eventData.pressEventCamera))
        {
            Debug.Log("✅ Успешно покрасили лицо");

            for (int i = 0; i < faceVariants.Length; i++)
                faceVariants[i].SetActive(i == selectedIndex);
        }
        else
        {
            Debug.Log("❌ Не попали по лицу");
        }

        StartCoroutine(ReturnWithBounce());
    }

    private IEnumerator ReturnWithBounce()
    {
        Vector3 target = startLocalPosition;
        float t = 0;
        Vector3 start = transform.localPosition;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            float bounce = Mathf.Sin(t * Mathf.PI) * 10f; // немного прыжка
            transform.localPosition = Vector3.Lerp(start, target, t) + Vector3.up * bounce;
            yield return null;
        }

        transform.localPosition = target;
    }
}
