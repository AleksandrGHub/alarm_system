using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(SpriteRenderer))]

public class Robber : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _speed = 2.0f;
    private float _jumpForce = 5.0f;
    private bool _punchAnimation = true;
    private const string Fire1 = "Fire1";
    private const string Jump = "Jump";
    private const string Horizontal = "Horizontal";
    private const string left_shift = "left shift";

    public static class AnimatorController
    {
        public static class State
        {
            public const string Run = "Run";

            public const string Walk = "Walk";

            public const string Jump = "Jump";

            public const string PunchAttack = "Punch_attack";
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_punchAnimation)
        {
            if (Input.GetButtonDown(Fire1))
            {
                _animator.SetTrigger(AnimatorController.State.PunchAttack);
            }

            if (Input.GetButtonDown(Jump))
            {
                JumpUp();
            }

            if (Input.GetButton(Horizontal))
            {

                if (Input.GetKey(left_shift))
                {
                    Walk(3);

                    _animator.SetBool(AnimatorController.State.Run, true);
                }

                else
                {
                    _animator.SetBool(AnimatorController.State.Run, false);
                }

                Walk(1);

                _animator.SetBool(AnimatorController.State.Walk, true);
            }

            else
            {
                _animator.SetBool(AnimatorController.State.Walk, false);

                _animator.SetBool(AnimatorController.State.Run, false);
            }
        }
    }

    private void Walk(int dynamics)
    {
        Vector3 direction = transform.right * Input.GetAxis(Horizontal);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction,
            
        _speed * dynamics * Time.deltaTime);

        _sprite.flipX = direction.x < 0.0F;
    }

    private void JumpUp()
    {
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);

        _animator.SetTrigger(AnimatorController.State.Jump);
    }

    private void AnimStop()
    {
        _punchAnimation = false;
    }

    private void AnimStart()
    {
        _punchAnimation = true;
    }
}