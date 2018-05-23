using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RC3.Graphs;
using System;

namespace RC3.Unity.Examples.LabeledTiling
{
    public class JointAddition : MonoBehaviour
    {
        #region variables

        [SerializeField] private SharedDigraph _grid;
        [SerializeField] private TileSet _tileSet;

        List<VertexObject> _vertices;
        private Digraph _graph;

        [Range(0.0f, 10000.0f)]
        [SerializeField] private float MaxForce = 1000.0f;

        [Range(0.0f, 10000.0f)]
        [SerializeField] private float MaxTorque = 1000.0f;

        private float BreakForce = Mathf.Infinity;
        private float BreakTorque = Mathf.Infinity;

        private List<VertexObject> _emptyTiles;
        private List<VertexObject> _meshedTiles;
        private List<VertexObject> _boundaries;
        private List<VertexObject> _vbodies;
        private List<Rigidbody> _bodies;
        private List<Material> _materials;
        private List<FixedJoint> _joints;

        public Color[] Spectrum;


        #endregion variables

        private void Start()
        {
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
            _emptyTiles = new List<VertexObject>();
            _meshedTiles = new List<VertexObject>();

            // _list = new int[_tileSet.Count];
            //_boundaries = new List<VertexObject>();   
            // _materials = new Material[_vertices.Count]; 
            //CacheMaterials();


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J)) { StoreTilesWithMeshes(); AddJoints(); CheckLowest(); }

            if (Input.GetKeyDown(KeyCode.G)) AddGravity();

            //if (Input.GetKeyDown(KeyCode.B)) StoreTilesWithMeshes();//StoreBoundaries();

            if (Input.GetKeyDown(KeyCode.D)) DeleteEmpty();
        }


        /// <summary>
        /// Stores the boundary tiles in separate List<>
        /// </summary>
        void StoreBoundaries()
        {
            int counter = 0;

            for (int i = 0; i < _graph.VertexCount; i++)
            {
                foreach (int j in _graph.GetVertexNeighborsOut(i))
                {
                    if (j == i)
                    {
                        _boundaries[counter] = _vertices[i];
                    }
                }
                counter++;
            }
            Debug.Log(counter);
        }


        /// <summary>
        /// Add Gravity to the Tiles containing Meshes
        /// </summary>
        void AddGravity()
        {
            if (_meshedTiles != null)
            {
                foreach (var r in _meshedTiles)
                {
                    var body = r.Body;
                    body.useGravity = true;
                }
            }
        }

        private float SmallestDistance()
        {
            float dist0 = 100;

            foreach (var v in _meshedTiles)
            {
                    var dist1 = v.transform.position.y;

                    if (dist1 < dist0)
                    {
                        dist0 = dist1;
                    }
            }
    
            Debug.Log("Smallest distance is" + dist0);
            return dist0;
        }

        private void CheckLowest()
        {
            var lowest = SmallestDistance();

            foreach (var v in _meshedTiles)
            {               
                if (v.transform.position.y == lowest)
                {
                    v.Body.isKinematic = true;
                }
            }
        }

        private void AddKinematic()
        {
            for (int i = 0; i < _graph.VertexCount; i++)
            {
                if (_vertices[i].transform.position.z == 2 && _vertices[i].Tile.name != _tileSet[0].name)
                {
                    _vertices[i].Body.isKinematic = true;
                }
            }
        }

        private void StoreTilesWithMeshes()
        {
            StoreEmptyTiles();

            for (int i = 0; i < _graph.VertexCount; i++)
            {
                var v = _vertices[i];

                if (_emptyTiles.Contains(v) == false)
                {
                    _meshedTiles.Add(v);
                }
                else continue;
            }
            Debug.Log("Meshed are" + _meshedTiles.Count);
            Debug.Log("Empty are" + _emptyTiles.Count);
        }
     
        /// <summary>
        /// Called from StoreTilesWithMeshes !!!
        /// </summary>
        private void StoreEmptyTiles()
        {
            for (int i = 0; i < _graph.VertexCount; i++)
            {
                var v = _vertices[i];
                
                if (v.Tile.name == _tileSet[0].name)
                {
                    _emptyTiles.Add(v);                   
                }
            }
        }

        private void DeleteEmpty()
        {
            if (_emptyTiles != null)
            {
                foreach (var v in _emptyTiles)
                {
                    Destroy(v.gameObject);
                   // Destroy(v.Body);
                   // Destroy(v);
                }
            }
        }

        private void AddJoints()
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                //foreach (var v in _vertices)

                var neigbours = _graph.GetVertexNeighborsOut(i);
                var v = _vertices[i];

                foreach (var n in neigbours)
                {
                    var vn = _vertices[n];

                    if (v != _vertices[n] && v.Tile.name != _tileSet[0].name && vn.Tile.name != _tileSet[0].name)
                    //v.Tile.Mesh==null && nv.Tile.Mesh==null)
                    // v.Tile.name != _tileSet[0].name && _vertices[n].Tile.name != _tileSet[0].name)
                    {
                        var joint = v.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = vn.GetComponent<Rigidbody>();

                        joint.breakForce = BreakForce;
                        joint.breakTorque = BreakTorque;

                      //  if (_vbodies.Contains(v) == false) _vbodies.Add(v);

                      //  Debug.Log(_vbodies.Count);

                    }
                }
                
            }
        }

    }
}

        /*
         * 
             void GravityAddition(List<Rigidbody> rigidbodies)
        {
            foreach (var r in rigidbodies)
            {
                r.useGravity = true;
            }
        }

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
                for (int i = 0; i < _materials.Count; i++)
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
            //var joints = _joints[index];

            float sum = 0.0f;
            int count = 0;

            foreach (var j in _joints)//[index])
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
           // factor = Fract(factor * last, out i);

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

