using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance { get; private set; }

    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeInDuration = 1f;

    private string sceneToLoad;
    private int buildIndexToLoad = -1;
    private bool loadingByName;
    private bool isTransitioning;
    private bool sceneLoadStarted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeAnimator == null)
        {
            fadeAnimator = GetComponent<Animator>();
        }

        if (fadeAnimator == null)
        {
            Debug.LogError("SceneFadeManager needs an Animator component on the same GameObject.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        TriggerFadeIn();
    }

    public void LoadScene(string sceneName)
    {
        if (fadeAnimator == null)
        {
            Debug.LogError("SceneFadeManager cannot load a scene because its Animator is missing.");
            return;
        }

        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;
        loadingByName = true;
        sceneToLoad = sceneName;
        buildIndexToLoad = -1;
        sceneLoadStarted = false;

        Debug.Log("Starting fade-out before loading scene: " + sceneName);
        TriggerFadeOut();
        Invoke(nameof(LoadPendingScene), fadeOutDuration);
    }

    public void LoadScene(int buildIndex)
    {
        if (fadeAnimator == null)
        {
            Debug.LogError("SceneFadeManager cannot load a scene because its Animator is missing.");
            return;
        }

        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;
        loadingByName = false;
        buildIndexToLoad = buildIndex;
        sceneToLoad = string.Empty;
        sceneLoadStarted = false;

        Debug.Log("Starting fade-out before loading build index: " + buildIndex);
        TriggerFadeOut();
        Invoke(nameof(LoadPendingScene), fadeOutDuration);
    }

    private void TriggerFadeOut()
    {
        CancelInvoke(nameof(DisableFadeImage));

        if (fadeImage != null)
        {
            fadeImage.SetActive(true);
        }

        fadeAnimator.Play("FadeOut", 0, 0f);
    }

    private void TriggerFadeIn()
    {
        if (fadeImage != null)
        {
            fadeImage.SetActive(true);
        }

        fadeAnimator.Play("FadeIn", 0, 0f);
        Invoke(nameof(DisableFadeImage), fadeInDuration);
    }

    private void DisableFadeImage()
    {
        if (fadeImage != null)
        {
            fadeImage.SetActive(false);
        }
    }

    // Add this method as an Animation Event at the end of the fade-out animation.
    public void OnFadeOutComplete()
    {
        LoadPendingScene();
    }

    private void LoadPendingScene()
    {
        if (sceneLoadStarted)
        {
            return;
        }

        if (!isTransitioning)
        {
            return;
        }

        sceneLoadStarted = true;
        CancelInvoke(nameof(LoadPendingScene));
        Debug.Log("Fade-out complete. Loading the next scene.");

        if (loadingByName)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(buildIndexToLoad);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isTransitioning = false;
        TriggerFadeIn();
    }
}