using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 3f;

    private void Update()
    {
        if (_playerTransform == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            _playerTransform.position,
            _moveSpeed * Time.deltaTime);
    }
}
