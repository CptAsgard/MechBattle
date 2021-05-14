using UnityEngine;

public static class ProjectileIntegrationMethods
{
    public static void HeunsNoExternalForces(float timeStep, Vector3 currentPosition, Vector3 currentVelocity, out Vector3 newPosition, out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        Vector3 acceleartionFactorEuler = Physics.gravity;
        Vector3 acceleartionFactorHeun = Physics.gravity;


        //Init velocity
        //Current velocity
        Vector3 velocityFactor = currentVelocity;
        //Wind velocity
        //velocityFactor += new Vector3(2f, 0f, 3f);


        //
        //Main algorithm
        //
        //Euler forward
        Vector3 pos_E = currentPosition + timeStep * velocityFactor;

        //acceleartionFactorEuler += CalculateDrag(currentVelocity);

        Vector3 vel_E = currentVelocity + timeStep * acceleartionFactorEuler;


        //Heuns method
        Vector3 pos_H = currentPosition + timeStep * 0.5f * (velocityFactor + vel_E);

        //acceleartionFactorHeun += CalculateDrag(vel_E);

        Vector3 vel_H = currentVelocity + timeStep * 0.5f * (acceleartionFactorEuler + acceleartionFactorHeun);


        newPosition = pos_H;
        newVelocity = vel_H;
    }
}
