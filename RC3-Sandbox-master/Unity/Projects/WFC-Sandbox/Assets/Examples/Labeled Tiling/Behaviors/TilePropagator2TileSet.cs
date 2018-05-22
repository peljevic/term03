/*
 * Notes
 * 
 * Tiles must be scaled by -1 in the x if modeled in a right hand coordinate system.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RC3.Graphs;
using RC3.WFC;

namespace RC3.Unity.Examples.LabeledTiling
{
    /// <summary>
    /// 
    /// </summary>
    public class TilePropagator2TileSet : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _tileGraph;
        [SerializeField] private TileSet _tileSet;
        [SerializeField] private TileSet _tileSet_1;
        [SerializeField] private int _boundaryTile = -1;
        [SerializeField] private int _substeps = 10;
        [SerializeField] private int _seed = 1;

        private Digraph _graph;
        private List<VertexObject> _verts;

        private TileModel _model;
        private TileMap _map;

        private CollapseStatus _status;

        List<string> names = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            _graph = _tileGraph.Graph;
            _verts = _tileGraph.VertexObjects;

            _map = _tileSet.CreateMap();
            _model = TileModel.CreateFromGraph(_map, _graph, _seed);

            _model.AssignedCallback = OnAssigned;
            _model.ReducedCallback = OnReduced;

            _status = CollapseStatus.Incomplete;

            AssignBoundary();
        }

        private void StoreNames()
        {
            names.Clear();

            for (int i = 1; i < _tileSet.Count; i++)
            {
                var name = _tileSet[i].name;
                names.Add(name);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tile"></param>
        private void OnAssigned(int position, int tile)
        {
            List<int> nv = new List<int>();

            StoreNames();

            foreach (var n in _graph.GetVertexNeighborsOut(position))
            {
                if (_verts[n].Tile != null)
                {
                    var neigh = _verts[n];

                    if (names.Contains(neigh.Tile.name))
                        nv.Add(n);
                    //Debug.Log(nv.Count);
                }
            }

            if (nv.Count > 2)
            {
                Debug.Log("Entered");
                _verts[position].Tile = _tileSet_1[tile];
            }

            else
            {
                // Debug.Log("First Set");
                _verts[position].Tile = _tileSet[tile];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        private void OnReduced(int position, int size)
        {
            if (size == 0)
                _verts[position].Collapse();
            else
                _verts[position].Reduce((float)size / _tileSet.Count);
        }


        /// <summary>
        /// 
        /// </summary>
        private void AssignBoundary()
        {
            if (_boundaryTile < 0)
                return;

            for (int i = 0; i < _graph.VertexCount; i++)
            {
                foreach (int j in _graph.GetVertexNeighborsOut(i))
                {
                    if (j == i)
                    {
                        _model.Assign(i, _boundaryTile);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _model.Reset();

                foreach (var v in _verts)
                    v.Tile = null;

                _status = CollapseStatus.Incomplete;
                AssignBoundary();
            }

            for (int i = 0; i < _substeps; i++)
            {
                if (_status == CollapseStatus.Incomplete)
                {
                    _status = _model.Observe();

                    if (_status == CollapseStatus.Contradiction)
                    {
                        Debug.Log("Contradiction found! Reset the model and try again.");
                        return;
                    }

                    if (_status == CollapseStatus.Complete)
                    {
                        Debug.Log("Collapse complete!");
                        return;
                    }

                    _model.Propagate();
                }
            }
        }
    }
}
