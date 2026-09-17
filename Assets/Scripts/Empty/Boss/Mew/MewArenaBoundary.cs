using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mew Boss 第三阶段限制圈。
///
/// 设计目标：
/// 1. 圆心固定在房间中心，不跟随 Mew 移动。
/// 2. 不创建 Collider2D，因此不会被项目中的 Projectel / Enviroment 碰撞逻辑识别。
/// 3. 玩家接近边缘时逐渐增强警告；普通越界时柔性拉回，严重穿透时才安全钳位。
/// 4. 支持带预警的显形、平滑缩圈、淡出销毁。
/// 5. 所有视觉均由 LineRenderer 与运行时生成的柔光贴图完成，无需额外素材。
/// </summary>
[DefaultExecutionOrder(10000)]
public sealed class MewArenaBoundary : MonoBehaviour
{
    [Header("限制参数")]
    [SerializeField, Min(1f)] private float defaultRadius = 19f;
    [SerializeField, Min(0f)] private float playerPadding = 0.25f;
    [SerializeField, Min(0.1f)] private float warningZoneWidth = 2.5f;
    [SerializeField] private bool cancelOutwardVelocity = true;
    [SerializeField] private bool enforceDuringIntro = false;

    [Header("柔性回拉")]
    [Tooltip("即使 enforceDuringIntro 为 false，显形期间也会使用柔性回拉，而不是显形结束后瞬间钳位。")]
    [SerializeField] private bool softCaptureDuringIntro = true;
    [Min(0.1f)]
    [SerializeField] private float softReturnBaseSpeed = 3.8f;
    [Min(0f)]
    [SerializeField] private float softReturnSpeedPerUnit = 2.2f;
    [Min(0.1f)]
    [SerializeField] private float softReturnMaximumSpeed = 16f;
    [Tooltip("玩家超出边界达到该距离时才使用最终安全钳位，防止高速移动穿出房间。")]
    [Min(0.5f)]
    [SerializeField] private float hardSafetyOverflow = 7f;
    [Tooltip("逐渐削弱向外速度的强度；不是瞬间清零。")]
    [Min(0f)]
    [SerializeField] private float outwardVelocityDamping = 10f;

    [Header("显形与缩圈")]
    [SerializeField, Min(0f)] private float defaultIntroDuration = 1.25f;
    [SerializeField, Min(0f)] private float defaultResizeDuration = 1.25f;
    [SerializeField, Min(0f)] private float deactivateDuration = 0.65f;
    [SerializeField, Range(1f, 1.5f)] private float introStartScale = 1.18f;

    [Header("可选接触特效")]
    [SerializeField] private GameObject edgeContactEffectPrefab;
    [SerializeField, Min(0.02f)] private float edgeEffectCooldown = 0.09f;
    [SerializeField, Min(0.05f)] private float edgeEffectLifetime = 0.4f;

    [Header("视觉")]
    [SerializeField, Range(48, 256)] private int circleSegments = 160;
    [SerializeField, Range(6, 24)] private int arcCount = 12;
    [SerializeField, Range(4, 24)] private int runeCount = 12;
    [SerializeField, Min(0.01f)] private float mainLineWidth = 0.12f;
    [SerializeField, Min(0.01f)] private float glowLineWidth = 0.48f;
    [SerializeField, Min(0f)] private float innerRingOffset = 0.48f;
    [SerializeField, Min(0f)] private float arcRadiusOffset = 0.24f;
    [SerializeField] private float outerArcRotationSpeed = 18f;
    [SerializeField] private float runeRotationSpeed = -11f;
    [SerializeField, Min(0f)] private float breathingAmount = 0.055f;
    [SerializeField, Min(0f)] private float breathingSpeed = 2.1f;
    [SerializeField] private int sortingOrder = 80;

