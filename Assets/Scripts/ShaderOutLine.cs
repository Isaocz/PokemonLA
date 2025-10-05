using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/ShaderOutLine")]
    public class ShaderOutLine : BaseMeshEffect
    {
        public Color OutLineColor;
        [Range(0, 50)]
        public int Size = 1;
        [Range(0, 50)]
        public float Strength = 1;

        private static List<UIVertex> m_VetexList = new List<UIVertex>();
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            vh.GetUIVertexStream(m_VetexList);

            int count = m_VetexList.Count;
            for (int i = 0; i < count; i += 6)
            {
                if (i + 3 >= count)
                    break;

                UIVertex v1 = m_VetexList[i];
                UIVertex v2 = m_VetexList[i + 1];
                UIVertex v3 = m_VetexList[i + 2];
                UIVertex v4 = m_VetexList[i + 3];
                UIVertex v5 = m_VetexList[i + 4];
                UIVertex v6 = m_VetexList[i + 5];
                Vector2 bottomLeft = v1.uv0;
                Vector2 topRight = v4.uv0;
                if (bottomLeft.x > topRight.x)
                {
                    bottomLeft = v4.uv0;
                    topRight = v1.uv0;
                }
                v1.uv2 = bottomLeft;
                v2.uv2 = bottomLeft;
                v3.uv2 = bottomLeft;
                v4.uv2 = bottomLeft;
                v5.uv2 = bottomLeft;
                v6.uv2 = bottomLeft;
                v1.uv3 = topRight;
                v2.uv3 = topRight;
                v3.uv3 = topRight;
                v4.uv3 = topRight;
                v5.uv3 = topRight;
                v6.uv3 = topRight;
                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
                m_VetexList[i + 3] = v4;
                m_VetexList[i + 4] = v5;
                m_VetexList[i + 5] = v6;
            }
            _ProcessVertices();
            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VetexList);

            //修改文本材质参数
            var text = GetComponent<Text>();
            text.material.SetColor("_LightColor", OutLineColor);
            text.material.SetFloat("_Size", Size);
            text.material.SetFloat("_Strength", Strength);
        }

        private void _ProcessVertices()
        {
            for (int i = 0, count = m_VetexList.Count - 3; i <= count; i += 3)
            {
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];

                // 计算原顶点坐标中心点
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;

                // 计算原始顶点坐标和UV的方向
                Vector2 triX, triY, uvX, uvY;
                Vector2 pos1 = v1.position;
                Vector2 pos2 = v2.position;
                Vector2 pos3 = v3.position;

                if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                    > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
                {
                    triX = pos2 - pos1;
                    triY = pos3 - pos2;
                    uvX = v2.uv0 - v1.uv0;
                    uvY = v3.uv0 - v2.uv0;
                }
                else
                {
                    triX = pos3 - pos2;
                    triY = pos2 - pos1;
                    uvX = v3.uv0 - v2.uv0;
                    uvY = v2.uv0 - v1.uv0;
                }

                // 计算原始UV框
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0);
                var uvOrigin = new Vector4(uvMin.x, uvMin.y, uvMax.x, uvMax.y);

                // 为每个顶点设置新的Position和UV，并传入原始UV框
                int width = Mathf.Max(Size - 1, 0);
                v1 = _SetNewPosAndUV(v1, width, posCenter, triX, triY, uvX, uvY, uvOrigin);
                v2 = _SetNewPosAndUV(v2, width, posCenter, triX, triY, uvX, uvY, uvOrigin);
                v3 = _SetNewPosAndUV(v3, width, posCenter, triX, triY, uvX, uvY, uvOrigin);

                // 应用设置后的UIVertex
                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
            }
        }

        private static UIVertex _SetNewPosAndUV(UIVertex pVertex, int pOutLineWidth,
            Vector2 pPosCenter,
            Vector2 pTriangleX, Vector2 pTriangleY,
            Vector2 pUVX, Vector2 pUVY,
            Vector4 pUVOrigin)
        {
            // Position
            var pos = pVertex.position;
            var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
            var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
            pos.x += posXOffset;
            pos.y += posYOffset;
            pVertex.position = pos;

            // UV
            var uv = pVertex.uv0;
            uv += (Vector4)(pUVX / pTriangleX.magnitude * posXOffset * ((Vector2.Dot(pTriangleX, Vector2.right) > 0) ? 1 : -1));
            uv += (Vector4)(pUVY / pTriangleY.magnitude * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1));
            pVertex.uv0 = uv;
            //// 原始UV框
            //pVertex.uv1 = new Vector2(pUVOrigin.x, pUVOrigin.y);
            //pVertex.uv2 = new Vector2(pUVOrigin.z, pUVOrigin.w);

            return pVertex;
        }

        private static float _Min(float pA, float pB, float pC)
        {
            return Mathf.Min(Mathf.Min(pA, pB), pC);
        }

        private static float _Max(float pA, float pB, float pC)
        {
            return Mathf.Max(Mathf.Max(pA, pB), pC);
        }

        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x), _Min(pA.y, pB.y, pC.y));
        }

        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x), _Max(pA.y, pB.y, pC.y));
        }
    }
}
