using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DynamicLightningBoltItem : MonoBehaviour
{
    //闪电每次闪烁模式
    public enum LightningBoltMode
    {
        None,
        Random,
        Loop,
        PingPong,
        Child,
        LightningStrike,
    }

    [Tooltip("闪电分段"), Range(0, 8), SerializeField]
    private int segment = 6;

    [Tooltip("闪电最大距离"), SerializeField]
    public float MaxRange = 1;

    [Tooltip("闪电最大宽度"), SerializeField]
    public float MaxWidth = 1;

    [Tooltip("闪电持续时长"), SerializeField]
    private float multDuration = 1;
    [Tooltip("闪电闪烁时长"), SerializeField]
    private float onceDuration = 0.05f;

    [Tooltip("混乱偏移指数"), Range(0.0f, 1.0f), SerializeField]
    private float chaosFactor = 0.15f;

    //一张贴图可以存放多个闪电切图，每次随机截取其中一部分，看起来闪电随机多样
    [Tooltip("纹理中的行数"), SerializeField]
    public int matTextureRows = 1;
    [Tooltip("纹理中的列数"), SerializeField]
    public int matTextureColumns = 1;

    [Tooltip("闪烁动画模式"), SerializeField]
    private LightningBoltMode animationMode = LightningBoltMode.None;

    //随机数，可以多个闪电同一个随机数，形成相同的闪电外观
    private System.Random randomGenerator = new System.Random();

    //材质求贴图尺寸/偏移
    private Vector2 matTextureSize;
    private Vector2[] matTextureOffset;

    private LineRenderer lineRenderer;

    /// <summary>
    /// 线渲染队列
    /// </summary>
    private List<LineRenderer> lineRendererList;

    //动画参数
    private int animationOffsetIndex;
    private int animationPingPongDir = 1;

    //闪电起始/结束为止
    private Transform startTrans;
    private Transform endTrans;

    /// <summary>
    /// 闪电击落特效
    /// </summary>
    public GameObject ThunderPS;


    /// <summary>
    /// 子闪电
    /// </summary>
    public DynamicLightningBoltItem ChildLightning;

    /// <summary>
    /// 子闪电个数
    /// </summary>
    public int ChildCount = 0;

    /// <summary>
    /// 落雷的高度
    /// </summary>
    public float LightningStrikeHigh;












    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        if (matTextureColumns < 1)
            matTextureColumns = 1;
        if (matTextureRows < 1)
            matTextureRows = 1;
        if (matTextureRows * matTextureColumns < 2 && animationMode != LightningBoltMode.None)
        {
            animationMode = LightningBoltMode.None;
        }

        SetMaterialTextute();
    }

    #region --- Test ---

    public Transform startPoint;
    public Transform endPoint;
    private float testTimer;

    private void Start()
    {

        if (animationMode != LightningBoltMode.Child)
        {
            startPoint = new GameObject("StartPoint").transform;
            startPoint.SetParent(transform);
            startPoint.localPosition = Vector3.zero;
            endPoint = new GameObject("EndPoint").transform;
            endPoint.SetParent(transform);
            endPoint.localPosition = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right * MaxRange;
            multDuration = 1000;
            //落雷模式
            switch (animationMode)
            {
                case LightningBoltMode.LightningStrike:
                    startPoint.localPosition = Vector3.zero + Vector3.up * LightningStrikeHigh;
                    endPoint.localPosition = Vector3.zero;
                    break;
            }
            StartLightning(startPoint, endPoint);
        }
        Instantiate(ThunderPS, endPoint.transform.position, Quaternion.identity, endPoint.transform);

        for (int i = 0; i < ChildCount; i++)
        {
            DynamicLightningBoltItem c = Instantiate(ChildLightning, transform.position, Quaternion.identity, transform);
            c.SetChildLightning(lineRenderer , this);
        }
    }


    /// <summary>
    /// 设置子闪电
    /// </summary>
    /// <param name="ParentLine"></param>
    /// <param name="ParentLightning"></param>
    public void SetChildLightning( LineRenderer ParentLine , DynamicLightningBoltItem ParentLightning )
    {
        if (animationMode == LightningBoltMode.Child) {
            lineRenderer.colorGradient = ParentLine.colorGradient;
            lineRenderer.material = ParentLine.material;
            lineRenderer.widthMultiplier = ParentLine.widthMultiplier / 4.0f;
            segment = ParentLightning.segment;
            MaxRange = ParentLightning.MaxRange;
            MaxWidth = ParentLightning.MaxWidth;
            multDuration = ParentLightning.multDuration;
            onceDuration = ParentLightning.onceDuration;
            chaosFactor = ParentLightning.chaosFactor;
            matTextureRows = ParentLightning.matTextureRows;
            matTextureColumns = ParentLightning.matTextureColumns;
            startPoint = ParentLightning.startPoint;
            endPoint = ParentLightning.endPoint;
            StartLightning(startPoint, endPoint);
        }
    }



    private void Update()
    {
        if (testTimer >= 1 && animationMode != LightningBoltMode.Child && animationMode != LightningBoltMode.LightningStrike && endPoint != null)
        {
            testTimer = 0;
            endPoint.localPosition = Quaternion.AngleAxis( Random.Range(0.0f,360.0f) , Vector3.forward ) * Vector3.right * MaxRange;
        }
        testTimer += Time.deltaTime;
    }

    #endregion

    public void StartLightning(Transform startPoint, Transform endPoint)
    {
        startTrans = startPoint;
        endTrans = endPoint;

        StopAllCoroutines();

        if (startTrans == null || endTrans == null)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        GetComponent<LineRenderer>().widthMultiplier = MaxWidth;

        StartCoroutine(TriggerMultLightning());
    }

    public void StopLightning()
    {
        StopAllCoroutines();
        lineRenderer.positionCount = 0;
    }

    private IEnumerator TriggerMultLightning()
    {
        float timer = 0;
        while (timer < multDuration)
        {
            if (startTrans == null || endTrans == null)
                break;
            GenerateLightning(startTrans.position, endTrans.position, segment);
            timer += onceDuration;
            yield return new WaitForSeconds(onceDuration);
        }
        lineRenderer.positionCount = 0;
    }

    //设置材质贴图的尺寸
    private void SetMaterialTextute()
    {
        matTextureSize = new Vector2(1.0f / matTextureColumns, 1.0f / matTextureRows);
        lineRenderer.material.mainTextureScale = matTextureSize;

        matTextureOffset = new Vector2[matTextureRows * matTextureColumns];
        for (int y = 0; y < matTextureRows; y++)
        {
            for (int x = 0; x < matTextureColumns; x++)
            {
                matTextureOffset[x + (y * matTextureColumns)] = new Vector2((float)x / matTextureColumns, (float)y / matTextureRows);
            }
        }
    }

    private void GenerateLightning(Vector3 startPoint, Vector3 endPoint, int segment)
    {
        List<(Vector3, Vector3)> segmentList = new List<(Vector3, Vector3)>();
        segmentList.Add((startPoint, endPoint));

        if (segment > 0 && segment <= 8)
        {
            int startIndex = 0;
            float offsetAmount = (endPoint - startPoint).magnitude * chaosFactor;
            Vector3 middlePoint;

            for (int i = 0; i < segment; i++)
            {
                //每次循环将新的分段数据添加至列表
                //previousIndex/startIndex标记上次循环的起始/结束位置
                //使用上次循环计算的结果作为下次循环的源数据，计算分段数据
                int previousIndex = startIndex;
                startIndex = segmentList.Count;
                for (int j = previousIndex; j < startIndex; j++)
                {
                    startPoint = segmentList[j].Item1;
                    endPoint = segmentList[j].Item2;
                    //插入中点
                    middlePoint = (startPoint + endPoint) * 0.5f;
                    //随机偏移中点
                    middlePoint += GetRandomVector(startPoint, endPoint, offsetAmount);
                    //添加两个新分段
                    segmentList.Add((startPoint, middlePoint));
                    segmentList.Add((middlePoint, endPoint));
                }
                //每循环一次，闪电偏移减半
                offsetAmount *= 0.5f;
            }
            //移除多余分段
            segmentList.RemoveRange(0, startIndex);
        }
        UpdateLineRenderer(segmentList);
    }
    private Vector3 GetRandomVector(Vector3 start, Vector3 end, float offsetAmount)
    {
        Vector3 directionNormalized = (end - start).normalized;
        Vector3 perpendicularNormal = GetPerpendicularVector(directionNormalized);

        //随机偏移距离
        float distance = ((float)randomGenerator.NextDouble() + 0.1f) * offsetAmount;

        //随机旋转角度
        float rotationAngle = (float)randomGenerator.NextDouble() * 360.0f;

        //绕着方向旋转，然后由垂直向量偏移
        return Quaternion.AngleAxis(rotationAngle, directionNormalized) * perpendicularNormal * distance;
    }
    private Vector3 GetPerpendicularVector(Vector3 directionNormalized)
    {
        if (directionNormalized == Vector3.zero)
        {
            return Vector3.right;
        }
        else
        {
            // use cross product to find any perpendicular vector around directionNormalized:
            // 0 = x * px + y * py + z * pz
            // => pz = -(x * px + y * py) / z
            // for computational stability use the component farthest from 0 to divide by
            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float px, py, pz;
            float ax = Mathf.Abs(x);
            float ay = Mathf.Abs(y);
            float az = Mathf.Abs(z);
            if (ax >= ay && ay >= az)
            {
                // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                py = 1.0f;
                pz = 1.0f;
                px = -(y * py + z * pz) / x;
            }
            else if (ay >= az)
            {
                // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                px = 1.0f;
                pz = 1.0f;
                py = -(x * px + z * pz) / y;
            }
            else
            {
                // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                px = 1.0f;
                py = 1.0f;
                pz = -(x * px + y * py) / z;
            }
            return new Vector3(px, py, pz).normalized;
        }
    }

    private void UpdateLineRenderer(List<(Vector3, Vector3)> segmentList)
    {
        if (segmentList == null || segmentList.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        lineRenderer.positionCount = segmentList.Count + 1;
        lineRenderer.SetPosition(0, segmentList[0].Item1);
        for (int i = 0; i < segmentList.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, segmentList[i].Item2);
        }
        SelectOffsetFromAnimationMode();
    }
    private void SelectOffsetFromAnimationMode()
    {
        int index = 0;

        switch (animationMode)
        {
            case LightningBoltMode.None:
                //固定第一个贴图
                index = 0;
                break;
            case LightningBoltMode.Random:
                //贴图列表中随机
                index = randomGenerator.Next(0, matTextureOffset.Length);
                break;
            case LightningBoltMode.Loop:
                //贴图列表中循环
                index = animationOffsetIndex++;
                if (animationOffsetIndex >= matTextureOffset.Length)
                    animationOffsetIndex = 0;
                break;
            case LightningBoltMode.PingPong:
                //贴图列表中循环
                index = animationOffsetIndex;
                animationOffsetIndex += animationPingPongDir;
                if (animationOffsetIndex >= matTextureOffset.Length)
                {
                    animationOffsetIndex = matTextureOffset.Length - 2;
                    animationPingPongDir = -1;
                }
                else if (animationOffsetIndex < 0)
                {
                    animationOffsetIndex = 1;
                    animationPingPongDir = 1;
                }
                break;
            default:
                break;
        }

        if (index < 0 || index >= matTextureOffset.Length)
            index = 0;
        lineRenderer.material.mainTextureOffset = matTextureOffset[index];
    }

}