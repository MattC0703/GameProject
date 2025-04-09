using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuBehaviors : MonoBehaviour
{
    private UIDocument _document;
    private Button _start;
    private Button _quit;
    private Button _options;
    private Button _load;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _start = _document.rootVisualElement.Q("Start") as Button;
        _start.RegisterCallback<ClickEvent>(onStartClick);

        _quit = _document.rootVisualElement.Q("Quit") as Button;
        _quit.RegisterCallback<ClickEvent>(onQuitClick);

        _options = _document.rootVisualElement.Q("Options") as Button;
        _options.RegisterCallback<ClickEvent>(onOptionsClick);
        
        _load = _document.rootVisualElement.Q("Load") as Button;
        _load.RegisterCallback<ClickEvent>(onLoadClick);
    }

    void onStartClick(ClickEvent click){
        Debug.Log("button pressed");
        SceneManager.LoadScene("Level1");
    }

    void onQuitClick(ClickEvent click){

    }

    void onOptionsClick(ClickEvent click){

    }

    void onLoadClick(ClickEvent click){

    }
}
