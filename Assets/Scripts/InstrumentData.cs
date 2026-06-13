using UnityEngine;

public class InstrumentData : MonoBehaviour
{
    public string instrumentName;
    [TextArea] public string description;
    public string slotID;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Renderer[] renderers;
    private Color[] originalColors;
    private bool isPlaced = false;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    public void Highlight(bool on)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = on ? Color.yellow : originalColors[i];
        }
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
        GetComponent<Collider>().enabled = !placed;
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}