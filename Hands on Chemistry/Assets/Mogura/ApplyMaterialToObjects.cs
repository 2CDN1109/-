using UnityEngine;

public class ApplyMaterialToObjects : MonoBehaviour
{
    public Material materialToApply;

    void Start()
    {
        // �S�ẴI�u�W�F�N�g���擾���܂��B
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // MeshRenderer�������Ă���I�u�W�F�N�g�ɑ΂��ă}�e���A����K�p���܂��B
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = materialToApply;
            }
        }
    }
}
