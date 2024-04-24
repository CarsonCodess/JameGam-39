using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Damageable
{
    [SerializeField] private float attackTime = 1f;
    [SerializeField] private float damage = 0.5f;
    private Transform _target;
    private NavMeshAgent _agent;
    private SpriteAnimator _anim;
    private bool _canAttack = true;
    private float _attackTimer;
    private string _currentAnimation;

    protected override void Awake()
    {
        base.Awake();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<SpriteAnimator>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_anim.GetCurrentAnimation() is "Death" or "Hurt")
            _agent.isStopped = true;
        _agent.SetDestination(_target.position);
        
        if (_agent.velocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (_agent.velocity.x > 0)
            transform.localScale = new Vector3(1, 1, 1); 

        if (!_canAttack)
            _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
            _canAttack = true;
        if (Vector2.Distance(transform.position, _target.position) <= _agent.stoppingDistance * 1.5f && _canAttack)
        {
            var player = _target.GetComponent<Player>();
            if(player.IsDead())
                return;
            PlayAnim("Attack");
            player.Damage(damage);
            _attackTimer = attackTime;
            _canAttack = false;
        }
        else if(Vector2.Distance(transform.position, _target.position) > _agent.stoppingDistance)
            PlayAnim("Walk");
        if (_anim.GetCurrentAnimation() == "Destroy")
        {
            _currentAnimation = "";
            WaveManager.instance.EnemyKilled();
            Destroy(gameObject);
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        if (health <= 0)
        {
            _anim.SwitchAnimation("Death");
            _currentAnimation = "Death";
        }
        else
        {
            _anim.SwitchAnimation("Hurt");
            _currentAnimation = "Hurt";
        }
    }

    private void PlayAnim(string anim)
    {
        if(_anim.GetCurrentAnimation() is "Death" or "Hurt" or "Destroy")
            return;
        if (_currentAnimation == "Attack")
        {
            if (_anim.GetCurrentAnimation() == "Attack")
                return;
            _currentAnimation = "";
        }

        if (_currentAnimation != anim)
        {
            _anim.SwitchAnimation(anim);
            _currentAnimation = anim;
        }
    }
}
