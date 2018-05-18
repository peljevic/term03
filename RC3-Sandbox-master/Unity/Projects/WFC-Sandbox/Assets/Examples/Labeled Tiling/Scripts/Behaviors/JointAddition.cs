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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J)) AddJoints();

            if (Input.GetKeyDown(KeyCode.G)) AddGravity();
        }

        private void GetRigidbodies()
        {
            for(int i =0; i<_vertices.Count; i++)
            {
                int counter = 0;

                var v = _vertices[i];
                if (v.Body.isKinematic == true)
                {
                    _bodies[counter] = v.Body;
                    counter++;
                }
            }
        }

        private void AddGravity()
        {
            

            foreach (var v in _vertices)
            {
                var b =v.GetComponent<Rigidbody>();
                b.useGravity = true;
            }
        }

        private void AddJoints()
        {
            for (int i=0; i<_vertices.Count; i++)
            { 
            //foreach (var v in _vertices)
            
                var neigbours = _graph.GetVertexNeighborsOut(i);
                var v = _vertices[i];

                foreach (var n in neigbours)
                {
                    if (v.Body.isKinematic == true && _vertices[i] != _vertices[n])
                    {
                        var joint = v.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = _vertices[n].GetComponent<Rigidbody>();

                        joint.breakForce = BreakForce;
                        joint.breakTorque = BreakTorque;
                    }
                }
            }

        }


    }
}
