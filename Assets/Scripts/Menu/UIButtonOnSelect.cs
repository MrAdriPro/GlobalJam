using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonOnSelect : MonoBehaviour, ISelectHandler
{

    private MainMenuController mainMenuController;
    private RectTransform rt;

    private void Start()
    {
        mainMenuController = GameObject.FindAnyObjectByType<MainMenuController>();
        rt = GetComponent<RectTransform>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Vector2 pos = rt.anchoredPosition;
        pos.x -= rt.rect.width / 2 + 25;
        mainMenuController.ChangeMainMenuSelectorPosition(pos);

        if (mainMenuController.audioSource != null && mainMenuController.selectSound != null)
        {
            mainMenuController.audioSource.PlayOneShot(mainMenuController.selectSound);
        }
    }
}
