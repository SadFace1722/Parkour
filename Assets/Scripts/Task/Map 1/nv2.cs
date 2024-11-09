using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nv2 : MonoBehaviour, PlayerInterface
{
    public static nv2 Instance;
    public bool generator;
    public int EnergyTank = 0;

    [SerializeField] private List<Material> materials;
    [SerializeField] private List<GameObject> gameObjects;  // Đảm bảo là các Point Light
    [SerializeField] private float fogDensityStart = 0.05f;
    [SerializeField] private float fogDensityEnd = 0.01f;
    [SerializeField] private float fogFadeDuration = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DeactivateAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        if (EnergyTank >= 1 && !generator)
        {
            generator = true;
            ActivateMaterials();
            ActivateGameObjects();
            StartCoroutine(FadeFog());
        }
        else
        {
            Debug.Log("nv2: Điều kiện chưa đủ để khởi động máy phát điện.");
        }
    }

    private void ActivateMaterials()
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.white);
            }
        }
    }

    private void ActivateGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            Light light = obj.GetComponent<Light>();
            if (light != null && light.type == LightType.Point)
            {
                light.enabled = true;
            }
        }
    }

    private void DeactivateAll()
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.DisableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.black);
            }
        }

        foreach (GameObject obj in gameObjects)
        {
            Light light = obj.GetComponent<Light>();
            if (light != null && light.type == LightType.Point)
            {
                light.enabled = false;
            }
        }
    }

    private IEnumerator FadeFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensityStart;

        float elapsedTime = 0f;
        while (elapsedTime < fogFadeDuration)
        {
            RenderSettings.fogDensity = Mathf.Lerp(fogDensityStart, fogDensityEnd, elapsedTime / fogFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogDensity = fogDensityEnd;
    }
}
