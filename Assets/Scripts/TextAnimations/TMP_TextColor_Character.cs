using UnityEngine;

public class TMP_TextColor_Character : TMP_TextAnimation
{
    [SerializeField] Gradient gradient;
    Color[] colors;
    float speed = 0.001f;

    public override void Animate()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            charInfo = textMesh.textInfo.characterInfo[i];
            index = charInfo.vertexIndex;

            colors[index] = gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * speed, 1f));
            colors[index + 1] = gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * speed, 1f));
            colors[index + 2] = gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * speed, 1f));
            colors[index + 3] = gradient.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * speed, 1f));
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
}