using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Instruction")]
    public TextMeshProUGUI instructionText;

    [Header("Instrument Info")]
    public GameObject instrumentInfoCanvas;
    public TextMeshProUGUI instrumentNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI expectedSlotText;

    [Header("Progress")]
    public GameObject progressCanvas;
    public TextMeshProUGUI progressText;

    [Header("Feedback")]
    public GameObject feedbackCanvas;
    public TextMeshProUGUI feedbackText;

    [Header("Completion")]
    public GameObject completionCanvas;
    public TextMeshProUGUI completionProgressText;

    [Header("Buttons")]
    public Button resetButton;
    public Button lightingToggleButton;
    public Button exitButton;
    public TextMeshProUGUI lightingButtonText;

    [Header("UI Audio")]
    public AudioSource uiAudioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        instructionText.text = "Place all instruments in their correct positions.";
        instrumentInfoCanvas.SetActive(false);
        completionCanvas.SetActive(false);
        progressCanvas.SetActive(true);
        feedbackCanvas.SetActive(true);
        feedbackText.text = "";
        UpdateProgress(0, 3);

        SetupButton(resetButton, () => GameManager.Instance.ResetSimulation());
        SetupButton(lightingToggleButton, () => GameManager.Instance.ToggleLighting());
        SetupButton(exitButton, () => Application.Quit());
    }

    private void SetupButton(Button button, UnityEngine.Events.UnityAction onClick)
    {
        if (button == null) return;
        button.onClick.AddListener(onClick);
        button.onClick.AddListener(() => PlayUIAudio(clickClip));
        AddHoverSound(button);
    }

    private void AddHoverSound(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()
            ?? button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener(_ => PlayUIAudio(hoverClip));
        trigger.triggers.Add(entry);
    }

    private void PlayUIAudio(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
            uiAudioSource.PlayOneShot(clip);
    }

    public void ShowInstrumentInfo(string name, string desc, string slotID)
    {
        instrumentInfoCanvas.SetActive(true);
        instrumentNameText.text = name;
        descriptionText.text = desc;
        expectedSlotText.text = "Expected Slot: Ghost " + slotID;
    }

    public void HideInstrumentInfo()
    {
        instrumentInfoCanvas.SetActive(false);
    }

    public void UpdateProgress(int placed, int total)
    {
        if (progressText != null)
            progressText.text = placed + " / " + total + " Instruments Placed";
    }

    public void ShowFeedback(string message, bool isSuccess)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = isSuccess
                ? new Color(0f, 0.9f, 0.46f)
                : new Color(1f, 0.32f, 0.32f);
        }
    }

    public void ShowCompletion()
    {
        progressCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        instrumentInfoCanvas.SetActive(false);
        completionCanvas.SetActive(true);
        completionProgressText.text = "3 / 3 Instruments Successfully Placed\nAll instruments are ready for the procedure.";
    }

    public void UpdateLightingButtonText(string modeName)
    {
        if (lightingButtonText != null)
            lightingButtonText.text = modeName;
    }

    public void ResetUI()
    {
        HideInstrumentInfo();
        progressCanvas.SetActive(true);
        feedbackCanvas.SetActive(true);
        completionCanvas.SetActive(false);
        feedbackText.text = "";
        feedbackText.color = Color.white;
        UpdateProgress(0, 3);
    }
}