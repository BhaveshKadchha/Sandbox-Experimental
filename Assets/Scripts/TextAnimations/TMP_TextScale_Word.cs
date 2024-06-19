using UnityEngine;
using System.Collections.Generic;

public class TMP_TextScale_Word : TMP_TextAnimation
{
    [SerializeField] float speed = 5;
    float time;
    int currentWord = 0;
    Matrix4x4 matrixScaleZero, matrixAnimatable;
    List<int> wordIndexes, wordLengths;

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
        matrixScaleZero = Matrix4x4.Scale(Vector3.zero);
    }

    public override void Animate()
    {
        if (currentWord >= wordIndexes.Count)
            return;

        time += Time.deltaTime * speed;
        time = Mathf.Clamp01(time);

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        matrixAnimatable = Matrix4x4.Scale(Vector3.one * time);

        ScaleVertices(wordLengths[currentWord], matrixAnimatable, wordIndexes[currentWord]);
        for (int w = currentWord + 1; w < wordIndexes.Count; w++)
            ScaleVertices(wordLengths[w], matrixScaleZero, wordIndexes[w]);

        if (time == 1)
        {
            currentWord++;
            time = 0;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    void ScaleVertices(float loopLength, Matrix4x4 scale, int wordIndex)
    {
        for (int i = 0; i < loopLength; i++)
        {
            charInfo = textMesh.textInfo.characterInfo[wordIndex + i];
            index = charInfo.vertexIndex;

            vertices[index] = scale.MultiplyPoint3x4(vertices[index]);
            vertices[index + 1] = scale.MultiplyPoint3x4(vertices[index + 1]);
            vertices[index + 2] = scale.MultiplyPoint3x4(vertices[index + 2]);
            vertices[index + 3] = scale.MultiplyPoint3x4(vertices[index + 3]);
        }
    }
}