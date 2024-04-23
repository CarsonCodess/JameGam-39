using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Damageable
{
    [SerializeField] private float attackTime = 1f;
    private Transform _target;
    private NavMeshAgent _agent;
    private Animator _anim;
    private bool _canAttack = true;
    private float _attackTimer;
    private string _currentAnimation;

    protected override void Awake()
    {
        base.Awake();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
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
            PlayAnim("LizardAttack");
            player.Damage();
            _attackTimer = attackTime;
            _canAttack = false;
        }
        else if(Vector2.Distance(transform.position, _target.position) > _agent.stoppingDistance)
            PlayAnim("LizardWalk");
    }
    
    private void PlayAnim(string anim)
    {
        if (_currentAnimation == "LizardAttack")
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("LizardAttack"))
                return;
            else
                _currentAnimation = "";
        }

        if (_currentAnimation != anim)
        {
            _anim.Play(anim);
            _currentAnimation = anim;
        }
    }
}
