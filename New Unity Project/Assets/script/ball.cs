using MLAgents;
using UnityEngine;

public class ball : Agent
{
    public static bool complete;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "sensing") 
        {
            complete = true;

        }
    }
}
