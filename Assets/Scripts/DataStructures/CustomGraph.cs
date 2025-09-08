using System.Collections.Generic;
using UnityEngine;

public class CustomGraph<T>
{
    
    private Dictionary<T, List<T>> vertices = new Dictionary<T, List<T>>();

    public bool AddVertex(T data)
    {
        return vertices.TryAdd(data, new List<T>());
    }

    public bool AddEdge(T from, T to)
    {
        if(!vertices.ContainsKey(from) || !vertices.ContainsKey(to))
        {
            return false;
        }
        if(vertices[from].Contains(to))
        {
            return false;
        }

        vertices[from].Add(to);
        return true;
    }

    public List<T> GetConnectedVertex(T vertex)
    {
        if(!vertices.ContainsKey(vertex))
        {
            return new List<T>();
        }

        return vertices[vertex];
    }

    public void RemoveVertex(T vertex)
    {
        if (!vertices.ContainsKey(vertex))
        {
            return;
        }
        
        vertices.Remove(vertex);
        
        foreach (KeyValuePair<T, List<T>> pair in vertices)
        {
            pair.Value.Remove(vertex);
        }
    }

    public bool ContainsVertex(T vertex)
    {
        return vertices.ContainsKey(vertex);
    }
}

