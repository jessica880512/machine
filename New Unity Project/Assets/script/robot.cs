using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using UnityEditor.U2D;

public class robot : Agent
{


    [Header("速度"), Range(1, 50)]
    public float speed = 10;
    private Rigidbody rigrobot;
    private Rigidbody rigball;
    private void Start()
    {
        rigrobot = GetComponent<Rigidbody>();
        rigball =GameObject.Find("ball").GetComponent<Rigidbody>();
     }
    public override void OnEpisodeBegin()
    {
        rigrobot.velocity = Vector3.zero;
        rigrobot.angularVelocity = Vector3.zero;
        rigball.velocity = Vector3.zero;
        rigball.angularVelocity = Vector3.zero;

        Vector3 posrobot = new Vector3(Random.Range(-2f, 2f), 0.1f, Random.Range(-2f, 0f));
        transform.position = posrobot;
        Vector3 posball = new Vector3(Random.Range(-2f, 2f), 0.1f, Random.Range(1f, 2f));
        rigball.position = posball;

        ball.complete = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigball.position);
        sensor.AddObservation(rigrobot.velocity.x);
        sensor.AddObservation(rigrobot.velocity.z);
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        rigrobot.AddForce(control*speed);

        if(ball.complete)
        {
            SetReward(1);
            EndEpisode();
        }
        if(transform.position.y <0 || rigball.position.y<0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
