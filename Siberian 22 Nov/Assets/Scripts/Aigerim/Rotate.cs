using UnityEngine;


public class Rotate : MonoBehaviour
{
    [SerializeField] private TypeRotate _typeRotate;
    [SerializeField] private float _speedNormal;
    [SerializeField] private float _speedFast;
    private float _curSpeed;
    private Vector3 _direction;
    public enum SpeedBlender
    {
        Stop,
        Normal,
        Fast
    }
    private enum TypeRotate
    {
        X,
        Y,
        Z
    }
    
    
    private void Awake()
    {
        _curSpeed = _speedNormal;

        switch (_typeRotate)
        {
            case TypeRotate.X:
            {
                _direction = Vector3.right;
            }
                break;
            case TypeRotate.Y:
            {
                _direction = Vector3.up;
            }
                break;
            case TypeRotate.Z:
            {
                _direction = Vector3.forward;
            }
                break;
        }
    }
    private void Update()
    {
        RotateControl();
    }


    public void SetSpeed(SpeedBlender newSpeed)
    {
        switch (newSpeed)
        {
            case SpeedBlender.Stop:
            {
                _curSpeed = 0;
            }
                break;
            case SpeedBlender.Normal:
            {
                _curSpeed = _speedNormal;
            }
                break;
            case SpeedBlender.Fast:
            {
                _curSpeed = _speedFast;
            }
                break;
        }
    }
    private void RotateControl()
    {
        transform.Rotate(_direction * (_curSpeed * Time.deltaTime));
    }
}
