using UnityEngine;

/// <summary>Small, reusable visual vocabulary. Geometry is built once; only color and ornament rotation animate.</summary>
public sealed class MewSigilVisual : MonoBehaviour
{
    public enum Style { Mirror, Clock, Sanctuary, Target }
    private Material material;
    private Mesh tickMesh;
    private LineRenderer ring, glow, ornament;
    private Style style;
    private Color tint;
    private float age, charge;

    public static MewSigilVisual Create(Transform parent, Vector3 position, float radius, Style style, int order = 65)
    {
        GameObject obj = new GameObject("Mew " + style + " sigil");
        obj.transform.SetParent(parent, true);
        obj.transform.position = position;
        var visual = obj.AddComponent<MewSigilVisual>();
        visual.Build(radius, style, order);
        return visual;
    }

    public void SetCharge(float value) { charge = Mathf.Clamp01(value); }

    private void Build(float radius, Style usedStyle, int order)
    {
        style = usedStyle;
        tint = style == Style.Sanctuary ? new Color(0.38f, 1f, 0.88f) :
            style == Style.Target ? new Color(1f, 0.42f, 0.72f) : new Color(0.73f, 0.56f, 1f);
        material = new Material(Shader.Find("Sprites/Default"));
        ring = Line("Fine rim", radius, 0.045f, 96, 0f, order + 1);
        glow = Line("Soft rim", radius, 0.2f, 96, 0f, order);
        int points = style == Style.Clock || style == Style.Sanctuary ? 96 : style == Style.Target ? 8 : 12;
        float inset = style == Style.Clock ? 0f : style == Style.Sanctuary ? -0.1f : style == Style.Target ? 0.65f : 0.32f;
        ornament = Line("Constellation", radius * 0.89f, 0.045f, points, inset, order + 2);
        if (style == Style.Clock) BuildTicks(radius, order + 2);
        SetColors(0f);
    }

    private LineRenderer Line(string name, float radius, float width, int count, float inset, int order)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(transform, false);
        LineRenderer line = obj.AddComponent<LineRenderer>();
        line.sharedMaterial = material;
        line.useWorldSpace = false;
        line.loop = true;
        line.widthMultiplier = width;
        line.sortingOrder = order;
        line.numCornerVertices = 2;
        Vector3[] points = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            float r = radius * (inset < 0f ? 1f + inset * (0.5f - 0.5f * Mathf.Cos(angle * 6f)) :
                i % 2 == 0 ? 1f : 1f - inset);
            points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * r;
        }
        line.positionCount = count;
        line.SetPositions(points);
        return line;
    }

    private void BuildTicks(float radius, int order)
    {
        const int count = 60;
        Vector3[] vertices = new Vector3[count * 4];
        int[] triangles = new int[count * 6];
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 radial = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 side = new Vector3(-radial.y, radial.x, 0f) * (i % 5 == 0 ? 0.055f : 0.025f);
            Vector3 outer = radial * radius * 0.98f;
            Vector3 inner = radial * (radius * 0.98f - (i % 5 == 0 ? 0.6f : 0.22f));
            int v = i * 4, t = i * 6;
            vertices[v] = inner - side; vertices[v + 1] = outer - side;
            vertices[v + 2] = outer + side; vertices[v + 3] = inner + side;
            triangles[t] = v; triangles[t + 1] = v + 1; triangles[t + 2] = v + 2;
            triangles[t + 3] = v; triangles[t + 4] = v + 2; triangles[t + 5] = v + 3;
            for (int j = 0; j < 4; j++) colors[v + j] = new Color(1f, 0.89f, 0.72f, i % 5 == 0 ? 0.65f : 0.25f);
        }
        tickMesh = new Mesh { name = "Mew clock ticks", vertices = vertices, triangles = triangles, colors = colors };
        tickMesh.uv = new Vector2[vertices.Length];
        tickMesh.RecalculateBounds();
        GameObject obj = new GameObject("Sixty engraved ticks", typeof(MeshFilter), typeof(MeshRenderer));
        obj.transform.SetParent(transform, false);
        obj.GetComponent<MeshFilter>().sharedMesh = tickMesh;
        var renderer = obj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.sortingOrder = order;
    }

    private void Update()
    {
        age += Time.deltaTime;
        if (ornament == null) return;
        if (style != Style.Clock)
            ornament.transform.localRotation = Quaternion.Euler(0f, 0f, age * (style == Style.Mirror ? -24f : 10f));
        if (style == Style.Target) ornament.transform.localScale = Vector3.one * Mathf.Lerp(1.35f, 1f, charge);
        SetColors(Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(age / 0.3f)));
    }

    private void SetColors(float alpha)
    {
        float pulse = 0.85f + 0.15f * Mathf.Sin(age * 3f);
        ring.startColor = ring.endColor = new Color(tint.r, tint.g, tint.b, alpha * 0.65f);
        glow.startColor = glow.endColor = new Color(tint.r, tint.g, tint.b, alpha * pulse * 0.13f);
        ornament.startColor = ornament.endColor = new Color(0.7f, 0.94f, 1f, alpha * pulse * 0.45f);
    }

    private void OnDestroy()
    {
        if (material != null) { if (Application.isPlaying) Destroy(material); else DestroyImmediate(material); }
        if (tickMesh != null) { if (Application.isPlaying) Destroy(tickMesh); else DestroyImmediate(tickMesh); }
    }
}
