using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Robot : Agent
{
    [Header("速度"), Range(1, 50)]
    public float speed = 10;

    private Rigidbody rigRobot;
    private Rigidbody rigBall;
    private void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = GameObject.Find("足球").GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 重新設定機器人
    /// </summary>
    public override void OnEpisodeBegin()
    {
        ///重設鋼體加速度腳速度
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        ///隨機機器人和足球位置
        Vector3 posRobot = new Vector3(Random.Range(-1f, 1f), 0.1f, Random.Range(-1f, 0f));
        transform.position = posRobot;
        Vector3 posBall = new Vector3(Random.Range(-0.5f, 0.5f), 0.1f, Random.Range(1f, 1.5f));
        rigBall.position = posBall;

        ///尚未進入球門
        Ball.complete = false;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        ///機器人與足球的座標 機器人加速度xz
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    /// <summary>
    /// 動作:控制機器人與回饋
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        ///使用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        rigRobot.AddForce(control * speed);
        ///進球加1分
        if (Ball.complete)
        {
            SetReward(1);
            EndEpisode();

        }
        ///機器人使足球掉落失敗扣1分
        if (transform.position.y < 0 || rigBall.position.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

}
