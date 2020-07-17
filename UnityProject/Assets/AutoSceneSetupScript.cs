using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoSceneSetupScript : MonoBehaviour
{
    public Transform playerTransform;
    public Transform planetTransform;

    [ContextMenu("Do Setup")]
    void Setup()
    {
        List<Police> polices = new List<Police>(FindObjectsOfType<Police>());
        List<Aligner> aligners = new List<Aligner>(FindObjectsOfType<Aligner>());
        List<RadialDistanceFixer> radialDistanceFixers = new List<RadialDistanceFixer>(FindObjectsOfType<RadialDistanceFixer>());
        List<FaceTarget> faceTargets = new List<FaceTarget>(FindObjectsOfType<FaceTarget>());

        foreach (Police p in polices)
        {
            if (p._playerTransform == null)
            {
                p._playerTransform = playerTransform;
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(p);
            }
        }

        foreach (Aligner a in aligners)
        {
            if (a._center == null)
            {
                a._center = planetTransform;
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(a);
            }
        }

        foreach (RadialDistanceFixer r in radialDistanceFixers)
        {
            if (r._center == null)
            {
                r._center = planetTransform;
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(r);
            }
        }

        foreach (FaceTarget f in faceTargets)
        {
            if (f._target == null)
            {
                f._target = Camera.main.transform;
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(f);
            }
        }

        print("Setup was done succesfully.");
    }
}
