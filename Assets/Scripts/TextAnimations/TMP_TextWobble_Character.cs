using UnityEngine;

public class TMP_TextWobble_Character : TMP_TextAnimation
{
    [SerializeField] float sinWobble = 3.3f;
    [SerializeField] float cosWobble = 2.5f;

    public override void Animate()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            charInfo = textMesh.textInfo.characterInfo[i];
            index = charInfo.vertexIndex;
            Vector3 offset = WobbleEffect(Time.time + i);

            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 WobbleEffect(float time) => new Vector2(Mathf.Sin(time * sinWobble), Mathf.Cos(time * cosWobble));
}