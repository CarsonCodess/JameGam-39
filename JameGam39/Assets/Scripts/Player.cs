using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Damageable
{
    public static Player instance;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    [Header("Health")]
    [SerializeField] private List<Image> hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [FormerlySerializedAs("attackTime")]
    [FormerlySerializedAs("swordAttackTime")]
    [Header("Weapons")] 
    [SerializeField] private float baseAttackTime;
    [FormerlySerializedAs("swordDamage")] [SerializeField] private float baseDamage;
    [Header("Teleport")]
    [SerializeField] private List<Transform> teleportPositions;
    [SerializeField] private float teleportCooldown;
    
    private Rigidbody2D _rb;
    private SpriteAnimator _anim;
    private string _currentAnimation;
    private bool _canMove = true;
    private bool _canAttack = true;
    private bool _canTeleport = true;
    private float _attackTimer;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private int _currentSpell;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<SpriteAnimator>();
        Application.targetFrameRate = 60;
        instance = this;
    }

    private void Update()
    {
        if (!_canMove || _anim.GetCurrentAnimation() == "Teleport")
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
        
        if (!_canAttack)
            _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
            _canAttack = true;

        if (Input.mouseScrollDelta.y > 0)
        {
            _currentSpell++;
            if (_currentSpell > 4)
                _currentSpell = 0;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            _currentSpell--;
            if (_currentSpell < 0)
                _currentSpell = 4;
        }

        if(Input.GetMouseButtonDown(0) && _canAttack)
        {
            _attackTimer = baseAttackTime;
            _canAttack = false;
            
            PlayAnim("Attack");

            foreach (var enemy in _enemies)
                enemy.Damage(baseDamage);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (_currentSpell)
            {
                case 0:
                    if(!_canTeleport || _anim.GetCurrentAnimation() == "Death")
                        return;
                    PlayAnim("Teleport");
                    Invoke(nameof(Teleport), 0.4f);
                    _canTeleport = false;
                    break;
            }
        }
    }

    private void Teleport()
    {
        var rand = Random.Range(0, teleportPositions.Count);
        transform.position = teleportPositions[rand].position;
        Invoke(nameof(ResetTeleport), teleportCooldown);
    }

    private void ResetTeleport()
    {
        _canTeleport = true;
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        UpdateUI();
    }

    public override void Heal()
    {
        base.Heal();
        UpdateUI();
    }

    private void UpdateUI()
    {
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
        if (_anim.GetCurrentAnimation() is "Death" or "Attack" or "Teleport")
        {
            if(anim != "Death")
                return;
        }

        if (_currentAnimation != anim)
        {
            _anim.SwitchAnimation(anim);
            _currentAnimation = anim;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
            _enemies.Add(other.GetComponent<EnemyController>());
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && _enemies.Contains(other.GetComponent<EnemyController>()))
            _enemies.Remove(other.GetComponent<EnemyController>());
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;
        _rb.velocity = Vector2.zero;
    }
}
