using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postprocesing : MonoBehaviour
{
    private static Postprocesing instance;
    public static Postprocesing Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Material lsdMaterial;
    [SerializeField]
    private Material defaultMaterial;

    float time = 0;

    public float redAngle;
    public float greenAngle;
    public float blueAngle;

    public float redFrequency;
    public float greenFrequency;
    public float blueFrequency;

    public bool LSDMode = false;

    public Vector4 pointAB = new Vector2();
    public Vector4 pointC0 = new Vector2();
    public int iterations = 0;

    private void Awake()
    {
        instance = this;
    }
  
    private void Start()
    {

        Reset();
    }

    public void Reset()
    {
        LSDMode = false;
        pointAB.Set(0.7f, 0.3f, 0.3f, 0.3f);
        pointC0.Set(0.5f, 0.7f, 0.5f, 0.5f);

    }

    private void OnApplicationQuit()
    {
        Reset();
    }

    private void Update()
    {
        time += Time.deltaTime;

        //pointAB.x += Time.deltaTime / 10000;
        //pointC0.y += Time.deltaTime / 10000;
        //
        //pointAB.x -= Mathf.Sin((60 * 3.14f)/360) * Time.deltaTime / 100;
        //pointAB.y -= Mathf.Sin((30 * 3.14f) / 360) * Time.deltaTime / 100;
        //
        //pointAB.z += Mathf.Sin((60 * 3.14f) / 360) * Time.deltaTime / 100;
        //pointAB.w -= Mathf.Sin((30 * 3.14f) / 360) * Time.deltaTime / 100;

        lsdMaterial.SetVector("_PointAB", pointAB);
        lsdMaterial.SetVector("_PointC0", pointC0);
        lsdMaterial.SetFloat("_timer", time);
        lsdMaterial.SetFloat("_redAngle", redAngle);
        lsdMaterial.SetFloat("_redFreq", redFrequency);
        lsdMaterial.SetFloat("_greenAngle", greenAngle);
        lsdMaterial.SetFloat("_greenFreq", greenFrequency);
        lsdMaterial.SetFloat("_blueAngle", blueAngle);
        lsdMaterial.SetFloat("_blueFreq", blueFrequency);
        lsdMaterial.SetInt("_CountOfIterations", iterations);

    }

    public void EffectsUp(float plusAmplitude, float plusTurbulence)
    {
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, defaultMaterial);
        if (LSDMode)
            Graphics.Blit(source, destination, lsdMaterial);
    }
}
