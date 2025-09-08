using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject controlsMenu;
    void Start()
    {
        controlsMenu.SetActive(false);
    }

    public void ShowConrolsMenu()
    {
        controlsMenu.SetActive(true);
    }
    public void HideConrolsMenu()
    {
        controlsMenu.SetActive(false);
    }
}
