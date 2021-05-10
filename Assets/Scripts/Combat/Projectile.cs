using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 currentPosition;
    private Vector3 currentVelocity;

    private void FixedUpdate()
    {
        StepBullet();
    }

    public void Initialize(Vector3 initialPosition, Vector3 muzzleVelocity)
    {
        currentPosition = initialPosition;
        currentVelocity = muzzleVelocity;
    }

    private void StepBullet()
    {
        IntegrationMethod.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }
}
