using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class BinaryTree<TKey, TValue> where TKey : IComparable
    {
        private BinaryTreeNode<TKey, TValue> root;

        public BinaryTree() { }
        public BinaryTree(BinaryTreeNode<TKey, TValue> root)
        {
            this.root = root;
        }

        public void Insert(BinaryTreeNode<TKey, TValue> node)
        {
            if (root == null)
                root = node;
            else
                Insert(node, root);
        }

        private void Insert(BinaryTreeNode<TKey, TValue> node, BinaryTreeNode<TKey, TValue> subRoot)
        {
            if (node.Key.CompareTo(subRoot.Key) < 0)
            {
                if (subRoot.Left == null)
                    subRoot.Left = node;
                else
                    Insert(node, subRoot.Left);
            }
            else if (node.Key.CompareTo(subRoot.Key) > 0)
            {
                if (subRoot.Right == null)
                    subRoot.Right = node;
                else
                    Insert(node, subRoot.Right);
            }

            return;
        }

        public TValue Search(TKey key)
        {
            return Search(key, root);
        }

        private TValue Search(TKey key, BinaryTreeNode<TKey, TValue> subRoot)
        {
            if (subRoot == null)
                return default(TValue);
            
            if (subRoot.Key.CompareTo(key) == 0)
                return subRoot.Data;
            else if (subRoot.Key.CompareTo(key) > 0)
                return Search(key, subRoot.Left);
            else
                return Search(key, subRoot.Right);
        }
    }
}
