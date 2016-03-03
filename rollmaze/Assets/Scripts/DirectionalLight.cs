using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum IntensityDirection
{
    None = 0,
    Dim = -1,
    Brighten = 1
}

public class DirectionalLight : MonoBehaviour
{
    public static DirectionalLight Instance;

    private IntensityDirection intensityDirection = IntensityDirection.None;

    private Light directionalLight;

    void Awake()
    {
        Instance = this;
        directionalLight = GetComponent<Light>();
    }

    void FixedUpdate()
    {
        directionalLight.intensity += ( (int)intensityDirection * Time.deltaTime );

        if ((directionalLight.intensity <= 0.7f) && (intensityDirection == IntensityDirection.Dim))
            intensityDirection = IntensityDirection.None;

        if ((directionalLight.intensity >= 1) && (intensityDirection == IntensityDirection.Brighten))
            intensityDirection = IntensityDirection.None;
    }

    public void Dim()
    {
        intensityDirection = IntensityDirection.Dim;
    }

    public void Brighten()
    {
        intensityDirection = IntensityDirection.Brighten;
    }
}
