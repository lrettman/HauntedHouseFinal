using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{
    private float m_Demo_GameTimer = 0f;
    private bool m_Demo_GameTimerIsTicking = false;
    private Label m_Demo_GameTimerLabel;

    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public UIDocument uiDocument;
    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;

    private VisualElement m_EndScreen;
    private VisualElement m_CaughtScreen;

    void Start()
    {
        if (uiDocument != null)
        {
            m_Demo_GameTimerLabel = uiDocument.rootVisualElement.Q<Label>("TimerLabel");
            m_EndScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndScreen");
            m_CaughtScreen = uiDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");
        }
        else
        {
            Debug.LogWarning("UIDocument is not assigned on GameEnding.");
        }

        m_Demo_GameTimer = 0.0f;
        m_Demo_GameTimerIsTicking = true;
        Demo_UpdateTimerLabel();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_Demo_GameTimerIsTicking)
        {
            m_Demo_GameTimer += Time.deltaTime;
            Demo_UpdateTimerLabel();
        }

        if (m_IsPlayerAtExit)
        {
            EndLevel(m_EndScreen, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(m_CaughtScreen, true, caughtAudio);
        }
    }

    void Demo_UpdateTimerLabel()
    {
        if (m_Demo_GameTimerLabel != null)
            m_Demo_GameTimerLabel.text = m_Demo_GameTimer.ToString("0.00");
    }

    void EndLevel(VisualElement element, bool doRestart, AudioSource audioSource)
    {
        if (element == null)
            return;

        if (!m_HasAudioPlayed && audioSource != null)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        // assign opacity (if compiler complains, use new StyleFloat(m_Timer / fadeDuration))
        element.style.opacity = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Time.timeScale = 0;
                Application.Quit();
            }
        }
    }
}