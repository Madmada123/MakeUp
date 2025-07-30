using UnityEngine;

public class LipsManager : MonoBehaviour
{
    [Header("Родитель, внутри которого лежат все 6 губ")]
    [SerializeField] private Transform lipsRoot;

    private GameObject[] lips;

    private void Awake()
    {
        if (lipsRoot == null)
        {
            Debug.LogError("LipsManager: lipsRoot не назначен!");
            return;
        }

        int childCount = lipsRoot.childCount;
        lips = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            lips[i] = lipsRoot.GetChild(i).gameObject;
            lips[i].SetActive(false); // выключим все на старте
        }

        Debug.Log($"[LipsManager] Нашёл {childCount} губ.");
    }

    public void ActivateByIndex(int index)
    {
        if (lips == null || lips.Length == 0)
        {
            Debug.LogError("LipsManager: массив губ пуст!");
            return;
        }

        if (index < 0 || index >= lips.Length)
        {
            Debug.LogError($"LipsManager: индекс {index} вне диапазона (0..{lips.Length - 1})");
            return;
        }

        for (int i = 0; i < lips.Length; i++)
        {
            lips[i].SetActive(i == index);
            Debug.Log($"{(i == index ? "✅ ВКЛ" : "❌ ВЫКЛ")} {lips[i].name}");
        }

        // На всякий: поднимем сортировку, чтобы точно было видно
        var sr = lips[index].GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 100;
        }
    }
}
