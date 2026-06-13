using UnityEngine;

public class SlotController : MonoBehaviour
{
    public string expectedSlotID;
    public GameObject ghostObject;
    public bool isOccupied = false;

    private Renderer[] ghostRenderers;
    private Color[] defaultGhostColors;
    private InstrumentData currentInstrumentInside;

    private Color correctColor = new Color(0f, 0.9f, 0.46f, 0.35f);
    private Color incorrectColor = new Color(1f, 0.32f, 0.32f, 0.35f);

    void Start()
    {
        Collider slotCollider = GetComponent<Collider>();
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (var col in childColliders)
            Physics.IgnoreCollision(slotCollider, col);

        CacheRenderers();
    }

    private void CacheRenderers()
    {
        if (ghostObject != null)
        {
            ghostRenderers = ghostObject.GetComponentsInChildren<Renderer>();
            defaultGhostColors = new Color[ghostRenderers.Length];
            for (int i = 0; i < ghostRenderers.Length; i++)
                defaultGhostColors[i] = ghostRenderers[i].material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return;

        InstrumentData instrument = other.GetComponent<InstrumentData>();
        if (instrument == null)
            instrument = other.GetComponentInParent<InstrumentData>();
        if (instrument == null) return;

        currentInstrumentInside = instrument;

        if (instrument.slotID == expectedSlotID)
            SetGhostColor(correctColor);
        else
            SetGhostColor(incorrectColor);
    }

    void OnTriggerExit(Collider other)
    {
        InstrumentData instrument = other.GetComponent<InstrumentData>();
        if (instrument == null)
            instrument = other.GetComponentInParent<InstrumentData>();
        if (instrument == null) return;

        currentInstrumentInside = null;
        ResetHighlight();
    }

    public InstrumentData GetInstrumentInside() => currentInstrumentInside;

    private void SetGhostColor(Color color)
    {
        if (ghostRenderers == null) return;
        foreach (var r in ghostRenderers)
            r.material.color = color;
    }

    public void ResetHighlight()
    {
        if (ghostRenderers == null || defaultGhostColors == null) return;
        for (int i = 0; i < ghostRenderers.Length; i++)
            ghostRenderers[i].material.color = defaultGhostColors[i];
    }

    public void ResetSlot()
    {
        isOccupied = false;
        currentInstrumentInside = null;
        if (ghostObject != null)
        {
            ghostObject.SetActive(true);
            CacheRenderers();
        }
        ResetHighlight();
    }
}