    [Header("颜色")]
    [SerializeField] private Color psychicPink = new Color(1f, 0.29f, 0.79f, 1f);
    [SerializeField] private Color psychicCyan = new Color(0.30f, 0.92f, 1f, 1f);
    [SerializeField] private Color innerViolet = new Color(0.61f, 0.34f, 1f, 1f);
    [SerializeField] private Color dangerColor = new Color(1f, 0.12f, 0.46f, 1f);

    public bool IsActive { get; private set; }
    public float CurrentRadius { get { return currentRadius; } }
    public float TargetRadius { get { return targetRadius; } }
    public Vector2 Center { get { return center; } }
    public float EdgeIntensity { get; private set; }

    private Transform playerTransform;
    private Rigidbody2D playerBody;
    private Vector2 center;

    private float currentRadius;
    private float targetRadius;
    private float resizeFromRadius;
    private float resizeTimer;
    private float resizeDuration;

    private float introTimer;
    private float introDuration;
    private float visibleAlpha;
    private bool isDeactivating;
    private float deactivateTimer;

    private float arcRotation;
    private float runeRotation;
    private float edgeEffectTimer;
    private bool visualsBuilt;

    private Material lineMaterial;
    private Texture2D auraTexture;
    private Sprite auraSprite;
    private SpriteRenderer auraRenderer;

    private LineRenderer outerGlow;
    private LineRenderer mainRing;
    private LineRenderer innerRing;
    private LineRenderer playerWarningArc;
    private readonly List<LineRenderer> arcRenderers = new List<LineRenderer>();
    private readonly List<LineRenderer> runeRenderers = new List<LineRenderer>();

