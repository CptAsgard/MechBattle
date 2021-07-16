using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedReticlePrefab;
    [SerializeField]
    private GameObject targetReticlePrefab;

    private MechSelectActions selectActions;
    private TargetReticle selectedReticle;

    private void Start()
    {
        selectedReticle = Instantiate(selectedReticlePrefab, transform).GetComponent<TargetReticle>();
    }

    private void Update()
    {
        if (selectActions == null)
        {
            selectActions = FindObjectOfType<MechSelectActions>();
            return;
        }

        GameObject selected = selectActions.MechSelectionState.selected?.gameObject;
        selectedReticle.SetTarget(selected);
        
        if (selected == null)
        {
            return;
        }

        GameObject targetEnemy = selected.GetComponent<TurretTargetRepository>().PriorityTarget?.gameObject;
        if (targetEnemy == null)
        {
            return;
        }

        // draw enemy target reticle
    }
}
