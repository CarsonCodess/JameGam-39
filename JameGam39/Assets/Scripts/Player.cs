using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    
    private Rigidbody2D _rb;
    private Animator _anim;
    private string _currentAnimation;
    private bool _canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        var inputH = Input.GetAxisRaw("Horizontal");
        var inputV = Input.GetAxisRaw("Vertical");
        var direction = new Vector2(inputH, inputV).normalized;
        Accelerate(accelerationTime, direction, 10 * moveSpeed * Time.deltaTime);

        if (inputH < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (inputH > 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        if (Mathf.Abs(inputH) <= 0 && Mathf.Abs(inputV) <= 0)
        {
            if(_currentAnimation != "Idle")
                _anim.Play("Idle");
            Decelerate(accelerationTime * 1000);
        }
        else
        {
            if(_currentAnimation != "Walk")
                _anim.Play("Walk");
        }
    }

    public void Accelerate(float time, Vector2 direction, float maxSpeed)
    {
        if (!_canMove)
            return;

        var acceleration = (direction * maxSpeed - _rb.velocity) / time;
        _rb.velocity += acceleration;
    }

    public void Decelerate(float time)
    {
        if (!_canMove)
            return;

        var deceleration = (Vector2.zero - _rb.velocity) / time;
        _rb.velocity += deceleration;
    }
}
