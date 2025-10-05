using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI�����
/// </summary>
public class UIPolygon : MaskableGraphic
{
    [SerializeField]
    Texture m_Texture;
    /// <summary>
    /// ���
    /// </summary>
    public bool fill = true;

    /// <summary>
    /// ����
    /// </summary>
    [Range(3, 360)]
    public int sides = 3;
    /// <summary>
    /// ��ת�Ƕ�
    /// </summary>
    [Range(0, 360)]
    public float rotation = 0;
    /// <summary>
    /// ��������
    /// </summary>
    [Range(0, 1)]
    public float[] VerticesDistances = new float[3];

    private float size = 0;



    public override Texture mainTexture
    {
        get
        {
            return m_Texture == null ? s_WhiteTexture : m_Texture;
        }
    }

    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value) return;
            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }
    #region �ṩ�ⲿ�Ľӿ�
    public void DrawPolygon(int _sides)
    {
        sides = _sides;
        VerticesDistances = new float[_sides + 1];
        for (int i = 0; i < _sides; i++) VerticesDistances[i] = 1;
    }

    public void DrawPolygon(List<float> datas)
    {
        List<float> finalDatas = new List<float>(datas);
        sides = finalDatas.Count;
        // �������һ���㣬���һ�������һ�����غ�
        finalDatas.Add(finalDatas[0]);
        VerticesDistances = finalDatas.ToArray();
        // �����ػ�
        SetVerticesDirty();
    }
    #endregion

    void Update()
    {
        // ���ݿ������ߴ�
        size = rectTransform.rect.width;
        if (rectTransform.rect.width > rectTransform.rect.height)
            size = rectTransform.rect.height;
        else
            size = rectTransform.rect.width;
    }

    protected UIVertex[] SetVertexs(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    /// <summary>
    /// ��дOnPopulateMesh����
    /// </summary>
    /// <param name="vh"></param>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Vector2 prevX = Vector2.zero;
        Vector2 prevY = Vector2.zero;
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;
        float degrees = 360f / sides;
        int vertices = sides + 1;
        Debug.Log(VerticesDistances.Length);
        Debug.Log( vertices);
        if (VerticesDistances.Length != vertices)
        {
            VerticesDistances = new float[vertices];
            for (int i = 0; i < vertices - 1; i++) VerticesDistances[i] = 1;
        }
        // ���һ�����㣬Ҳ���ǵ�һ������
        VerticesDistances[vertices - 1] = VerticesDistances[0];
        for (int i = 0; i < vertices; i++)
        {
            float outer = -rectTransform.pivot.x * size * VerticesDistances[i];
            float inner = -rectTransform.pivot.x * size * VerticesDistances[i];
            float rad = Mathf.Deg2Rad * (i * degrees + rotation);
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            uv0 = new Vector2(0, 1);
            uv1 = new Vector2(1, 1);
            uv2 = new Vector2(1, 0);
            uv3 = new Vector2(0, 0);
            pos0 = prevX;
            pos1 = new Vector2(outer * c, outer * s);
            if (fill)
            {
                pos2 = Vector2.zero;
                pos3 = Vector2.zero;
            }
            else
            {
                pos2 = new Vector2(inner * c, inner * s);
                pos3 = prevY;
            }
            prevX = pos1;
            prevY = pos2;
            vh.AddUIVertexQuad(SetVertexs(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
        }
    }
}

