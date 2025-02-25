using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private float _baseForce = 150f;
    [SerializeField] private float _damage = 25.0f;
    [SerializeField] private float _forceMultiplier = 1.0f;
    [SerializeField] private Transform _camera;
    [SerializeField] private Material _bootMaterial;
    [SerializeField] private Material _bootMaterialBase;

    private void Start()
    {
        _bootMaterialBase = this.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        //_bootMaterial.SetColor("_EmissionColor", Color.red * 15);
        //_bootMaterial.EnableKeyword("_EMISSION");



        //while (_camera == null)
        //{
        //    _camera = SuperManager.instance.ghostManager.GetMainTarget();
        //}

        if (_camera != null)
        {
            //MeasureHeightBoot();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            GameObject vfxHeavy = SuperManager.instance.vfxManager.InstantiateVFX_vfxFootImpact(this.transform);
            Ghost behaviorGhost = collision.gameObject.GetComponent<Ghost>();
            behaviorGhost.LowerHealth(_damage);
            behaviorGhost.AddForceToGhostOppositeToTarget(_baseForce*_forceMultiplier, ForceMode.Impulse);
            SuperManager.instance.soundManager.PlaySoundAtLocation(SoundType.Sword, 0.2f, this.transform.position);
        }
    }

    private void MeasureHeightBoot()
    {
        float height = Mathf.Abs(this.transform.position.y - _camera.position.y);

        if (height < 1.0)
        {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material = _bootMaterial;

            _forceMultiplier = 1 + height * 2;
        }
        else
        {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material = _bootMaterialBase;
            _forceMultiplier = 1;
        }
        
    }
}