    private void Awake()
    {
        currentRadius = defaultRadius;
        targetRadius = defaultRadius;
        BuildVisuals();
        SetVisualAlpha(0f);
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (lineMaterial != null)
            {
                Destroy(lineMaterial);
            }

            if (auraSprite != null)
            {
                Destroy(auraSprite);
            }

            if (auraTexture != null)
            {
                Destroy(auraTexture);
            }
        }
    }

    /// <summary>
    /// 激活限制圈。建议 center 传入 Mew 三阶段专用房间的固定中心 mapCenter。
    /// </summary>
    public void Activate(Vector2 arenaCenter, Transform targetPlayer, float radius, float introSeconds = -1f)
    {
        BuildVisuals();

        center = arenaCenter;
        transform.position = new Vector3(center.x, center.y, transform.position.z);

        playerTransform = targetPlayer;
        playerBody = playerTransform != null ? playerTransform.GetComponent<Rigidbody2D>() : null;

        targetRadius = Mathf.Max(1f, radius);
        currentRadius = targetRadius * introStartScale;
        resizeFromRadius = currentRadius;
        resizeTimer = 0f;
        resizeDuration = Mathf.Max(0.01f, introSeconds >= 0f ? introSeconds : defaultIntroDuration);

        introDuration = Mathf.Max(0f, introSeconds >= 0f ? introSeconds : defaultIntroDuration);
        introTimer = 0f;
        deactivateTimer = 0f;
        isDeactivating = false;
        IsActive = true;
        visibleAlpha = 0f;

        UpdateAllVisuals(0f);
    }

    public void SetCenter(Vector2 arenaCenter)
    {
        center = arenaCenter;
        transform.position = new Vector3(center.x, center.y, transform.position.z);
    }

    public void SetContactEffect(GameObject effectPrefab)
    {
        edgeContactEffectPrefab = effectPrefab;
    }

    /// <summary>
    /// 平滑调整限制圈半径。重复传入同一个半径不会重新开始插值。
    /// </summary>
    public void SetTargetRadius(float radius, float duration = -1f)
    {
        radius = Mathf.Max(1f, radius);
        if (Mathf.Abs(radius - targetRadius) < 0.001f)
        {
            return;
        }

        resizeFromRadius = currentRadius;
        targetRadius = radius;
        resizeTimer = 0f;
        resizeDuration = Mathf.Max(0.01f, duration >= 0f ? duration : defaultResizeDuration);
    }

    public void Deactivate(float duration = -1f, bool destroyAfterFade = true)
    {
        if (!IsActive && !isDeactivating)
        {
            if (destroyAfterFade)
            {
                Destroy(gameObject);
            }
            return;
        }

        IsActive = false;
        isDeactivating = true;
        deactivateTimer = 0f;
        deactivateDuration = Mathf.Max(0.01f, duration >= 0f ? duration : deactivateDuration);

        if (destroyAfterFade)
        {
            Destroy(gameObject, deactivateDuration + 0.08f);
        }
    }

    private void Update()
    {
        if (!IsActive && !isDeactivating)
        {
            return;
        }

        float dt = Time.deltaTime;
        edgeEffectTimer = Mathf.Max(0f, edgeEffectTimer - dt);

        if (IsActive)
        {
            introTimer += dt;
            float intro01 = introDuration <= 0f ? 1f : Mathf.Clamp01(introTimer / introDuration);
            visibleAlpha = Smooth01(intro01);

            resizeTimer += dt;
            float resize01 = resizeDuration <= 0f ? 1f : Mathf.Clamp01(resizeTimer / resizeDuration);
            currentRadius = Mathf.Lerp(resizeFromRadius, targetRadius, Smooth01(resize01));

            arcRotation = Mathf.Repeat(arcRotation + outerArcRotationSpeed * dt, 360f);
            runeRotation = Mathf.Repeat(runeRotation + runeRotationSpeed * dt, 360f);

            UpdateEdgeIntensity();
            UpdateAllVisuals(Time.time);
        }
        else if (isDeactivating)
        {
            deactivateTimer += dt;
            float fade01 = Mathf.Clamp01(deactivateTimer / Mathf.Max(0.01f, deactivateDuration));
            visibleAlpha = 1f - Smooth01(fade01);
            EdgeIntensity = 0f;
            UpdateAllVisuals(Time.time);
        }
    }

    /// <summary>
    /// 运行顺序被设为很靠后，以便在 PlayerControler 修改 Rigidbody2D.position 后再进行边界修正。
    /// </summary>
    private void FixedUpdate()
    {
        if (!CanEnforceBoundary())
        {
            return;
        }

        // Rigidbody2D 只在 FixedUpdate 中修正，避免旧版 FixedUpdate + LateUpdate 双重钳位。
        if (playerBody != null)
        {
            ConstrainPlayer(Time.fixedDeltaTime, true);
        }
    }

    private void LateUpdate()
    {
        if (!CanEnforceBoundary())
        {
            return;
        }

        // 没有刚体、由 transform 直接移动的玩家才在 LateUpdate 中柔性修正。
        if (playerBody == null)
        {
            ConstrainPlayer(Time.deltaTime, false);
            return;
        }

        // 对真正的高速穿透只保留最终安全兜底；一般越界不会瞬移。
        HardSafetyCheck();
    }

    private bool CanEnforceBoundary()
    {
        if (!IsActive || playerTransform == null)
        {
            return false;
        }

        if (introTimer < introDuration &&
            !enforceDuringIntro &&
            !softCaptureDuringIntro)
        {
            return false;
        }

        return true;
    }

    private void ConstrainPlayer(float deltaTime, bool useRigidbody)
    {
        Vector2 playerPosition = useRigidbody && playerBody != null
            ? playerBody.position
            : (Vector2)playerTransform.position;

        Vector2 delta = playerPosition - center;
        float distance = delta.magnitude;
        float maxDistance = Mathf.Max(0.1f, currentRadius - playerPadding);
        float overflow = distance - maxDistance;

        if (overflow <= 0f)
        {
            return;
        }

        Vector2 direction = distance > 0.000001f
            ? delta / distance
            : Vector2.right;
        Vector2 boundaryPosition = center + direction * maxDistance;

        // 只在严重穿透时瞬时兜底；普通越界按距离逐渐加快地拉回。
        bool needsHardSafety = overflow >= Mathf.Max(0.5f, hardSafetyOverflow);
        float returnSpeed = Mathf.Clamp(
            softReturnBaseSpeed + overflow * softReturnSpeedPerUnit,
            0.1f,
            Mathf.Max(0.1f, softReturnMaximumSpeed));

        Vector2 correctedPosition = needsHardSafety
            ? boundaryPosition
            : Vector2.MoveTowards(
                playerPosition,
                boundaryPosition,
                returnSpeed * Mathf.Max(0f, deltaTime));

        if (useRigidbody && playerBody != null)
        {
            playerBody.MovePosition(correctedPosition);

            if (cancelOutwardVelocity)
            {
                Vector2 velocity = playerBody.velocity;
                float outwardSpeed = Vector2.Dot(velocity, direction);
                if (outwardSpeed > 0f)
                {
                    float damping01 = 1f - Mathf.Exp(
                        -Mathf.Max(0f, outwardVelocityDamping) *
                        Mathf.Max(0f, deltaTime));
                    playerBody.velocity =
                        velocity - direction * outwardSpeed * damping01;
                }
            }
        }
        else
        {
            Vector3 p = playerTransform.position;
            playerTransform.position = new Vector3(
                correctedPosition.x,
                correctedPosition.y,
                p.z);
        }

        EdgeIntensity = Mathf.Max(
            EdgeIntensity,
            Mathf.Clamp01(overflow / Mathf.Max(0.1f, warningZoneWidth)));
        SpawnEdgeContactEffect(correctedPosition, direction);
    }

    private void HardSafetyCheck()
    {
        if (playerBody == null)
        {
            return;
        }

        Vector2 playerPosition = playerBody.position;
        Vector2 delta = playerPosition - center;
        float distance = delta.magnitude;
        float maxDistance = Mathf.Max(0.1f, currentRadius - playerPadding);
        float overflow = distance - maxDistance;

        if (overflow < Mathf.Max(0.5f, hardSafetyOverflow))
        {
            return;
        }

        Vector2 direction = distance > 0.000001f
            ? delta / distance
            : Vector2.right;
        Vector2 safePosition = center + direction * maxDistance;
        playerBody.position = safePosition;

        if (cancelOutwardVelocity)
        {
            Vector2 velocity = playerBody.velocity;
            float outwardSpeed = Vector2.Dot(velocity, direction);
            if (outwardSpeed > 0f)
            {
                playerBody.velocity = velocity - direction * outwardSpeed;
            }
        }

        EdgeIntensity = 1f;
        SpawnEdgeContactEffect(safePosition, direction);
    }

    private void SpawnEdgeContactEffect(Vector2 position, Vector2 outwardDirection)
    {
        if (edgeContactEffectPrefab == null || edgeEffectTimer > 0f)
        {
            return;
        }

        float angle = Mathf.Atan2(outwardDirection.y, outwardDirection.x) * Mathf.Rad2Deg - 90f;
        GameObject effect = Instantiate(
            edgeContactEffectPrefab,
            position,
            Quaternion.Euler(0f, 0f, angle));
        Destroy(effect, edgeEffectLifetime);
        edgeEffectTimer = edgeEffectCooldown;
    }

    private void UpdateEdgeIntensity()
    {
        if (playerTransform == null)
        {
            EdgeIntensity = 0f;
            return;
        }

        Vector2 playerPosition = playerBody != null
            ? playerBody.position
            : (Vector2)playerTransform.position;

        float distance = Vector2.Distance(playerPosition, center);
        EdgeIntensity = Mathf.Clamp01(Mathf.InverseLerp(
            currentRadius - warningZoneWidth,
            currentRadius,
            distance));
    }

    private void BuildVisuals()
    {
        if (visualsBuilt)
        {
            return;
        }

        visualsBuilt = true;
        transform.name = "Mew Arena Boundary";

        Shader spriteShader = Shader.Find("Sprites/Default");
        if (spriteShader == null)
        {
            spriteShader = Shader.Find("Unlit/Color");
        }

        lineMaterial = new Material(spriteShader);
        lineMaterial.name = "Mew Arena Boundary Runtime Material";

        auraRenderer = CreateAuraRenderer();
        outerGlow = CreateLine("Outer Glow", glowLineWidth, sortingOrder + 1, true);
        mainRing = CreateLine("Main Ring", mainLineWidth, sortingOrder + 4, true);
        innerRing = CreateLine("Inner Ring", mainLineWidth * 0.58f, sortingOrder + 3, true);
        playerWarningArc = CreateLine("Player Warning Arc", mainLineWidth * 2.4f, sortingOrder + 8, false);

        arcRenderers.Clear();
        int safeArcCount = Mathf.Max(1, arcCount);
        for (int i = 0; i < safeArcCount; i++)
        {
            LineRenderer arc = CreateLine("Rotating Arc " + i, mainLineWidth * 1.15f, sortingOrder + 6, false);
            arc.widthCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.3f, 1f), new Keyframe(1f, 0f));
            arcRenderers.Add(arc);
        }

        runeRenderers.Clear();
        int safeRuneCount = Mathf.Max(1, runeCount);
        for (int i = 0; i < safeRuneCount; i++)
        {
            LineRenderer rune = CreateLine("Psychic Rune " + i, mainLineWidth * 0.72f, sortingOrder + 7, false);
            rune.positionCount = 9;
            runeRenderers.Add(rune);
        }
    }

    private SpriteRenderer CreateAuraRenderer()
    {
        GameObject auraObject = new GameObject("Psychic Veil");
        auraObject.transform.SetParent(transform, false);

        SpriteRenderer renderer = auraObject.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = sortingOrder;
        renderer.sprite = CreateAuraSprite();
        renderer.color = Color.white;
        return renderer;
    }

    private Sprite CreateAuraSprite()
    {
        const int size = 256;
        auraTexture = new Texture2D(size, size, TextureFormat.RGBA32, false, true);
        auraTexture.name = "Mew Arena Boundary Aura";
        auraTexture.wrapMode = TextureWrapMode.Clamp;
        auraTexture.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[size * size];
        Vector2 textureCenter = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
        float maxRadius = size * 0.5f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float normalizedRadius = Vector2.Distance(new Vector2(x, y), textureCenter) / maxRadius;

                float edgeBand = Mathf.Clamp01(1f - Mathf.Abs(normalizedRadius - 0.91f) / 0.09f);
                edgeBand = edgeBand * edgeBand * (3f - 2f * edgeBand);

                float insideVeil = Mathf.Clamp01(Mathf.InverseLerp(0.62f, 0.96f, normalizedRadius));
                float circularCutoff = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.94f, 1f, normalizedRadius));
                float alpha = Mathf.Clamp01(edgeBand * 0.72f + insideVeil * 0.12f) * circularCutoff;

                float cyanMix = Mathf.Clamp01(Mathf.InverseLerp(0.82f, 1f, normalizedRadius));
                Color c = Color.Lerp(innerViolet, psychicCyan, cyanMix);
                c.a = alpha;
                pixels[y * size + x] = c;
            }
        }

        auraTexture.SetPixels(pixels);
        auraTexture.Apply(false, true);

        auraSprite = Sprite.Create(
            auraTexture,
            new Rect(0f, 0f, size, size),
            new Vector2(0.5f, 0.5f),
            size);
        auraSprite.name = "Mew Arena Boundary Aura Sprite";
        return auraSprite;
    }

    private LineRenderer CreateLine(string objectName, float width, int order, bool loop)
    {
        GameObject lineObject = new GameObject(objectName);
        lineObject.transform.SetParent(transform, false);

        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.loop = loop;
        line.alignment = LineAlignment.TransformZ;
        line.textureMode = LineTextureMode.Stretch;
        line.numCapVertices = 4;
        line.numCornerVertices = 4;
        line.widthMultiplier = width;
        line.sharedMaterial = lineMaterial;
        line.sortingOrder = order;
        return line;
    }

    private void UpdateAllVisuals(float time)
    {
        if (!visualsBuilt)
        {
            return;
        }

        float edgePulse = EdgeIntensity * (0.70f + 0.30f * Mathf.Sin(time * 9f));
        float breathing = Mathf.Sin(time * breathingSpeed) * breathingAmount;
        float visualRadius = Mathf.Max(0.1f, currentRadius + breathing);

        Color mainColor = Color.Lerp(psychicPink, dangerColor, edgePulse);
        Color cyanColor = Color.Lerp(psychicCyan, dangerColor, edgePulse * 0.72f);
        Color violetColor = Color.Lerp(innerViolet, dangerColor, edgePulse * 0.48f);

        UpdateCircle(outerGlow, visualRadius, circleSegments);
        UpdateCircle(mainRing, visualRadius, circleSegments);
        UpdateCircle(innerRing, Mathf.Max(0.1f, visualRadius - innerRingOffset), circleSegments);

        SetLineColor(outerGlow, WithAlpha(mainColor, visibleAlpha * (0.16f + edgePulse * 0.18f)));
        outerGlow.widthMultiplier = glowLineWidth * (1f + edgePulse * 0.32f);

        Gradient mainGradient = CreateAlternatingGradient(
            WithAlpha(mainColor, visibleAlpha),
            WithAlpha(cyanColor, visibleAlpha));
        mainRing.colorGradient = mainGradient;
        mainRing.widthMultiplier = mainLineWidth * (1f + edgePulse * 0.42f);

        SetLineColor(innerRing, WithAlpha(violetColor, visibleAlpha * 0.38f));

        UpdateRotatingArcs(visualRadius, mainColor, cyanColor);
        UpdateRunes(visualRadius, violetColor, cyanColor);
        UpdatePlayerWarningArc(visualRadius, edgePulse);

        if (auraRenderer != null)
        {
            float auraDiameter = visualRadius * 2.08f;
            auraRenderer.transform.localScale = new Vector3(auraDiameter, auraDiameter, 1f);
            Color auraColor = Color.Lerp(innerViolet, dangerColor, edgePulse * 0.65f);
            auraColor.a = visibleAlpha * (0.18f + EdgeIntensity * 0.12f);
            auraRenderer.color = auraColor;
        }
    }

    private void UpdateCircle(LineRenderer line, float radius, int segments)
    {
        int count = Mathf.Max(12, segments);
        line.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            float angle = i / (float)count * Mathf.PI * 2f;
            line.SetPosition(i, new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f));
        }
    }

    private void UpdateRotatingArcs(float radius, Color colorA, Color colorB)
    {
        int count = arcRenderers.Count;
        if (count == 0)
        {
            return;
        }

        float step = 360f / count;
        float arcLength = step * 0.48f;
        const int pointsPerArc = 12;

        for (int i = 0; i < count; i++)
        {
            LineRenderer arc = arcRenderers[i];
            arc.positionCount = pointsPerArc;

            float startDegrees = arcRotation + i * step;
            float arcRadius = radius + arcRadiusOffset + (i % 2 == 0 ? 0.06f : -0.04f);

            for (int p = 0; p < pointsPerArc; p++)
            {
                float t = p / (float)(pointsPerArc - 1);
                float angle = (startDegrees + arcLength * t) * Mathf.Deg2Rad;
                arc.SetPosition(p, new Vector3(
                    Mathf.Cos(angle) * arcRadius,
                    Mathf.Sin(angle) * arcRadius,
                    0f));
            }

            Color c = i % 2 == 0 ? colorA : colorB;
            SetLineColor(arc, WithAlpha(c, visibleAlpha * 0.86f));
        }
    }

    private void UpdateRunes(float radius, Color colorA, Color colorB)
    {
        int count = runeRenderers.Count;
        if (count == 0)
        {
            return;
        }

        float step = 360f / count;
        float runeRadius = radius + 0.50f;
        float runeHalfWidth = 0.13f;
        float runeHalfHeight = 0.28f;

        for (int i = 0; i < count; i++)
        {
            float angleDegrees = runeRotation + i * step;
            float angle = angleDegrees * Mathf.Deg2Rad;
            Vector2 radial = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 tangent = new Vector2(-radial.y, radial.x);
            Vector2 position = radial * runeRadius;

            LineRenderer rune = runeRenderers[i];
            for (int point = 0; point <= 8; point++)
            {
                float a = point * Mathf.PI * 0.25f;
                float inset = point % 2 == 0 ? 1f : 0.3f;
                rune.SetPosition(point, position + (radial * Mathf.Cos(a) * runeHalfHeight +
                    tangent * Mathf.Sin(a) * runeHalfWidth) * inset);
            }

            Color c = i % 2 == 0 ? colorA : colorB;
            SetLineColor(rune, WithAlpha(c, visibleAlpha * 0.82f));
        }
    }

    private void UpdatePlayerWarningArc(float radius, float edgePulse)
    {
        if (playerTransform == null || EdgeIntensity <= 0.001f)
        {
            playerWarningArc.positionCount = 0;
            return;
        }

        Vector2 playerPosition = playerBody != null
            ? playerBody.position
            : (Vector2)playerTransform.position;
        Vector2 direction = playerPosition - center;
        if (direction.sqrMagnitude < 0.0001f)
        {
            playerWarningArc.positionCount = 0;
            return;
        }

        float centerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float halfAngle = Mathf.Lerp(9f, 24f, EdgeIntensity);
        int pointCount = 28;
        playerWarningArc.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            float angle = Mathf.Lerp(centerAngle - halfAngle, centerAngle + halfAngle, t) * Mathf.Deg2Rad;
            float arcRadius = radius + 0.035f;
            playerWarningArc.SetPosition(i, new Vector3(
                Mathf.Cos(angle) * arcRadius,
                Mathf.Sin(angle) * arcRadius,
                0f));
        }

        Color warningColor = Color.Lerp(psychicPink, dangerColor, Mathf.Max(EdgeIntensity, edgePulse));
        SetLineColor(playerWarningArc, WithAlpha(warningColor, visibleAlpha * EdgeIntensity));
        playerWarningArc.widthMultiplier = mainLineWidth * Mathf.Lerp(1.3f, 3.2f, EdgeIntensity);
    }

    private void SetVisualAlpha(float alpha)
    {
        visibleAlpha = Mathf.Clamp01(alpha);
        UpdateAllVisuals(0f);
    }

    private void SetLineColor(LineRenderer line, Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }

    private readonly Gradient ringGradient = new Gradient();
    private readonly GradientColorKey[] ringColors = new GradientColorKey[5];
    private readonly GradientAlphaKey[] ringAlphas = new GradientAlphaKey[5];

    private Gradient CreateAlternatingGradient(Color a, Color b)
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = i % 2 == 0 ? a : b;
            ringColors[i] = new GradientColorKey(color, i * 0.25f);
            ringAlphas[i] = new GradientAlphaKey(color.a, i * 0.25f);
        }
        ringGradient.SetKeys(ringColors, ringAlphas);
        return ringGradient;
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        color.a = Mathf.Clamp01(alpha);
        return color;
    }

    private static float Smooth01(float t)
    {
        t = Mathf.Clamp01(t);
        return t * t * (3f - 2f * t);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 gizmoCenter = Application.isPlaying
            ? new Vector3(center.x, center.y, 0f)
            : transform.position;
        float gizmoRadius = Application.isPlaying ? currentRadius : defaultRadius;

        Gizmos.color = psychicPink;
        const int segmentCount = 64;
        Vector3 previous = gizmoCenter + Vector3.right * gizmoRadius;
        for (int i = 1; i <= segmentCount; i++)
        {
            float angle = i / (float)segmentCount * Mathf.PI * 2f;
            Vector3 next = gizmoCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * gizmoRadius;
            Gizmos.DrawLine(previous, next);
            previous = next;
        }
    }
#endif
}
