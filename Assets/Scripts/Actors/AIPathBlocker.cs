using Pathfinding;
using UnityEngine;

public class AIPathBlocker : MonoBehaviour
{
    public int seekerTag = 0;

    [SerializeField]
    private Vector3 seekerSize = Vector3.zero;
    [SerializeField]
    private float updateRate = 0.2f;

    private float timer = 0f;
    private GraphUpdateObject previousGuo = null;

    private void Start()
    {
        timer = updateRate;
        GetComponentInParent<Seeker>().traversableTags = Mathf.RoundToInt(Mathf.Pow(2, seekerTag)) + 1;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0f)
        {
            return;
        }

        timer = updateRate;
        UpdateGraph();
    }

    private void UpdateGraph()
    {
        if (previousGuo != null)
        {
            previousGuo.setTag = 0;
            AstarPath.active.UpdateGraphs(previousGuo);
        }

        Bounds bounds = new Bounds(transform.position, seekerSize);
        GraphUpdateObject guo = new GraphUpdateObject(bounds)
        {
            modifyTag = true, 
            setTag = seekerTag
        };

        AstarPath.active.UpdateGraphs(guo);
        previousGuo = guo;
    }
}
