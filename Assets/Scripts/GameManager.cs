using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalInstruments = 3;
    private int placedCount = 0;
    private InstrumentData selectedInstrument;

    [Header("Lighting")]
    public GameObject lightingModeA;
    public GameObject lightingModeB;
    private bool isModeA = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctPlacementClip;
    public AudioClip wrongPlacementClip;
    public AudioClip completionClip;
    public AudioClip pickupClip;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateProgress(0, totalInstruments);

        lightingModeA.SetActive(true);
        lightingModeB.SetActive(false);
    }

    public void SelectInstrument(InstrumentData instrument)
    {
        if (selectedInstrument != null)
            selectedInstrument.Highlight(false);

        selectedInstrument = instrument;
        selectedInstrument.Highlight(true);

        if (audioSource != null && pickupClip != null)
            audioSource.PlayOneShot(pickupClip);

        UIManager.Instance.ShowInstrumentInfo(
            instrument.instrumentName,
            instrument.description,
            instrument.slotID
        );
    }

    public void DeselectInstrument()
    {
        if (selectedInstrument != null)
            selectedInstrument.Highlight(false);

        selectedInstrument = null;
        UIManager.Instance.HideInstrumentInfo();
    }

    public void DragSelectedInstrument(Vector3 targetPoint)
    {
        if (selectedInstrument == null) return;
        selectedInstrument.transform.position = targetPoint;
    }

    public void TryPlaceInstrument()
    {
        if (selectedInstrument == null) return;

        SlotController[] allSlots = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        SlotController correctSlot = null;
        SlotController wrongSlot = null;

        foreach (var slot in allSlots)
        {
            if (slot.isOccupied) continue;

            BoxCollider slotCollider = slot.GetComponent<BoxCollider>();
            if (slotCollider == null) continue;

            Vector3 worldCenter = slot.transform.TransformPoint(slotCollider.center);
            Vector3 halfExtents = Vector3.Scale(slotCollider.size * 0.5f, slot.transform.lossyScale);

            Collider[] overlaps = Physics.OverlapBox(
                worldCenter,
                halfExtents,
                slot.transform.rotation,
                LayerMask.GetMask("Instruments")
            );

            foreach (var col in overlaps)
            {
                InstrumentData instrument = col.GetComponent<InstrumentData>();
                if (instrument == null)
                    instrument = col.GetComponentInParent<InstrumentData>();
                if (instrument == null || instrument != selectedInstrument) continue;

                if (slot.expectedSlotID == selectedInstrument.slotID)
                    correctSlot = slot;
                else
                    wrongSlot = slot;
            }
        }

        if (correctSlot != null)
            EvaluatePlacement(selectedInstrument, correctSlot);
        else if (wrongSlot != null)
            EvaluatePlacement(selectedInstrument, wrongSlot);
        else
            selectedInstrument.ReturnToOriginalPosition();

        DeselectInstrument();
    }

    private void EvaluatePlacement(InstrumentData instrument, SlotController slot)
    {
        if (slot.expectedSlotID == instrument.slotID)
        {
            instrument.transform.position = slot.transform.position;
            instrument.transform.rotation = slot.transform.rotation;
            slot.isOccupied = true;
            slot.ghostObject.SetActive(false);
            instrument.SetPlaced(true);
            placedCount++;
            UIManager.Instance.UpdateProgress(placedCount, totalInstruments);
            UIManager.Instance.ShowFeedback(instrument.instrumentName + " Placed Successfully", true);

            if (audioSource != null && correctPlacementClip != null)
                audioSource.PlayOneShot(correctPlacementClip);

            ResetAllSlotHighlights();

            if (placedCount >= totalInstruments)
            {
                if (audioSource != null && completionClip != null)
                    audioSource.PlayOneShot(completionClip);
                UIManager.Instance.ShowCompletion();
            }
        }
        else
        {
            instrument.ReturnToOriginalPosition();
            UIManager.Instance.ShowFeedback("Incorrect Instrument. Please place in correct location.", false);

            if (audioSource != null && wrongPlacementClip != null)
                audioSource.PlayOneShot(wrongPlacementClip);

            ResetAllSlotHighlights();
        }
    }

    private void ResetAllSlotHighlights()
    {
        SlotController[] allSlots = FindObjectsByType<SlotController>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
            slot.ResetHighlight();
    }

    public void ResetSimulation()
    {
        placedCount = 0;

        foreach (var instrument in FindObjectsByType<InstrumentData>(FindObjectsSortMode.None))
        {
            instrument.ReturnToOriginalPosition();
            instrument.SetPlaced(false);
        }

        foreach (var slot in FindObjectsByType<SlotController>(FindObjectsSortMode.None))
            slot.ResetSlot();

        ResetAllSlotHighlights();
        FindFirstObjectByType<InputController>().ResetDragState();
        UIManager.Instance.ResetUI();
        UIManager.Instance.UpdateProgress(0, totalInstruments);
    }

    public void ToggleLighting()
    {
        if (isModeA)
        {
            isModeA = false;
            lightingModeA.SetActive(false);
            lightingModeB.SetActive(true);
            UIManager.Instance.UpdateLightingButtonText("Lighting Mode B");
        }
        else
        {
            isModeA = true;
            lightingModeA.SetActive(true);
            lightingModeB.SetActive(false);
            UIManager.Instance.UpdateLightingButtonText("Lighting Mode A");
        }
    }
}