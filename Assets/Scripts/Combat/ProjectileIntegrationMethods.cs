using UnityEngine;

public static class ProjectileIntegrationMethods
{
    private static Vector3 gravity = new Vector3(0f, -9.81f, 0f);

    public static void HeunsNoExternalForces(float timeStep, Vector3 currentPosition, Vector3 currentVelocity, out Vector3 newPosition, out Vector3 newVelocity)
    {
        Vector3 accFactor = Physics.gravity;
        Vector3 newVelEuler = currentVelocity + timeStep * accFactor;

        newVelocity = currentVelocity + timeStep * 0.5f * (accFactor + accFactor);
        newPosition = currentPosition + timeStep * 0.5f * (currentVelocity + newVelEuler);
    }
}
