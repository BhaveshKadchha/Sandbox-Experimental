using System.Collections.Generic;
using UnityEngine;

public class TMP_TextWobble_Word : TMP_TextAnimation
{
    [SerializeField] float sinWobble = 3.3f;
    [SerializeField] float cosWobble = 2.5f;

    List<int> wordIndexes;
    List<int> wordLengths;

    public override void Awake()
    {
        base.Awake();
        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }

    public override void Animate()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int w = 0; w < wordIndexes.Count; w++)
        {
            int wordIndex = wordIndexes[w];
            Vector3 offset = Wobble(Time.time + w);

            for (int i = 0; i < wordLengths[w]; i++)
            {
                charInfo = textMesh.textInfo.characterInfo[wordIndex + i];
                index = charInfo.vertexIndex;

                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time) => new Vector2(Mathf.Sin(time * sinWobble), Mathf.Cos(time * cosWobble));
}