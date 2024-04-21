using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Damageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    [Header("Health")]
    [SerializeField] private List<Image> hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    
    private Rigidbody2D _rb;
    private Animator _anim;
    private string _currentAnimation;
    private bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!_canMove)
            return;
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
            PlayAnim("Idle");
            Decelerate(accelerationTime * 1000);
        }
        else
            PlayAnim("Walk");
        
        if(Input.GetKeyDown(KeyCode.T))
            Damage();
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        var tempHealth = health;
        foreach (var heart in hearts)
        {
            if (tempHealth >= 1)
            {
                heart.sprite = fullHeart;
                tempHealth -= 1f;
            }
            else if (tempHealth >= 0.5)
            {
                heart.sprite = halfHeart;
                tempHealth -= 0.5f;
            }
            else
                heart.sprite = emptyHeart;
        }

        if (health <= 0)
        {
            _rb.velocity = Vector2.zero;
            _canMove = false;
            PlayAnim("Death");
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
        var deceleration = (Vector2.zero - _rb.velocity) / time;
        _rb.velocity += deceleration;
    }

    private void PlayAnim(string anim)
    {
        if(_currentAnimation == "Death")
            return;
        if (_currentAnimation != anim)
        {
            _anim.Play(anim);
            _currentAnimation = anim;
        }
    }
}
