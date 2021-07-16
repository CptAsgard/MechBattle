using UnityEditor;
using UnityEngine;

public class ForwardConeEditor : EditorWindow
{
    private float angleDegrees;
    private float distance;
    private float degreesPerVertex;

    [MenuItem("Window/MechBattle/Generate Forward Cone")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ForwardConeEditor window = (ForwardConeEditor) GetWindow(typeof(ForwardConeEditor));
        window.Show();
    }

    public void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);

        angleDegrees = EditorGUILayout.FloatField("Angle (degrees)", angleDegrees);
        distance = EditorGUILayout.FloatField("Cone length", distance);
        degreesPerVertex = EditorGUILayout.FloatField("Degrees per vertex", degreesPerVertex);
        
        if (GUILayout.Button("Create Cone"))
        {
            Mesh cone = CreateCone();
            ProjectWindowUtil.CreateAsset(cone, "New Forward Cone.mesh");
        }
    }

    private Mesh CreateCone()
    {
        Mesh mesh = new Mesh();

        int vertexCount = Mathf.Max(3, Mathf.RoundToInt(angleDegrees / degreesPerVertex) + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        float realAnglePerVertex = angleDegrees / (vertexCount - 2);
        for (int i = 1; i < vertexCount - 1; i++)
        {
            float currentAngle = -(angleDegrees / 2) + (i - 1) * realAnglePerVertex;
            Vector3 currentVertex = new Vector3(Mathf.Sin(Mathf.Deg2Rad * currentAngle), 0,
                Mathf.Cos(Mathf.Deg2Rad * currentAngle)) * distance;

            float nextAngle = -(angleDegrees / 2) + i * realAnglePerVertex;
            Vector3 nextVertex = new Vector3(Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0,
                Mathf.Cos(Mathf.Deg2Rad * nextAngle)) * distance;

            vertices[i] = currentVertex;
            vertices[i + 1] = nextVertex;

            triangles[3 * (i - 1)] = 0;
            triangles[3 * (i - 1) + 1] = i;
            triangles[3 * (i - 1) + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = new Vector2[vertexCount];
        mesh.normals = new Vector3[vertexCount];

        return mesh;
    }
}
