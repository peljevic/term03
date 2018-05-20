using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RC3.Graphs;

namespace RC3.Unity.Examples.LabeledTiling
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _grid;       
        [SerializeField] private TileSet _tileSet;
        [SerializeField] private GUISkin mySkin;

        private List<VertexObject> _vertices;
        private Digraph _graph;
        private int _unassigned;
        private int[] _list;

        private void Start()
        {
            _list = new int[_tileSet.Count];
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
            _unassigned = _graph.VertexCount;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                CountTiles();

            if (Input.GetKeyDown(KeyCode.R))
                ResetTiles();

        }

        void ResetTiles()
        {
            for (int i = 0; i < _list.Length; i++)
                _list[i] = 0;

            _unassigned = _graph.VertexCount;
        }

        void CountTiles()
        {
            foreach (var v in _vertices)
            {
                for (int i = 0; i < _tileSet.Count; i++)
                { if (v.Tile == _tileSet[i]) { _list[i]++; _unassigned--; break; } }
            }

        }

        private void OnGUI()
        {           
            GUI.skin = mySkin;
            GUI.Label(new Rect(new Vector2(35, 350), new Vector2(350, 100)), "graph capacity : " + _graph.VertexCount.ToString());

            GUI.Label(new Rect(new Vector2(35, 370), new Vector2(350, 100)), "StoreTilesWithMeshes(); AddJoints(); CheckLowest() = J");
            GUI.Label(new Rect(new Vector2(35, 390), new Vector2(350, 100)), " AddGravity(); = G");
            GUI.Label(new Rect(new Vector2(35, 410), new Vector2(350, 100)), " DeleteEmpty(); = D");
            GUI.Label(new Rect(new Vector2(35, 430), new Vector2(350, 100)), " Count tiles by type = C");

            GUI.Label(new Rect(new Vector2(Screen.width - 165, 120), new Vector2(250, 100)), "unassigned : " + _unassigned.ToString());

            for (int i = 0; i < _tileSet.Count; i++)
            {               
                GUI.Label(new Rect(new Vector2(Screen.width - 165, 140 + 20*i), new Vector2(250, 100)), "tile type " + i + " : " + _list[i].ToString());
            }

        }
    }
}
