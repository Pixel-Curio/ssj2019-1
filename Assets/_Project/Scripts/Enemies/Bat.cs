using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class Bat : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector2 _movementSpeed;
        [SerializeField] private Bar _healthBar;
        [Inject] private PlayableCharacter _target;

        private void Awake()
        {
            _healthBar.SetWidth(10);
        }

        private void FixedUpdate()
        {
            Vector2 offset = Vector2.zero;

            offset = (_target.transform.position - transform.position).normalized * _movementSpeed;

            _rigidbody.MovePosition(_rigidbody.position + offset * Time.fixedDeltaTime);

            _animator.SetFloat("HorizontalSpeed", offset.x);
        }
    }
}
