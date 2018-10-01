using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static List<Wall> Walls = new List<Wall>();

    public int HP;

    private void Awake()
    {
        Walls.Add(this);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Walls.Remove(this);
    }
}
