using UnityEngine;

/// <summary>
/// Effect Look More Like Water Shader Efect
/// </summary>
public class TMP_TextWobble_Vertex : TMP_TextAnimation
{
    [SerializeField] float sinWobble = 3.3f;
    [SerializeField] float cosWobble = 2.5f;

    public override void Animate()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = WobbleEffect(Time.time + i);
            vertices[i] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 WobbleEffect(float time) => new Vector2(Mathf.Sin(time * sinWobble), Mathf.Cos(time * cosWobble));
}