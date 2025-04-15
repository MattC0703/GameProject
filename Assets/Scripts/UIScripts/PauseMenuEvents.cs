using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class PauseMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _resume;
    private Button _unstuck;
    private Button _options;

    public GameController gameController;
    public PlayerCheckpoints playerCheckpoints;

    void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _resume = _document.rootVisualElement.Q("Resume") as Button;
        _resume.RegisterCallback<ClickEvent>(onResumeClick);

        _unstuck = _document.rootVisualElement.Q("Unstuck") as Button;
        _unstuck.RegisterCallback<ClickEvent>(onUnstuckClick);

        _options = _document.rootVisualElement.Q("Options") as Button;

        Debug.Log("pause menu has been enabled!");
    }

    void OnDisable()
    {
        _resume.UnregisterCallback<ClickEvent>(onResumeClick);
        _unstuck.UnregisterCallback<ClickEvent>(onUnstuckClick);
    }

    void onResumeClick(ClickEvent click){
        GameController.isPaused = false;
    }

    void onUnstuckClick(ClickEvent click){
        GameController.isPaused = false;
        Debug.Log(playerCheckpoints.getLastCheckpoint());
        playerCheckpoints.TeleportToCheckpoint();
    }
}