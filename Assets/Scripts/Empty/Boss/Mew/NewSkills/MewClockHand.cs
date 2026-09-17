using System.Collections.Generic;
using UnityEngine;

/// <summary>A resolution-independent pointed clock hand, with an ivory edge, faceted needle and jeweled pivot.</summary>
public sealed class MewClockHand : MonoBehaviour
{
    private Material material;
    private readonly List<Mesh> meshes = new List<Mesh>();

    public void Build(float length, float hitWidth, Color tint, int order)
    {
        material = new Material(Shader.Find("Sprites/Default"));
        // Keep the thin needle readable while showing its full gameplay hit width.
        var danger = gameObject.AddComponent<LineRenderer>();
        danger.sharedMaterial = material;
        danger.useWorldSpace = false;
        danger.positionCount = 2;
        danger.SetPosition(0, Vector3.zero);
        danger.SetPosition(1, Vector3.up * length);
        danger.startWidth = danger.endWidth = hitWidth;
        danger.startColor = danger.endColor = new Color(1f, 0.3f, 0.7f, 0.18f);
        danger.sortingOrder = order - 1;
        float halfWidth = Mathf.Max(0.15f, hitWidth * 0.5f);
        Vector2[] needle = {
            new Vector2(0f, length), new Vector2(-halfWidth, length * 0.78f),
            new Vector2(-halfWidth * 0.46f, length * 0.69f),
            new Vector2(-halfWidth * 0.38f, 0f), new Vector2(-halfWidth * 0.65f, -0.55f),
            new Vector2(0f, -0.9f), new Vector2(halfWidth * 0.65f, -0.55f),
            new Vector2(halfWidth * 0.38f, 0f), new Vector2(halfWidth * 0.46f, length * 0.69f),
            new Vector2(halfWidth, length * 0.78f)
        };
        Polygon("Ivory needle edge", needle, new Color(1f, 0.94f, 0.77f), order);
        Vector2[] inset = new Vector2[needle.Length];
        for (int i = 0; i < needle.Length; i++) inset[i] = new Vector2(needle[i].x * 0.62f, needle[i].y * 0.97f);
        Polygon("Rose crystal needle", inset, tint, order + 1);
        Polygon("Needle highlight", new[] { Vector2.zero, new Vector2(0f, length * 0.96f),
            new Vector2(halfWidth * 0.32f, length * 0.76f), new Vector2(halfWidth * 0.15f, 0f) },
            new Color(1f, 0.98f, 1f), order + 2);
        Disc("Pivot rim", 0.46f, new Color(1f, 0.94f, 0.77f), order + 3);
        Disc("Pivot crystal", 0.31f, new Color(0.65f, 0.3f, 0.85f), order + 4);
        Polygon("Pivot glint", new[] { new Vector2(0f, 0.24f), new Vector2(-0.16f, 0f),
            new Vector2(0f, -0.24f), new Vector2(0.16f, 0f) }, new Color(0.8f, 1f, 1f), order + 5);
    }

    private void Disc(string name, float radius, Color color, int order)
    {
        Vector2[] points = new Vector2[40];
        for (int i = 0; i < points.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / points.Length;
            points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }
        Polygon(name, points, color, order);
    }

    private void Polygon(string name, Vector2[] points, Color color, int order)
    {
        GameObject part = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
        part.transform.SetParent(transform, false);
        Vector3[] vertices = new Vector3[points.Length + 1];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[points.Length * 3];
        colors[0] = color;
        for (int i = 0; i < points.Length; i++)
        {
            vertices[i + 1] = points[i];
            colors[i + 1] = color;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % points.Length + 1;
        }
        Mesh mesh = new Mesh { name = name, vertices = vertices, colors = colors, triangles = triangles };
        mesh.uv = new Vector2[vertices.Length];
        mesh.RecalculateBounds();
        meshes.Add(mesh);
        part.GetComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = part.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.sortingOrder = order;
    }

    private void OnDestroy()
    {
        foreach (Mesh mesh in meshes)
            if (mesh != null) { if (Application.isPlaying) Destroy(mesh); else DestroyImmediate(mesh); }
        if (material != null) { if (Application.isPlaying) Destroy(material); else DestroyImmediate(material); }
    }
}
