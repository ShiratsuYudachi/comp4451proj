using System;

public class CircularBuffer<T>
{
    private T[] buffer;
    private int currentIndex;
    private int count;
    
    public CircularBuffer(int size)
    {
        buffer = new T[size];
        currentIndex = 0;
        count = 0;
    }
    
    public void Add(T item)
    {
        buffer[currentIndex] = item;
        currentIndex = (currentIndex + 1) % buffer.Length;
        count = Math.Min(count + 1, buffer.Length);
    }
    
    public bool Contains(T item)
    {
        for (int i = 0; i < count; i++)
        {
            if (buffer[i]?.Equals(item) ?? item == null)
                return true;
        }
        return false;
    }
    
    public void Clear()
    {
        Array.Clear(buffer, 0, buffer.Length);
        currentIndex = 0;
        count = 0;
    }
}