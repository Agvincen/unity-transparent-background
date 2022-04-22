using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[System.Serializable]
public struct POINT
{
    public int X;
    public int Y;
}
public class MouseRotate : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    public GameObject Model;
    public Vector2 RotationSensitivity = Vector2.right;
    public Vector2 RotationDamping = Vector2.right;

    private Collider _collider;
    private Camera _camera;
    private bool _rotationEnabled;
    private bool _refresh;
    private Vector2 _releaseVelocity;
    private Vector2 _mouse;
    public POINT _mousePos;

    void Awake()
    {
        _camera = Camera.main;
        _collider = Model.GetComponent<Collider>();
        if (_collider == null)
        {
            Debug.LogError("Model is missing a collider. Rotations will not work!");
            Destroy(this);
        }
    }

    void Update()
    {
        if (_refresh)
        {
            SetCursorPos(_mousePos.X, _mousePos.Y);
            Cursor.visible = true;
            _refresh = false;
        }

        _mouse = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) //Left Click
        {
            Ray ray = _camera.ScreenPointToRay(_mouse);
            if (Physics.Raycast(ray, out _))
                _rotationEnabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _rotationEnabled = false;

            Cursor.lockState = CursorLockMode.None;
            _refresh = true;
        }
        else if (_rotationEnabled)
        {
            Vector2 spin = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * RotationSensitivity;
            if(Cursor.visible && spin.magnitude > 0.01)
            {
                GetCursorPos(out _mousePos);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            _releaseVelocity = spin;
            RotateModel(spin);
        }
        else
        {
            if(_releaseVelocity.magnitude > 0.1f)
            {
                _releaseVelocity *= RotationDamping;
                RotateModel(_releaseVelocity);
            }
        }
    }
    void RotateModel(Vector2 rot)
    {
        Model.transform.Rotate(new Vector2(rot.y, -rot.x) * Time.deltaTime);
    }
}
