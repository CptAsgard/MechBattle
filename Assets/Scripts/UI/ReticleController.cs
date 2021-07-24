using System.Linq;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedReticlePrefab;
    [SerializeField]
    private GameObject targetReticlePrefab;

    private MechSelectActions selectActions;
    private TargetReticle selectedReticle;
    private TargetReticle enemyReticle;

    private void Start()
    {
        selectedReticle = Instantiate(selectedReticlePrefab, transform).GetComponent<TargetReticle>();
        enemyReticle = Instantiate(targetReticlePrefab, transform).GetComponent<TargetReticle>();
    }

    private void Update()
    {
        if (selectActions == null)
        {
            selectActions = FindObjectOfType<MechSelectActions>();
            return;
        }

        GameObject selected = MechSelectActions.MechSelectionState.selected?.gameObject;
        selectedReticle.SetTarget(selected);
        
        GameObject targetEnemy = selected?.GetComponent<WeaponTargetRepository>().Targets.FirstOrDefault()?.gameObject;
        enemyReticle.SetTarget(targetEnemy);
    }
}
