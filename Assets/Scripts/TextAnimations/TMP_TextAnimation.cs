using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public abstract class TMP_TextAnimation : MonoBehaviour
{
    [HideInInspector] public TMP_Text textMesh;
    [HideInInspector] public Mesh mesh;
    [HideInInspector] public Vector3[] vertices;
    [HideInInspector] public TMP_CharacterInfo charInfo;
    [HideInInspector] public int index;

    public virtual void Awake() => textMesh = GetComponent<TMP_Text>();

    public virtual void Update() => Animate();

    public abstract void Animate();
}