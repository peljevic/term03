using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC3.Unity.Examples.LabeledTiling
{
    /// <summary>
    /// 
    /// </summary>
    public class VertexObject : RC3.Unity.VertexObject
    {
        [SerializeField] private Tile _tile;
        [SerializeField, HideInInspector] private List<Rigidbody> _bodies;
        //[SerializeField] private Rigidbody _rigidbody;

        private Rigidbody _rigidbody;
        private GameObject _child;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Vector3 _scale;
        private int _counter=0;


        /// <summary>
        /// 
        /// </summary>
        public Tile Tile
        {
            get { return _tile; }
            set
            {
                _tile = value;
                OnSetTile();
            }
        }

        public Rigidbody Body
        {
            get { return _rigidbody; }
        }

        public List<Rigidbody> Bodies
        {
            get { return _bodies; }

        }


        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _child = transform.GetChild(0).gameObject;
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            _scale = transform.localScale;
            _rigidbody = GetComponent<Rigidbody>();

            OnSetTile();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void OnSetTile()
        {
            transform.localScale = _scale;

            if (_tile == null)
            {
                _filter.sharedMesh = null;
                _renderer.sharedMaterial = null;
                _child.SetActive(true);
                return;
            }

            _filter.sharedMesh = _tile.Mesh;
            _renderer.sharedMaterial = _tile.Material;
            //_rigidbody.isKinematic = true;
           // if(_tile.name == _tileSet[0].name)   
             _bodies.Add(_rigidbody);
            _child.SetActive(false);
            _counter++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void Reduce(float factor)
        {
            transform.localScale = _scale * factor;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Collapse()
        {
            // do something when the tile collapses
        }
    }
}
