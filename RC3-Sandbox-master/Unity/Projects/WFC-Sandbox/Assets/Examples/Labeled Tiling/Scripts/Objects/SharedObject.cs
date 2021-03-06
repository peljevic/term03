﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */

namespace RC3.Unity
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "Objects/SharedSelection")]
    public class SharedObject : ScriptableObject
    {
        private HashSet<int> _indices;


        /// <summary>
        /// 
        /// </summary>
        public HashSet<int> Indices
        {
            get { return _indices; }
        }


        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            _indices = new HashSet<int>();
        }
    }
}
