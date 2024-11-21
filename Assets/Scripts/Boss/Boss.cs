using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Animator ani;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public bool atacando;
    public RangoBoss rango;
    public float speed;
    public GameObject[] hit;
    public int hit_Select;






    //
    public bool lanza_llamas;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject fine;
    public GameObject cabeza;
    private float cronometro2;

    //fine_ball
    public GameObject fine_ball;
    public GameObject point;
    public List<GameObject> pool2 = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
