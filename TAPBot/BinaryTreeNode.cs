using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class BinaryTreeNode<TKey, TValue>
    {
        private TKey key;
        public TKey Key
        {
            get { return key; }
            //set { key = value; }
        }

        protected TValue data;
        public TValue Data
        {
            get { return data; }
            //set { data = value; }
        }

        protected BinaryTreeNode<TKey, TValue> left;
        public BinaryTreeNode<TKey, TValue> Left
        {
            get { return left; }
            set { left = value; }
        }

        protected BinaryTreeNode<TKey, TValue> right;
        public BinaryTreeNode<TKey, TValue> Right
        {
            get { return right; }
            set { right = value; }
        }

        public BinaryTreeNode(TKey key)
        {
            this.key = key;
        }

        public BinaryTreeNode(TKey key, TValue data)
        {
            this.key = key;
            this.data = data;
        }
    }
}
