using System;
using UnityEngine;

public class CustomLinkedList<T>
{
    Node<T> head;
    Node<T> tail;
    

    public int Size {get; protected set;} 
    public bool IsEmpty { get{ return Size == 0; } }


    public T this[int index]
    {
        get { return FindAt(index); }
        set { Set(index, value); }
    }

    public CustomLinkedList()
    {
        Empty();
    }

    public int Find(T value)
    {
        Node<T> node;
        return FindNode(value, out node);
    }

    public T FindAt(int index)
    {
       return FindNodeAt(index).value;
    }

    public void Insert(T value)
    {
        InsertNodeBefore(tail, value);
    }

    public void InsertAt(int index, T value)
    {
        InsertNodeBefore(FindNodeAt(index), value);
    }

    public T Set(int index, T value)
    {
        FindNodeAt(index).value = value;
        return value;
    }

    public bool Remove(T value)
    {
        Node<T> node;
        int index = FindNode(value, out node);

        if(index == -1)
            return false;
        

        node.previous.next = node.next;
        node.next.previous = node.previous;

        Size--;
        return true;
    }

    public T RemoveAt(int index)
    {
        Node<T> node = FindNodeAt(index);

        node.previous.next = node.next;
        node.next.previous = node.previous;

        Size--;
        return node.value;
    }

    public void Empty()
    {
        tail = new Node<T>(default(T), null, null);
        head = new Node<T>(default(T), null, tail);
        tail.previous = head;
        Size = 0;
    }

    private Node<T> FindNodeAt(int index)
    {
        if(index < 0 || index >= Size)  
            throw new IndexOutOfRangeException("Invalid Index " + index + " for List of size " + Size);


        Node<T> node;

        if(index < Size/2)
        {
            node = head;
            
            for(int i = 0; i <= index; i++)
            {
                node = node.next;
            }
        }
        else
        {
            node = tail;

            for(int i = Size; i > index; i--)
            {
                node = node.previous;
            }
        }

        return node;
    }

    private int FindNode(T value, out Node<T> node)
    {
        node = head;

        for(int i = 0; i < Size; i++)
        {
            node = node.next;
            if (Equals(node.value, value))
            {
                return i;
            }
        }

        node = null;
        return -1;
    }

    private void InsertNodeBefore(Node<T> node, T value)
    {
        Node<T> newNode = new Node<T>(value, node.previous, node);
        node.previous.next = newNode;
        node.previous = newNode;

        Size++;
    }

    class Node<X>
    {
  	    // To store the value or data
        public X value; 
  
        // Pointer to the next node
        public Node<X> next; 
    
  	    // Pointer to the previous node
        public Node<X> previous; 

        // Constructor
        public Node(X value, Node<X> previous, Node<X> next)
        {
            this.value = value;
            this.previous = previous;
            this.next = next;
        }
    }
}
