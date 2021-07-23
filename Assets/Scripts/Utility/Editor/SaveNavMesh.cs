using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SaveNavMesh : MonoBehaviour
{
    [MenuItem("Tools/MechBattle/NavMesh to Mesh")]
    private static void CreateMesh()
    {
        NavMeshTriangulation navMesh = NavMesh.CalculateTriangulation();
        
        Mesh mesh = new Mesh();
        mesh.vertices = navMesh.vertices;
        mesh.triangles = navMesh.indices;

        ProjectWindowUtil.CreateAsset(mesh, "New NavMesh.mesh");
    }
}
