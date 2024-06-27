using UnityEngine;

public class ExperimentalHelper
{
    public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, int fontSize, Color fontColor, TextAlignment alignment, TextAnchor anchor)
    {
        GameObject go = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = go.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = go.GetComponent<TextMesh>();

        textMesh.anchor = anchor;
        textMesh.alignment = alignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = fontColor;
        //textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorlPosition(Camera cam = null)
    {
        if (cam == null)
            cam = Camera.main;

        Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);
        return vec;
    }
}