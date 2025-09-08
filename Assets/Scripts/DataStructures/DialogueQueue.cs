using System;

public class DialogueQueue<T>
{
    private class Node
    {
        //Stores a line of dialogue
        public T Data;
        
        //Pointer to the next node in the queue
        public Node Next;
        
        //Node constructor
        public Node(T data)
        {
            //Sets the data value initially
            Data = data;
            Next = null;
        }
    }

    //Points to the first line in the queue
    private Node front;
    
    //Points to the last line in the queue
    private Node rear;
    
    //Keeps track of how many lines are in the queue
    private int count;

    //Queue Constructor
    public DialogueQueue()
    {
        //Intialises the queue variables
        front = rear = null;
        count = 0;
    }
    
    public void Enqueue(T item)
    {
        //Creates a node for the newest line to store in 
        Node newNode = new Node(item);
        
        //Checks if the queue is empty first
        if (IsEmpty())
        {
            front = rear = newNode;
        }
        
        //Forms a chain, linking all lines in order of FIFO
        else
        {
            //Links the previous rear value to the latest line enrty
            rear.Next = newNode;
            
            //Assigns the newest line entry to rear.
            rear = newNode;
        }
        //Increases the number of lines in the queue
        count++;
    }
    
    public T Dequeue()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Queue is empty");
        }

        //Stores a line in the first node (front) we are about to return and remove
        T item = front.Data;
        
        //Moves the front to the next node in the queue
        front = front.Next;
        
        //Resets the rear if the queue is empty
        if (front == null)
        {
            rear = null;
        }
        
        count--;
        return item;
    }

    // Peek at front item without removing it
    public T Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty");
        }

        return front.Data;
    }

    // Check if queue is empty
    public bool IsEmpty()
    {
        return count == 0;
    }

    // Get the number of items in the queue
    public int Count()
    {
        return count;
    }
}