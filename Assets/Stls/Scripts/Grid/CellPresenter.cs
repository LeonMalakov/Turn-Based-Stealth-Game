using UnityEngine;

namespace Stls
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CellPresenter : MonoBehaviour
    {
        [SerializeField] private Material _default;
        [SerializeField] private Material _visible;
        [SerializeField] private Material _attack;
        [SerializeField] private Cell _cell;

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            ApplyMaterial(_cell.IsVisible, _cell.IsInAttackRange);

            _cell.VisibilityStateChanged += ApplyMaterial;
        }

        private void OnDisable()
        {
            _cell.VisibilityStateChanged -= ApplyMaterial;
        }


        private void ApplyMaterial(bool isVisible, bool isInAttackRange)
        {
            if (isInAttackRange)
                _renderer.sharedMaterial = _attack;
            else if (isVisible)
                _renderer.sharedMaterial = _visible;
            else
                _renderer.sharedMaterial = _default;
        }
    }
}