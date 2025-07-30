using UnityEngine;

public class AcneManager : MonoBehaviour
{
    [SerializeField] private GameObject acneObject;

    public void HideAcne()
    {
        if (acneObject != null)
            acneObject.SetActive(false);
    }

    private void ShowAcne() // если нужно для перезапуска
    {
        if (acneObject != null)
            acneObject.SetActive(true);
    }
}
