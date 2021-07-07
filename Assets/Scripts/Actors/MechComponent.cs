using UnityEngine;

public class MechComponent
{
    public MechComponentLocation Location;
    public float Health;

    public MechComponent()
    {
    }

    public MechComponent(MechComponentLocation location, float health)
    {
        Location = location;
        Health = health;
    }
}
