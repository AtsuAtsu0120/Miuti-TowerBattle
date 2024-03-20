using System.Collections.Generic;
using Core.Components;
using UnityEngine;

namespace Foundation
{
    [CreateAssetMenu]
    public class FallingObjectAsset : ScriptableObject
    {
        public List<FallingObject> FallingObjectList => _fallingObjectList;
        [SerializeField] private List<FallingObject> _fallingObjectList;
    }
}
