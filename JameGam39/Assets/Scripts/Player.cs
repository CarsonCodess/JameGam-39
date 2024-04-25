using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    [SerializeField] private TMP_Text currentAbilityText;
    [SerializeField] private List<Transform> teleportPositions;
    [SerializeField] private float teleportCooldown;
    [Header("Shield")]
    [SerializeField] private GameObject forceField;
    [Header("Dagger")]
    [SerializeField] private List<Transform> daggerPositions;
    [SerializeField] private GameObject dagger;
    [SerializeField] private float daggerCooldown;
    
    private Rigidbody2D _rb;
    private SpriteAnimator _anim;
    private string _currentAnimation;
    private bool _canMove = true;
    private bool _canAttack = true;
    private bool _canTeleport = true;
    private bool _shieldIsActive = false;
    private bool _canUseShield = true;
    private float _attackTimer;
    private List<EnemyController> _enemies = new List<EnemyController>();
    private int _currentSpell;
    private bool _canTeleportUnlocked;
    private bool _shieldIsUnlocked;
    private bool _canDaggerUnlocked;
    private bool _canDagger = true;
    
    public void UnlockShield()
    {
        _shieldIsUnlocked = true;
    }

    public void UnlockTeleport()
    {
        _canTeleportUnlocked = true;
    }
    
    public void UnlockDagger()
    {
        _canDaggerUnlocked = true;
    }
    
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
        switch (_currentSpell)
        {
            case 0:
                currentAbilityText.text = "Teleport";
                break;
            case 1:
                currentAbilityText.text = "Shield";
                break;
            case 2:
                currentAbilityText.text = "Dagger";
                break;
        }
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
            if (_currentSpell > 2)
                _currentSpell = 0;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            _currentSpell--;
            if (_currentSpell < 0)
                _currentSpell = 2;
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
                    if(!_canTeleport || _anim.GetCurrentAnimation() == "Death" || !_canTeleportUnlocked)
                        return;
                    PlayAnim("Teleport");
                    Invoke(nameof(Teleport), 0.4f);
                    _canTeleport = false;
                    break;
                case 1:
                    if(!_canUseShield || _anim.GetCurrentAnimation() == "Death" || !_shieldIsUnlocked)
                        return;
                    ActivateShield();
                    break;
                case 2:
                    if(!_canDagger || _anim.GetCurrentAnimation() == "Death" || !_canDaggerUnlocked)
                        return;
                    PlayAnim("Dagger");
                    StartCoroutine(DaggerStart());
                    _canDagger = false;
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

    private IEnumerator DaggerStart()
    {
        _canDagger = false;
        SetCanMove(false);
        foreach (var d in daggerPositions)
        {
            Instantiate(dagger, d);
            yield return new WaitForSeconds(0.5f);
        }
        Invoke(nameof(ResetDagger), daggerCooldown);
        //Invoke(nameof(MoveTrue), 3.5f);
        MoveTrue();
    }

    private void ResetDagger()
    {
        _canDagger = true;
    }

    private void ResetTeleport()
    {
        _canTeleport = true;
    }

    public override void Damage(float amount)
    {
        if(_shieldIsActive){
            return;
        }
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
            Invoke(nameof(ShowDeath), 1f);
        }
    }

    private void ShowDeath()
    {
        DeathScreen.instance.ShowDeathScreen(WaveManager.instance.EnemiesKilledTotal);
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

    public void SetCanMove(bool value = true)
    {
        _canMove = value;
        _rb.velocity = Vector2.zero;
    }

    public void ActivateShield()
    {
        forceField.SetActive(true);
        _shieldIsActive = true;
        _canUseShield = false;
        Invoke(nameof(ResetShield), 3f);
    }
    
    public void ResetShield()
    {
        forceField.SetActive(false);
        _shieldIsActive = false;
        Invoke(nameof(ResetShieldCooldown), 18f);
    }

    public void ResetShieldCooldown(){
        _canUseShield = true;
    }
    
    public void MoveTrue()
    {
        _canMove = true;
        _rb.velocity = Vector2.zero;
    }
}
