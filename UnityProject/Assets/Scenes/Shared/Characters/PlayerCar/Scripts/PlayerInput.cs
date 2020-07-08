using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public class PlayerInput : MonoBehaviour
{
    private VehicleController vc;

    // Start is called before the first frame update
    void Awake()
    {
        vc = GetComponent<VehicleController>();
    }

    // Update is called once per frame
    void Update()
    {
        vc.SetInput(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
}
