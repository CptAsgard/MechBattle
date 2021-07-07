using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Mesh spawnGizmo;
    [SerializeField]
    private float gizmoScale = 0.04f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawMesh(spawnGizmo, transform.position, transform.rotation, Vector3.one * gizmoScale);
    }
}
