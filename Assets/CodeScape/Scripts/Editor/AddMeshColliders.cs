using UnityEngine;
using UnityEditor;

public class AddMeshColliders : EditorWindow
{
    [MenuItem("Tools/Add Mesh Colliders")]
    public static void ShowWindow()
    {
        GetWindow<AddMeshColliders>("Add Mesh Colliders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add Mesh Colliders to Selected GameObject", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Mesh Colliders"))
        {
            AddCollidersToSelectedGameObject();
        }
    }

    private void AddCollidersToSelectedGameObject()
    {
        GameObject selectedGameObject = Selection.activeGameObject;

        if (selectedGameObject == null)
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject in the hierarchy.");
            return;
        }

        MeshRenderer[] meshRenderers = selectedGameObject.GetComponentsInChildren<MeshRenderer>();

        if (meshRenderers.Length == 0)
        {
            Debug.LogWarning("No MeshRenderers found in the selected GameObject or its children.");
            return;
        }

        int colliderCount = 0;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            GameObject obj = meshRenderer.gameObject;

            if (obj.GetComponent<MeshCollider>() == null)
            {
                obj.AddComponent<MeshCollider>();
                colliderCount++;
            }
        }

        Debug.Log($"{colliderCount} MeshColliders added to the selected GameObject and its children.");
    }
}
