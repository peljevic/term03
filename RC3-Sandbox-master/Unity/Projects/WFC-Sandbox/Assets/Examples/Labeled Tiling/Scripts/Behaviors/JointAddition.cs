using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RC3.Graphs;

namespace RC3.Unity.Examples.LabeledTiling
{
    public class JointAddition : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _grid;
        [SerializeField] private TileSet _tileSet;
        private List<VertexObject> _vertices;
        private Digraph _graph;

        [Range(0.0f, 10000.0f)]
        [SerializeField] private float MaxForce = 1000.0f;

        [Range(0.0f, 10000.0f)]
        [SerializeField] private float MaxTorque = 1000.0f;

        private float BreakForce = Mathf.Infinity;
        private float BreakTorque = Mathf.Infinity;

        private Rigidbody[] _bodies;
        private Material[] _materials;
        private FixedJoint[][] _joints;

        private void Start()
        {
           // _list = new int[_tileSet.Count];
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
           // _unassigned = _graph.VertexCount;

        }

    }
}
