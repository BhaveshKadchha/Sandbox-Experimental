using System.Collections.Generic;
using UnityEngine;

public class TMP_TextScale_Character : TMP_TextAnimation
{
    [SerializeField] float speed = 10;
    [SerializeField] List<int> animatableLetters;
    float time;
    int currentLetter = 0;
    Matrix4x4 scaleZero, scaleAnim;

    public override void Awake()
    {
        base.Awake();

        scaleZero = Matrix4x4.Scale(Vector3.zero);
        animatableLetters = new List<int>();

        string s = textMesh.text;
        for (int i = 0; i < s.Length; i++)
            if (s[i] != ' ') animatableLetters.Add(i);
    }

    public override void Animate()
    {
        if (currentLetter >= animatableLetters.Count) return;

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        scaleAnim = Matrix4x4.Scale(Vector3.one * time);

        UpdateVertices(animatableLetters[currentLetter], scaleAnim);

        for (int i = animatableLetters[currentLetter] + 1; i < textMesh.textInfo.characterCount; i++)
            UpdateVertices(i, scaleZero);

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
        time += Time.deltaTime * speed;

        if (time > 1)
        {
            currentLetter++;
            time = 0;
        }
    }

    void UpdateVertices(int characterNumber, Matrix4x4 matrix)
    {
        charInfo = textMesh.textInfo.characterInfo[characterNumber];
        index = charInfo.vertexIndex;

        vertices[index] = matrix.MultiplyPoint3x4(vertices[index]);
        vertices[index + 1] = matrix.MultiplyPoint3x4(vertices[index + 1]);
        vertices[index + 2] = matrix.MultiplyPoint3x4(vertices[index + 2]);
        vertices[index + 3] = matrix.MultiplyPoint3x4(vertices[index + 3]);
    }
}