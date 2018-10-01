using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public static List<Mob> MobInstances = new List<Mob>();

    [SerializeField]
    private float speed;
    [SerializeField]
    private float JumpHeight;

    [SerializeField]
    private Vector3 Destination;

    private Vector3 Goal
    {
        get
        {
            return Destination;
        }
        set
        {
            Destination = value;
            OnGoalChanged.Invoke();
        }
    }

    private PopulationZone SleepPlace;
    private ProductionZone WorkPlace;

    [SerializeField]
    private Vector3 LocalEyePos;

    private event Action OnGoalChanged = new Action(delegate { });

    SphereCollider ComfortZone;

    private List<NeuralNetwork.DataSet> thisDataSetList = new List<NeuralNetwork.DataSet>();

    [SerializeField]
    private float FrontalSensorRange;
    [SerializeField]
    private float LeftSensorRange;
    [SerializeField]
    private float RightSensorRange;


    public Vector3 ResetPos;

    private Rigidbody MobRigidbody;



    public int id;

    private void Awake()
    {
        MobInstances.Add(this);
        id = MobInstances.Count;
        ComfortZone = this.gameObject.GetComponent<SphereCollider>();
        MobRigidbody = gameObject.GetComponent<Rigidbody>();
        // this.gameObject.SetActive(false);
    }

    public IEnumerator EatLoop()
    {
        while (true)
        {
            if (City.Instance.MaterialList.Find(x => x.GetMaterialType() == Enums.Material.greenFood).GetCount() > 0)
            {
                City.Instance.MaterialList.Find(x => x.GetMaterialType() == Enums.Material.greenFood).UseMaterial(City.Instance.foodRations);
            }
            if (City.Instance.MaterialList.Find(x => x.GetMaterialType() == Enums.Material.meat).GetCount() > 0)
            {
                City.Instance.MaterialList.Find(x => x.GetMaterialType() == Enums.Material.meat).UseMaterial(City.Instance.foodRations);
            }
            yield return new WaitForSeconds(1);
        }
    }

    void Start()
    {
        Lastdistance = 10000;
        LastTimeOfMove = 10000;
        ResetPos = transform.position;

        System.Random r = new System.Random();
        StartCoroutine(EatLoop());
        //front, left, right, angle
        //thisDataSetList.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.0, 0.0, 0.5 }, new double[] { 0.5 }));
        ///thisDataSetList.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 0.0, 0.25 }, new double[] { 0.25 }));
        //thisDataSetList.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 0.0, 0.75 }, new double[] { 0.25 }));
        //thisDataSetList.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 1.0, 0.25 }, new double[] { 0.75 }));
        //thisDataSetList.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 1.0, 0.75 }, new double[] { 0.75 }));
        // thisNet.Train(thisDataSetList, 1);
        // thisDataSetList.Clear();
        //this.transform.position = Player.Instance.gameObject.transform.position;
        OnGoalChanged += () => GoToDestination(Goal);
        // Goal = Destination;
    }

    #region WORKPLACE_MANAGMENT
    public void SetWorkPlace(ProductionZone z)
    {
        WorkPlace = z;
        Goal = z.gameObject.transform.position;
    }

    public void UnsetWorkPlace()
    {
        WorkPlace = null;
        Goal = CityCenter.Instance.gameObject.transform.position;
    }

    public ProductionZone GetWorkPlace()
    {
        return WorkPlace;
    }
    #endregion

    #region SLEEPPLACE_MANAGMENT

    public void SetSleepPlace(PopulationZone home)
    {
        SleepPlace = home;
        SleepPlace.Inhabitants.Add(this);
    }

    public void UnsetSleepPlace()
    {
        if (SleepPlace != null)
        {
            SleepPlace.Inhabitants.Remove(this);
            SleepPlace = null;
        }
    }

    public PopulationZone GetSleepPlace()
    {
        return SleepPlace;
    }

    #endregion

    private void OnDestroy()
    {
        MobInstances.Remove(this);
    }

    public void GoToDestination(Vector3 destinationPoint)
    {
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    private void Jump()
    {
        //Debug.Log(this.gameObject.name + " JUMPED");
        MobRigidbody.velocity += new Vector3(0, JumpHeight, 0);
    }

    NeuralNetwork.NeuralNet thisNet = new NeuralNetwork.NeuralNet(4, 3, 1);

    [SerializeField]
    private float TimeOfMove;

    private float LastTimeOfMove;
    private float Lastdistance;

    private void ResetPosition()
    {
        if (Vector3.Distance(transform.position, Goal) < Lastdistance)
        {
            Lastdistance = Vector3.Distance(transform.position, Goal);
            thisDataSetList.Clear();
        }
        this.gameObject.transform.position = ResetPos;
        StopAllCoroutines();
        Goal = Destination;
    }
    private void Update()
    {
    }

    private IEnumerator Move()
    {
        RaycastHit hit;
        Vector3 deltaPos = this.gameObject.transform.position;
        Vector3 prevPos = this.gameObject.transform.position;

        double FrontSensor = 1.0;
        double RightSensor = 1.0;
        double LeftSensor = 1.0;
        double RightAngle = 1.0;
        double[] InputLayer = new double[4];
        double[] OutputLayer = new double[1];
        Vector2 dir = new Vector2(0, 0);
        Vector3 dire = new Vector3(0, 0);
        Vector2 localGoal = new Vector2(0, 0);

        while (!ComfortZone.bounds.Contains(new Vector3(Goal.x, this.gameObject.transform.position.y, Goal.z)))
        {
            TimeOfMove += Time.deltaTime;
            localGoal.Set(transform.position.x - Goal.x, transform.position.z - Goal.z);

            //Debug.DrawLine(transform.position, Goal);

            dire = this.gameObject.transform.TransformDirection(Vector3.forward);
            dir.Set(dire.x, dire.z);

            RightAngle = Vector2.SignedAngle(dir, localGoal) / 360 + 0.5;


            if (Physics.Raycast(this.gameObject.transform.position + LocalEyePos, transform.TransformDirection(Vector3.forward), out hit))
            {
                if (hit.distance < FrontalSensorRange && !hit.collider.gameObject.tag.Equals("Mob"))
                {
                    FrontSensor = (hit.distance / FrontalSensorRange);
                }
            }

            if (Physics.Raycast(this.gameObject.transform.position + LocalEyePos, transform.TransformDirection(Vector3.right), out hit))
            {
                if (hit.distance < RightSensorRange && !hit.collider.gameObject.tag.Equals("Mob"))
                {
                    RightSensor = (hit.distance / RightSensorRange);
                }
            }


            if (Physics.Raycast(this.gameObject.transform.position + LocalEyePos, transform.TransformDirection(Vector3.left), out hit))
            {
                if (hit.distance < LeftSensorRange && !hit.collider.gameObject.tag.Equals("Mob"))
                {
                    LeftSensor = (hit.distance / LeftSensorRange);
                }
            }

            if (FrontSensor < 0.02)
            {
                // ResetPosition();
            }

            InputLayer[0] = FrontSensor;
            InputLayer[1] = RightSensor;
            InputLayer[2] = LeftSensor;
            InputLayer[3] = RightAngle;

            //now we should talk to SI, and make him think
            MovingNeuralNet.thisNet.Compute(InputLayer);

            OutputLayer[0] = MovingNeuralNet.thisNet.OutputLayer.ToArray()[0].Value;

            transform.Rotate(0, Time.deltaTime * ((float)(OutputLayer[0] - 0.5) * 360), 0);

            this.gameObject.transform.position += this.gameObject.transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;

            yield return null;

            FrontSensor = 1.0;
            LeftSensor = 1.0;
            RightSensor = 1.0;

            deltaPos = this.gameObject.transform.position - prevPos;

            prevPos = this.gameObject.transform.position;
        }
    }


}
