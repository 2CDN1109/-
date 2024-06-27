using UnityEngine;

public class ApplyMaterialToObjects : MonoBehaviour
{
    public Material materialToApply;

    void Start()
    {
        // 全てのオブジェクトを取得します。
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // MeshRendererを持っているオブジェクトに対してマテリアルを適用します。
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = materialToApply;
            }
        }
    }
}
