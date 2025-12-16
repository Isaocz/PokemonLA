using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BindScreenSizeToMaterial : MonoBehaviour
{
    public Renderer rend1;
    public Renderer rend2;
    public string propName = "_mScreenSize";

    void Awake()
    {
        if (rend1 == null) rend1 = GetComponent<Renderer>();
        Vector2 s = new Vector2(transform.localScale.x, transform.localScale.y);
        rend1.sharedMaterial.SetVector(propName, new Vector4(s.x * 160, s.y * 160, 0, 0));
        if (rend2 == null) rend2 = GetComponent<Renderer>();
        rend2.sharedMaterial.SetVector(propName, new Vector4(s.x * 160, s.y * 160, 0, 0));
        //Vector2 s = GetComponent<SpriteRenderer>().size;
        //Debug.Log(s);
        //rend.sharedMaterial.SetVector(propName, new Vector4(s.x, s.y, 0, 0));
    }

    void Update()
    {
        //Vector2 s = new Vector2(Screen.width, Screen.height);
        //rend.sharedMaterial.SetVector(propName, new Vector4(s.x, s.y, 0, 0));
        // 或者使用 MaterialPropertyBlock 对共享材质安全设置
        Vector2 s = new Vector2(transform.localScale.x, transform.localScale.y);
        rend1.sharedMaterial.SetVector(propName, new Vector4(s.x * 160, s.y * 160, 0, 0));
        rend2.sharedMaterial.SetVector(propName, new Vector4(s.x * 160, s.y * 160, 0, 0));
    }
}