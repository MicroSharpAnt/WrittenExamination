using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JoyCastle笔试题目：
/// ③从上到下依次打印出一颗二叉树的最左侧节点
///
/// 思路：
/// 将二叉树每层最左侧节点存入Dictionary中，key为深度，value为最左侧节点值，再输出所有value值即可
/// </summary>
public class LeftNodes : MonoBehaviour
{
    private void Start()
    {
        // 先序遍历创建树
        int[] array = { 2, 11, 10, -1, 12, 13, -1, -1, -1, 15, -1, -1, 23, 7, -1, -1, 14, -1, -1 };
        // int[] array =  { 3, 9, -1, -1, 20, 15, -1, -1, 7, -1, 18, -1, -1 }; // 测试用例
        List<int> list = new List<int>();
        LinkBinaryTree tree = new LinkBinaryTree();

        list.AddRange(array);
        tree.CreateBinaryTreeByPreOrder(list.Count, list);

        Debug.Log("================先序遍历================");

        tree.PreOrder(tree.Root());

        Debug.Log("================输出最左侧的节点================");

        Dictionary<int, int> dic = new Dictionary<int, int>();
        tree.AddLeftNodesToDic(tree.Head, 1, dic);
        foreach (var item in dic.Values)
        {
            Debug.Log(item);
        }
    }
}

public class LinkBinaryTree
{
    public TreeNode<int> Head { get; set; }

    public LinkBinaryTree()
    {
        Head = null;
    }

    public bool IsEmpty()
    {
        return Head == null;
    }

    public TreeNode<int> Root()
    {
        return Head;
    }

    /// <summary>
    /// 先序遍历的方式创建树
    /// </summary>
    /// <param name="count">节点数量（包括空节点）</param>
    /// <param name="data">需要存入的数据</param>
    /// <returns></returns>
    public TreeNode<int> CreateBinaryTreeByPreOrder(int count, List<int> data)
    {
        if (data.Count == 0)
        {
            return null;
        }

        int d = data[0];
        TreeNode<int> node;
        int index = count - data.Count;

        if (d == -1)
        {
            node = null;
            data.RemoveAt(0);
            return node;
        }

        node = new TreeNode<int>(d);
        if (index == 0)
        {
            Head = node;
        }

        data.RemoveAt(0);
        node.LChild = CreateBinaryTreeByPreOrder(count, data);
        node.RChild = CreateBinaryTreeByPreOrder(count, data);

        return node;
    }

    /// <summary>
    /// 先序遍历输出节点
    /// </summary>
    /// <param name="ptr">根节点</param>
    public void PreOrder(TreeNode<int> ptr)
    {
        if (IsEmpty())
        {
            Debug.Log("Tree is Empty");
            return;
        }

        if (ptr != null)
        {
            Debug.Log(ptr.Data + " ");
            PreOrder(ptr.LChild);
            PreOrder(ptr.RChild);
        }
    }

    /// <summary>
    /// 将树最左侧节点存入dic中，key为深度，value为最左侧节点值
    /// </summary>
    /// <param name="ptr">根节点</param>
    /// <param name="dept">根节点深度值</param>
    /// <param name="dic">储存最左侧节点的字典</param>
    public void AddLeftNodesToDic(TreeNode<int> ptr, int dept, Dictionary<int, int> dic)
    {
        if (ptr == null)
        {
            return;
        }

        if (!dic.ContainsKey(dept))
        {
            dic.Add(dept, ptr.Data);
        }

        if (ptr.LChild != null)
        {
            AddLeftNodesToDic(ptr.LChild, dept + 1, dic);
        }

        if (ptr.RChild != null)
        {
            AddLeftNodesToDic(ptr.RChild, dept + 1, dic);
        }
    }
}

public class TreeNode<T>
{
    public T Data { get; set; }

    public TreeNode<T> LChild { get; set; }

    public TreeNode<T> RChild { get; set; }

    public TreeNode(T value)
    {
        Data = value;
        LChild = null;
        RChild = null;
    }
}