using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] pages; // массив страниц
    private int currentPage = 0;

    public void ShowNextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
    }

    public void ShowPreviousPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }
    }
}
