using System;

// Custom linked stack made by us and not unity!
public class CustomStack<T>
{
    Node<T> top = null;
    
    public bool IsEmpty()
    {
        return top == null;
    }

    public void Push(T item)
    {
        Node<T> newNode =  new Node<T>();
        newNode.value = item;
        newNode.next = top;
        top = newNode;
    }

    public T Pop()
    {
        if(IsEmpty()) throw new InvalidOperationException("Stack Is Empty");

        T value = top.value;
        top = top.next;
        return value;
    }

    public T Peek()
    {
        if(IsEmpty()) throw new InvalidOperationException("Stack Is Empty");
        return top.value;
    }
    public void Clear()
    {
        top = null;
    }

    private class Node<X>
    {
        public Node<X> next;
        public X value;
    }

   
}