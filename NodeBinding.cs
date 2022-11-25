using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNodeBinding
{
    [Serializable]
    public class NodeBinding : MonoBehaviour
    {
        [SerializeField]
        public List<Node> nodeList;
        private Dictionary<string, Node> bindings = new Dictionary<string, Node>();

        private void OnEnable()
        {
            bindings.Clear();
            foreach (var node in nodeList)
            {
                if (bindings.ContainsKey(node.name)) throw new Exception("has same key in the node bindings.");
                bindings.Add(node.name, node);
            }
        }

        [Serializable]
        public class Node
        {
            [SerializeField]
            public string name;

            [SerializeField]
            public NodeType type = NodeType.Component;

            [SerializeField]
            public Component component;

            [SerializeField]
            public GameObject gameObject;
        }

        public enum NodeType
        {
            GameObject,
            Component,
        }
    }
}
