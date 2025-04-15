using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeToBlack : MonoBehaviour
{
    private VisualElement _fadeOverlay;
    private UIDocument _uiDocument;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _fadeOverlay = root.Q<VisualElement>("fadeOverlay");
    }

    public IEnumerator FadeOutIn(System.Action onBlack)
    {
        // Fade to black
        Debug.Log("started coroutine");
        Debug.Log(_fadeOverlay.style.opacity);
        _fadeOverlay.style.opacity = 1;
        Debug.Log(_fadeOverlay.style.opacity);
        yield return new WaitForSeconds(0.5f);

        Debug.Log("invoked the teleport");
        onBlack?.Invoke(); // call the script backed into the coroutine

        // Fade back in
        Debug.Log("fading back in");
        _fadeOverlay.style.opacity = 0;
        yield return new WaitForSeconds(0.5f);
    }
}
