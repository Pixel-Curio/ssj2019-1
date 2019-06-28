using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class PlayableCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector2 _movementSpeed;
        [Inject] private readonly IGun _gun;
        private Vector2 _lastOffset = Vector2.down;

        private void FixedUpdate()
        {
            Vector2 offset = Vector2.zero;

            if (Input.GetKey(KeyCode.A)) offset += Vector2.left;
            if (Input.GetKey(KeyCode.D)) offset += Vector2.right;
            if (Input.GetKey(KeyCode.W)) offset += Vector2.up;
            if (Input.GetKey(KeyCode.S)) offset += Vector2.down;
            if (offset != Vector2.zero) _lastOffset = offset;

            if (Input.GetKey(KeyCode.Space)) FireWeapon(_lastOffset);

            offset = offset.normalized * _movementSpeed;

            _rigidbody.MovePosition(_rigidbody.position + offset * Time.fixedDeltaTime);

            _animator.SetFloat("HorizontalSpeed", offset.x);
            _animator.SetFloat("VerticalSpeed", offset.y);
        }

        private void FireWeapon(Vector2 direction)
        {
            _gun.Fire(transform.position, direction);
        }
    }
}
