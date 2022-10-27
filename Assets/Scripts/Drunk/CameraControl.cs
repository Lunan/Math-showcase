using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 20.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float mouseScrollSpeed = 2;
    private float totalRun = 1.0f;

    //private TerrainSpawner spawner;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (/*!Utils.paused*/true)
            {                
                transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);


                //Keyboard commands
                Vector3 p = GetBaseInput();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * shiftAdd;
                    p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                    p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                    p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
                }
                else
                {
                    totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                    p = p * mainSpeed;
                }

                p = p * Time.deltaTime;
                transform.Translate(p);
                if (Input.mouseScrollDelta.y != 0)
                {
                    transform.position += transform.forward * Input.mouseScrollDelta.y * mouseScrollSpeed;
                }                
            }
        }           
        
    }

    private float GetLowestAbs(float a, float b)
    {
        float absA = Mathf.Abs(a);
        float absB = Mathf.Abs(b);
        if (absA == absB)
        {
            return Mathf.Min(a, b);
        }
        if (absA > absB)
        {
            return b;
        }
        else
        {
            return a;
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 then it's not active.
        Vector3 p_Velocity = new Vector3();
#if !UNITY_EDITOR
        if (Input.mousePosition.y <= Screen.height * 0.02f && Application.isFocused)
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.mousePosition.y >= Screen.height * 0.98f && Application.isFocused)
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
#endif
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)/* || Input.mousePosition.x <= Screen.width * 0.02f*/)
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)/* || Input.mousePosition.x >= Screen.width * 0.98f*/)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}