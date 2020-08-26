using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;

/// <summary>
/// Stores items in lists together sorted by the list's index.
/// - Only use this if the order of values stored together under a single key doesn't matter -
/// </summary>
/// <typeparam name="Key"></typeparam>
/// <typeparam name="Value"></typeparam>
public class SortedBag<Key, Value> : IEnumerable<KeyValuePair<Key, List<Value>>>
{
    /// <summary>
    /// The number of bags
    /// </summary>
    public IList<Key> Keys { get => bags.Keys; }

    SortedList<Key, List<Value>> bags = new SortedList<Key, List<Value>>();

    public bool ContainsKey(Key key)
    {
        return bags.ContainsKey(key);
    }
    public bool ContainsKey(Key key, Value value)
    {
        if (bags.ContainsKey(key))
            return bags[key].Contains(value);
        else return false;
    }

    /// <summary>
    /// Adds an item to an indexed bag, or creates a bag and puts the item in it
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(Key key, Value value)
    {
        if (!bags.ContainsKey(key))
            bags.Add(key, new List<Value>());

        bags[key].Add(value);
    }

    /// <summary>
    /// Removes an entire bag of items
    /// </summary>
    /// <param name="key">The index of the bag to remove</param>
    public void Remove(Key key)
    {
        if (bags.ContainsKey(key))
            bags.Remove(key);
        else
        {
            string keysOutputLog = "\nThis collection contains:\n";
            foreach (var item in bags.Keys)
            {
                keysOutputLog += item.ToString() + "\n";
            }
            throw new KeyNotFoundException("The key " + key + " does not exist: " + keysOutputLog);
        }
    }

    /// <summary>
    /// Removes a single item from a bag
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Remove(Key key, Value value)
    {

        if (bags.ContainsKey(key))
        {
            if (bags[key].Contains(value))
            {
                bags[key].Remove(value);
            }
            else
            {
                string valuesOutputLog = "\nThis bag contains:\n";
                foreach (var item in bags[key])
                {
                    valuesOutputLog += item.ToString() + "\n";
                }
                throw new KeyNotFoundException("The item " + value + " does not exist: " + valuesOutputLog);
            }
        }
        else
        {
            string keysOutputLog = "\nThis collection contains:\n";
            foreach (var item in bags.Keys)
            {
                keysOutputLog += item.ToString() + "\n";
            }
            throw new KeyNotFoundException("The key " + key + " does not exist: " + keysOutputLog);
        }
    }

    public IEnumerator<KeyValuePair<Key, List<Value>>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<Key, List<Value>>>)bags).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)bags).GetEnumerator();
    }

    public List<Value> this[Key key]
    {
        get { return bags[key]; }
    }

}