using System;
using System.Collections;
using UnityEngine;

namespace Stls
{
    [RequireComponent(typeof(Pawn))]
    [DisallowMultipleComponent]
    public class PawnView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        private Pawn _pawn;
        protected Transform _transform;
        protected Coroutine _translationCoroutine;

        protected virtual void Awake()
        {
            _transform = transform;
            _pawn = GetComponent<Pawn>();
        }

        protected virtual void OnEnable()
        {
            _pawn.Died += OnDied;
            _pawn.Attacking += OnAttacking;
            _pawn.TakedownMoving += OnTakedownMoving;
            _pawn.Moving += OnMoving;
            _pawn.InstantMoving += OnInstantMoving;
        }

        protected virtual void OnDisable()
        {
            _pawn.Died -= OnDied;
            _pawn.Attacking -= OnAttacking;
            _pawn.TakedownMoving -= OnTakedownMoving;
            _pawn.Moving -= OnMoving;
            _pawn.InstantMoving -= OnInstantMoving;
        }

        private void OnDied(GridDirection attackedFrom)
        {
            ResetToStartTurnState();

            _animator.SetInteger(Constants.ManAnimatorController.Parameters.TakedownIndex, (int)attackedFrom);
            _animator.SetTrigger(Constants.ManAnimatorController.Parameters.Die);
        }

        private void OnAttacking(Cell cell)
        {
            ResetToStartTurnState();
            LookAt(cell);
            _animator.SetTrigger(Constants.ManAnimatorController.Parameters.Attack);
        }

        private void LookAt(Cell cell)
        {
            Vector3 toTargetVector = cell.Position.ToWorldPosition() - _transform.position;
            toTargetVector.y = 0;
            _transform.rotation = Quaternion.FromToRotation(Vector3.forward, toTargetVector);
        }

        private void OnTakedownMoving(Cell location, GridDirection direction, GridDirection takedownDirection)
        {
            ResetToStartTurnState();

            _translationCoroutine = StartCoroutine(TranslationCoroutine(_pawn.Location, location, direction, SetupAnimator));

            void SetupAnimator()
            {
                _animator.SetInteger(Constants.ManAnimatorController.Parameters.TakedownIndex, (int)takedownDirection);
                _animator.SetTrigger(Constants.ManAnimatorController.Parameters.Takedown);
            }
        }

        private void OnMoving(Cell location, GridDirection direction)
        {
            ResetToStartTurnState();

            _translationCoroutine = StartCoroutine(TranslationCoroutine(_pawn.Location, location, direction, SetupAnimator));

            void SetupAnimator() => _animator.SetTrigger(Constants.ManAnimatorController.Parameters.Move);
        }

        private void OnInstantMoving(Cell location, GridDirection direction)
        {
            BreakTranslationCoroutine();
            ResetAnimation();

            ApplyLocation(location);
            ApplyRotation(direction);
        }

        protected void ResetToStartTurnState()
        {
            BreakTranslationCoroutine();
            ResetAnimation();

            ApplyLocation(_pawn.Location);
            ApplyRotation(_pawn.Direction);
        }

        protected void BreakTranslationCoroutine()
        {
            if (_translationCoroutine != null)
            {
                StopCoroutine(_translationCoroutine);
                _translationCoroutine = null;
            }
        }

        protected void ResetAnimation()
        {
            _animator.Play(Constants.ManAnimatorController.States.Idle);
        }

        protected void ApplyRotation(GridDirection direction)
        {
            _transform.eulerAngles = new Vector3(
                _transform.eulerAngles.x,
                GridMetrics.StartAngle + (int)direction * GridMetrics.Angle,
                _transform.eulerAngles.z);
        }

        protected void ApplyLocation(Cell location)
        {
            _transform.position = location.Position.ToWorldPosition();
        }

        private IEnumerator TranslationCoroutine(Cell from, Cell to, GridDirection direction, Action animatorSetup)
        {
            var moveDirection = GridCoordinates.GetDirectionFromCoordinates(to.Position - from.Position);
            ApplyRotation(moveDirection);

            animatorSetup?.Invoke();

            // < Translation
            Vector3 startPosition = from.Position.ToWorldPosition();
            Vector3 targetPosition = to.Position.ToWorldPosition();
            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime / Constants.Game.TurnTime;
                _transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }
            // Translation >

            // Apply target rotation and location.
            ApplyRotation(direction);
            ApplyLocation(to);

            _translationCoroutine = null;
        }
    }
}