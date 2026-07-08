using UnityEngine;
using UnityEngine.InputSystem;

public class Instantiation : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;
    InputSystem_Actions inputActions;
    public InputAction Fire;

    void Awake()
    {
     inputActions = new InputSystem_Actions();
     Fire = inputActions.Player.Fire;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        OnFire();
    }

    void OnFire()
    {
        if (Fire.triggered)
        {
            Instantiate(projectilePrefab);
        }
    }
}
