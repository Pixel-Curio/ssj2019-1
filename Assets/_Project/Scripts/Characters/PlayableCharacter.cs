using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCurio.OccultClassic
{
    public class PlayableCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector2 _movementSpeed;

        private void FixedUpdate()
        {
            Vector2 offset = Vector2.zero;

            if (Input.GetKey(KeyCode.A)) offset.x += -_movementSpeed.x;
            if (Input.GetKey(KeyCode.D)) offset.x += _movementSpeed.x;
            if (Input.GetKey(KeyCode.W)) offset.y += _movementSpeed.y;
            if (Input.GetKey(KeyCode.S)) offset.y += -_movementSpeed.y;

            _rigidbody.MovePosition(_rigidbody.position + offset * Time.fixedDeltaTime);

            _animator.SetFloat("HorizontalSpeed", offset.x);
            _animator.SetFloat("VerticalSpeed", offset.y);
        }
    }
}
