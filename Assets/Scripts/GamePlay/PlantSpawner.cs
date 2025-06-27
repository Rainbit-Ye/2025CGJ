using UnityEngine;

[ExecuteInEditMode] // 允许在编辑器预览
public class PlantSpawner2D : MonoBehaviour
{
    [Header("🌱 植物设置")]
    public GameObject plantPrefab;    // 植物预制体（必须有SpriteRenderer）
    [Range(1, 20)] public int count = 5;     // 生成数量
    public float spacing = 1f;       // 间距（单位：Unity单位）
    public Vector2 startPos = new Vector2(-2, 0); // 起始位置

    [Header("🎛️ 物理效果")]
    public bool addCollider = true;  // 自动添加碰撞体
    public bool isStatic = false;    // 是否静态（无刚体）

    [Header("🔧 调试")]
    public bool previewInEditor = true; // 编辑器预览

    void Start()
    {
        SpawnPlants();
    }

    // 生成植物
    void SpawnPlants()
    {
        ClearExistingPlants(); // 清理旧植物

        if (plantPrefab == null)
        {
            Debug.LogError("⚠️ 未分配植物预制体！");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = startPos + Vector2.right * (i * spacing);
            GameObject plant = Instantiate(plantPrefab, spawnPos, Quaternion.identity, transform);

            // 自动配置2D物理组件
            if (!isStatic && plant.GetComponent<Rigidbody2D>() == null)
            {
                var rb = plant.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0; // 无重力（按需调整）
            }

            if (addCollider && plant.GetComponent<Collider2D>() == null)
            {
                plant.AddComponent<BoxCollider2D>(); // 默认方框碰撞体
            }
        }
    }

    // 清除已生成的植物
    void ClearExistingPlants()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    // 编辑器可视化
    void OnDrawGizmos()
    {
        if (!previewInEditor || plantPrefab == null) return;

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = startPos + Vector2.right * (i * spacing);
            Gizmos.DrawWireCube(pos, GetPlantSize());
        }
    }

    // 获取预制体尺寸（用于预览）
    Vector3 GetPlantSize()
    {
        var renderer = plantPrefab.GetComponent<SpriteRenderer>();
        return renderer != null ? renderer.bounds.size : Vector3.one;
    }
}