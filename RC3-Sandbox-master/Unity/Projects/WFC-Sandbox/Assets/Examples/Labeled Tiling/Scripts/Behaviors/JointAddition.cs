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
        private List<Material> _materials;
        private List<FixedJoint> _joints;

        public Color[] Spectrum;

        private void Start()
        {
           // _list = new int[_tileSet.Count];
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;

           // _materials = new Material[_vertices.Count]; 
           //CacheMaterials();
           
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
                
                if (v.Tile.name != _tileSet[0].name)
                {
                    var b = v.GetComponent<Rigidbody>();
                    b.useGravity = true;
                }
                else if (v.Tile.name == _tileSet[0].name)
                {
                    v.Body.isKinematic = false;
                    Debug.Log("0 tile!"); 
                }
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
                    if (v.Body.isKinematic == true && v != _vertices[n] && v.Tile.name != _tileSet[0].name && _vertices[n].Tile.name != _tileSet[0].name)
                    {
                        var joint = v.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = _vertices[n].GetComponent<Rigidbody>();

                        joint.breakForce = BreakForce;
                        joint.breakTorque = BreakTorque;

                    //     _joints.Add(joint);

                    }
                }
            }

        }

        /*
        private void CacheMaterials()
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                var v = _vertices[i];
                if (v == null) continue;

                var m = v.Tile.Material;
                _materials.Add(m); 
                m.color = Spectrum[0];
            }
        }


        /// <summary>
        /// Updates the body colors.
        /// TODO implement better
        /// </summary>
        /// <returns>The body colors.</returns>
        IEnumerator UpdateBodyColors()
        {
            const float factor = 0.75f;

            while (true)
            {
                for (int i = 0; i < _materials.Length; i++)
                {
                    var m = _materials[i];

                    if (m != null)
                        m.color = Color.Lerp(m.color, GetTorqueColor(i), factor);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private Color GetTorqueColor(int index)
        {
            var joints = _joints[index];

            float sum = 0.0f;
            int count = 0;

            foreach (var j in _joints[index])
            {
                if (j != null)
                {
                    sum += j.currentTorque.sqrMagnitude;
                    count++;
                }
            }

            if (count == 0)
                return Spectrum[0];

            return Lerp(Spectrum, sum / (count * MaxTorque));
        }

        public static Color Lerp(IReadOnlyList<Color> colors, float factor)
        {
            int last = colors.Count - 1;
            int i;
            factor = SlurMathf.Fract(factor * last, out i);

            if (i < 0)
                return colors[0];
            else if (i >= last)
                return colors[last];

            return Color.LerpUnclamped(colors[i], colors[i + 1], factor);
        }

        private Color GetForceColor(int index)
        {
            var joints = _joints[index];

            float sum = 0.0f;
            int count = 0;

            foreach (var j in _joints[index])
            {
                if (j != null)
                {
                    sum += j.currentForce.sqrMagnitude;
                    count++;
                }
            }

            if (count == 0)
                return Spectrum[0];

            return Lerp(Spectrum, sum / (count * MaxTorque));
        }
        */
         

    }
}
