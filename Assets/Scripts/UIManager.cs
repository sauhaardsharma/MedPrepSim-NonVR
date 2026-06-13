using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Set instruction text
        instructionText.text = "Place all instruments in their correct positions.";

        // Hide by default
        instrumentInfoCanvas.SetActive(false);
        completionCanvas.SetActive(false);

        // Show by default
        progressCanvas.SetActive(true);
        feedbackCanvas.SetActive(true);

        // Clear feedback
        feedbackText.text = "";

        // Initial progress
        UpdateProgress(0, 3);

        // Button listeners
        resetButton.onClick.AddListener(() => GameManager.Instance.ResetSimulation());
        lightingToggleButton.onClick.AddListener(() => GameManager.Instance.ToggleLighting());
        exitButton.onClick.AddListener(() => Application.Quit());
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
                ? new Color(0f, 0.9f, 0.46f)   // green #00E676
                : new Color(1f, 0.32f, 0.32f);  // red #FF5252
        }
    }

    public void ShowCompletion()
    {
        // Hide overlapping canvases
        progressCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        instrumentInfoCanvas.SetActive(false);

        // Show completion
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

        // Restore canvases
        progressCanvas.SetActive(true);
        feedbackCanvas.SetActive(true);
        completionCanvas.SetActive(false);

        // Clear feedback
        feedbackText.text = "";
        feedbackText.color = Color.white;

        UpdateProgress(0, 3);
    }
}