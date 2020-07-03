using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShrinker : MonoBehaviour
{
    [SerializeField] Transform planetTransform = null;
    [SerializeField] float shrinkDuration = 30;
    [SerializeField] float planetFinalRadius = 0;

    List<RadialDistanceFixer> rdfs;
    float planetStartRadius;
    float planetCurrentRadius;
    float timeSinceStart;

    bool shrinkIsDone;

    // Start is called before the first frame update
    void Start()
    {
        planetStartRadius = planetTransform.localScale.z / 2f; //assuming even scale.x = scale.y = scale.z
        planetCurrentRadius = planetStartRadius;
        timeSinceStart = 0f;
        shrinkIsDone = false;

        rdfs = new List<RadialDistanceFixer>( FindObjectsOfType<RadialDistanceFixer>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!shrinkIsDone)
        {
            Shrink();
            timeSinceStart += Time.deltaTime;
        }
    }

    void Shrink()
    {
        float shrinkFraction = timeSinceStart / shrinkDuration;
        //print("shrink fraction: " + shrinkFraction.ToString() + " Time Since Start:" + timeSinceStart);

        float planetNewRadius = Mathf.Lerp(planetStartRadius, planetFinalRadius, shrinkFraction);
        float deltaRadius = planetNewRadius - planetCurrentRadius;

        planetCurrentRadius += deltaRadius;

        planetTransform.localScale = Vector3.one * planetCurrentRadius * 2f;

        foreach(RadialDistanceFixer r in rdfs)
        {
            r._distance += deltaRadius; 
        }

        if (planetCurrentRadius <= planetFinalRadius)
        {
            shrinkIsDone = true;
            //print("Shrink Is Done");
        }
    }
}
