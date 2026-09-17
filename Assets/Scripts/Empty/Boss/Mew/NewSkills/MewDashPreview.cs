using UnityEngine;

/// <summary>World-space ribbon revealed from caster to destination. Reuses geometry buffers for every frame.</summary>
public sealed class MewDashPreview : MonoBehaviour
{
    private const int Segments = 24;
    private const int Lanes = 5;
    private readonly Vector3[] vertices = new Vector3[(Segments + 1) * Lanes];
    private readonly Color[] colors = new Color[(Segments + 1) * Lanes];
    private Mesh mesh;
    private Material material;
    private Material interiorFlow;
    private LineRenderer arrow;
    private SkillArrow prefabArrow;
    private SpriteRenderer[] arrowSprites;

    public static MewDashPreview Create(GameObject arrowPrefab = null)
    {
        var obj = new GameObject("Mew directional dash ribbon", typeof(MeshFilter), typeof(MeshRenderer));
        var preview = obj.AddComponent<MewDashPreview>();
        preview.Build();
        if (arrowPrefab != null)
        {
            var instance = Instantiate(arrowPrefab, preview.transform);
            preview.prefabArrow = instance.GetComponent<SkillArrow>();
            if (preview.prefabArrow != null)
            {
                preview.prefabArrow.enabled = false;
                preview.arrow.enabled = false;
                preview.GetComponent<MeshRenderer>().enabled = false;
                Shader flowShader = Resources.Load<Shader>("MewVisuals/MewDashFlow");
                if (flowShader != null)
                {
                    preview.interiorFlow = new Material(flowShader);
                    preview.prefabArrow.ArrowHead.sharedMaterial = preview.interiorFlow;
                    preview.prefabArrow.ArrowBody.sharedMaterial = preview.interiorFlow;
                }
                preview.arrowSprites = instance.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in preview.arrowSprites)
                    sprite.sortingOrder = 103 + sprite.sortingOrder;
            }
        }
        return preview;
    }

    private void Build()
    {
        material = new Material(Shader.Find("Sprites/Default"));
        mesh = new Mesh { name = "Mew dash gradient" };
        mesh.MarkDynamic();
        int[] triangles = new int[Segments * (Lanes - 1) * 6];
        for (int i = 0; i < Segments; i++)
            for (int lane = 0; lane < Lanes - 1; lane++)
            {
                int t = (i * (Lanes - 1) + lane) * 6, v = i * Lanes + lane;
                triangles[t] = v; triangles[t + 1] = v + 1; triangles[t + 2] = v + Lanes;
                triangles[t + 3] = v + 1; triangles[t + 4] = v + Lanes + 1; triangles[t + 5] = v + Lanes;
            }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = new Vector2[vertices.Length];
        GetComponent<MeshFilter>().sharedMesh = mesh;
        var renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.sortingOrder = 102;
        arrow = new GameObject("Leading arrow").AddComponent<LineRenderer>();
        arrow.transform.SetParent(transform, false);
        arrow.sharedMaterial = material;
        arrow.useWorldSpace = true;
        arrow.positionCount = 3;
        arrow.startWidth = arrow.endWidth = 0.09f;
        arrow.sortingOrder = 103;
    }

    public void Show(Vector2 start, Vector2 end, float reveal, bool dashing, float width)
    {
        Vector2 delta = end - start;
        Vector2 direction = delta.sqrMagnitude > 0.0001f ? delta.normalized : Vector2.right;
        Vector2 normal = new Vector2(-direction.y, direction.x);
        reveal = Mathf.Clamp01(reveal);
        for (int i = 0; i <= Segments; i++)
        {
            float u = i / (float)Segments;
            Vector2 p = Vector2.Lerp(start, end, u);
            float visible = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((reveal - u) * 8f + (reveal >= 0.999f ? 1f : 0f)));
            float flow = 0.85f + 0.15f * Mathf.Sin(u * 28f - Time.time * 10f);
            Color c = Color.Lerp(new Color(0.53f, 0.83f, 1f), new Color(1f, 0.38f, 0.73f), u);
            c.a = visible * flow * (dashing ? 0.22f : 0.35f);
            for (int lane = 0; lane < Lanes; lane++)
            {
                vertices[i * Lanes + lane] = p + normal * width * (lane / (float)(Lanes - 1) - 0.5f);
                Color feathered = c;
                feathered.a *= lane == 0 || lane == Lanes - 1 ? 0f : lane == 2 ? 1f : 0.65f;
                colors[i * Lanes + lane] = feathered;
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.RecalculateBounds();
        Vector2 tip = Vector2.Lerp(start, end, reveal);
        if (prefabArrow != null)
        {
            float length = Vector2.Distance(start, tip);
            prefabArrow.transform.position = start;
            prefabArrow.ArrowScale = Mathf.Min(Mathf.Clamp(width * 0.65f, 0.65f, 1.5f), Mathf.Max(0.001f, length));
            prefabArrow.ArrowAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            prefabArrow.ArrowLength = Mathf.Max(0f, length - prefabArrow.ArrowScale);
            prefabArrow.ArrowColor = new Color(1f, 0.58f, 0.83f);
            prefabArrow.SetArrow();
            float alpha = Mathf.SmoothStep(0f, 1f, reveal * 5f) * (dashing ? 0.48f : 0.8f);
            foreach (var sprite in arrowSprites)
            {
                bool fill = sprite == prefabArrow.ArrowHead || sprite == prefabArrow.ArrowBody;
                sprite.color = fill ? new Color(1f, 0.58f, 0.83f, alpha * 0.7f)
                    : new Color(1f, 0.91f, 1f, alpha);
                sprite.enabled = length > 0.08f;
            }
        }
        float size = Mathf.Min(0.6f, delta.magnitude * 0.25f);
        arrow.SetPosition(0, tip - direction * size + normal * size * 0.65f);
        arrow.SetPosition(1, tip);
        arrow.SetPosition(2, tip - direction * size - normal * size * 0.65f);
        arrow.startColor = arrow.endColor = new Color(1f, 0.82f, 0.96f, reveal > 0.02f ? 0.85f : 0f);
    }

    private void OnDestroy()
    {
        if (interiorFlow != null) { if (Application.isPlaying) Destroy(interiorFlow); else DestroyImmediate(interiorFlow); }
        if (mesh != null) { if (Application.isPlaying) Destroy(mesh); else DestroyImmediate(mesh); }
        if (material != null) { if (Application.isPlaying) Destroy(material); else DestroyImmediate(material); }
    }
}
