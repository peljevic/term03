/*
 * Notes
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RC3.Graphs;

namespace RC3.Unity.Examples.LabeledTiling
{
    /// <summary>
    /// 
    /// </summary>
    public class RhombicGraph : MonoBehaviour
    {
        [SerializeField] SharedDigraph _tileGraph;
        [SerializeField] VertexObject _vertexPrefab;
        [SerializeField] private int _countX = 10;
        [SerializeField] private int _countY = 10;
        [SerializeField] private int _countZ = 10;


        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            var graph = Digraph.Factory.CreateRhombicDodecahedronGrid(_countX, _countY, _countZ, true);

            _tileGraph.Initialize(graph);
            _tileGraph.VertexObjects.AddRange(CreateVertexObjects());

            transform.position = new Vector3(-_countX, -_countY, -_countZ); // center
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<VertexObject> CreateVertexObjects()
        {
            int count = 0;

            foreach (var p in GetVertexPositions())
            {
                var vPos = transform.position;

                vPos.x = p.x * 1.4f;
                vPos.y = p.y * 1.4f;
                vPos.z = p.z * 2f;


                var vobj = Instantiate(_vertexPrefab, vPos, transform.rotation);
                //var vPos = vobj.transform.localPosition; //= p * 2.0f;
               
                //vPos.x = p.x * 2f;  // 1.4f;
                //vPos.y = p.y * 2.8f;
                //vPos.z = p.z * 2f;

                vobj.Vertex = count++;

                yield return vobj;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector3> GetVertexPositions()
        {
            for (int z = 0; z < _countZ; z++)
            {
                for (int y = 0; y < _countY; y++)
                {
                    for (int x = 0; x < _countX; x++)
                    {
                        //if (x % 2 == 0)
                        //    yield return new Vector3(x, y, z);

                       // else
                           yield return new Vector3(x + 0.5f, y + 0.7f, z-0.5f);
                    }
                }
            }

            for (int z = 0; z < _countZ; z++)
            {
                for (int y = 0; y < _countY; y++)
                {
                    for (int x = 0; x < _countX; x++)
                    {
                       // if (x % 2 == 0)
                       //     yield return new Vector3(x + 0.5f, y + 0.7f, z + 0.5f);
                       // else
                            yield return new Vector3(x, y+1.2f, z);
                    }
                }
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            var graph = _tileGraph.Graph;
            var verts = _tileGraph.VertexObjects;

            for(int i= 0; i < graph.VertexCount; i++)
            {
                var p0 = verts[i].transform.position;

                foreach(var j in graph.GetVertexNeighborsOut(i))
                {
                    if (j <= i) continue;
                    
                    var p1 = verts[j].transform.position;
                    Debug.DrawLine(p0,p1);
                }
            }
        }
        */
        
    }
}
