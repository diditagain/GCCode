using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputScriptableObject input;
    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private PlayerSprite _playerSprite;

    private Coroutine _movementCoroutine;
    private WaitForSeconds fixedDeltaTime;

    private Coroutine _autoMoveCoroutine;
    private void Awake()
    {
        fixedDeltaTime = new WaitForSeconds(Time.fixedDeltaTime);
        _animator = GetComponent<Animator>();

        _playerSprite = GetComponent<PlayerSprite>();
    }
    
    private void OnEnable()
    {
        input.MovementPerformed += OnMovementPerformed;
        input.MovementCanceled += OnMovementCanceled;
    }
    private void OnDisable()
    {
        input.EnableControls(false);
        input.MovementPerformed -= OnMovementPerformed;
        input.MovementCanceled -= OnMovementCanceled;
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnMovementPerformed()
    {
        _animator.SetBool("Moving", true);

        if(_movementCoroutine == null)        
            _movementCoroutine = StartCoroutine(MovementCoroutine());
        
    }
    private void OnMovementCanceled()
    {
        _animator.SetBool("Moving", false);
        _rigidBody.velocity = Vector2.zero;
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }            
    }

    IEnumerator MovementCoroutine()
    {
        while (true)
        {
            _rigidBody.velocity = movementSpeed * Time.fixedDeltaTime * input.MovementVector;
            _playerSprite.UpdateSprite(input.MovementVector.x);

            yield return fixedDeltaTime;
        }
    }


    public void GoToNewArea(Area areaTrigger)
    {
        if(_autoMoveCoroutine == null)
            _autoMoveCoroutine = StartCoroutine(NewAreaMovementCoroutine(areaTrigger));
    }

    IEnumerator NewAreaMovementCoroutine(Area areaTrigger)
    {
        input.EnableControls(false);
        OnMovementCanceled();

        _animator.SetBool("Moving", true);

        float dot = Vector2.Dot((areaTrigger.EndPos - areaTrigger.StartPos).normalized, Vector2.right);
        _playerSprite.UpdateSprite(dot);

        float lerpTime = 5f;
        float timer = 0;

        while (timer < lerpTime)
        {
            transform.position = Vector3.Lerp(areaTrigger.StartPos, areaTrigger.EndPos, timer/lerpTime);
            timer += Time.deltaTime;
            yield return null;
        }

        _animator.SetBool("Moving", false);

        input.EnableControls(true);

        _autoMoveCoroutine = null;
    }
}
