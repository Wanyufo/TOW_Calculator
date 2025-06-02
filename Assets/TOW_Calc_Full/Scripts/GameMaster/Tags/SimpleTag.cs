using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameMaster
{
    public class SimpleTag : MonoBehaviour, ITag
    {
        [SerializeField] public TagManager.SimpleTag smartTag;
    
        private void OnEnable()
        {
            GM.I.TagManager.RegisterTag(smartTag, this);
        }
    
        private void OnDestroy()
        {
            GM.I.TagManager.DeRegisterTag(smartTag, this);
        }
    
        public Enum GetTag()
        {
            return smartTag;
        }
    }
}