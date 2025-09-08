using UnityEngine;
using System;
using System.Collections.Generic;

public class HashMap<K, V>
{
    private const int InitialSize = 16;
    private LinkedList<KeyValuePair<K, V>>[] buckets;

    public HashMap()
    {
        buckets = new LinkedList<KeyValuePair<K, V>>[InitialSize];
    }

    private int GetBucketIndex(K key)
    {
        int hashCode = key.GetHashCode();
        return Math.Abs(hashCode % buckets.Length);
    }

    public void Put(K key, V value)
    {
        int index = GetBucketIndex(key);
        if (buckets[index] == null)
        {
            buckets[index] = new LinkedList<KeyValuePair<K, V>>();
        }

        foreach (var pair in buckets[index])
        {
            if (pair.Key.Equals(key))
            {
                // Update the value if the key already exists
                buckets[index].Remove(pair);
                break;
            }
        }

        buckets[index].AddLast(new KeyValuePair<K, V>(key, value));
    }

    public V Get(K key)
    {
        int index = GetBucketIndex(key);
        if (buckets[index] != null)
        {
            foreach (var pair in buckets[index])
            {
                if (pair.Key.Equals(key))
                {
                    return pair.Value;
                }
            }
        }
        throw new KeyNotFoundException("Key not found: " + key);
    }

    public bool ContainsKey(K key)
    {
        int index = GetBucketIndex(key);
        if (buckets[index] != null)
        {
            foreach (var pair in buckets[index])
            {
                if (pair.Key.Equals(key))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
