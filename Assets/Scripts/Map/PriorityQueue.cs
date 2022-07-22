using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueueNode<T> {
    public PriorityQueueNode() {
        
    }

    public PriorityQueueNode(T v, float o) {
        this.v = v;
        this.o = o;
    }

    public T v;
    public float o;
    public PriorityQueueNode<T> next = null;

}

public class PriorityQueue<T> : IEnumerable<T> {
    public int Count { get; private set; }
    public bool Empty { get { return Count == 0; } }

    private PriorityQueueNode<T> head = null;

    public void Enqueue(T v, float o) {
        var item = new PriorityQueueNode<T>(v, o);

        if(head == null) {
            head = item;
        } else if(head.o >= o) {
            item.next = head;

            head = item;
        } else {
            PriorityQueueNode<T> it = head;
            PriorityQueueNode<T> previous_it = it;
            while(it != null && it.o < o) {
                previous_it = it;
                it = it.next;
            }
            previous_it.next = item;
            item.next = it;
        }

        Count++;
    }

    public T Dequeue() {
        if(head == null)
            return default;

        var val = head.v;
        head = head.next;
        Count--;
        return val;
    }

    public T Peek() {
        if(head == null)
            return default;

        return head.v;
    }

    public IEnumerator<T> GetEnumerator() {
        PriorityQueueNode<T> it = head;

        while(it != null) {
            yield return it.v;
            it = it.next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        PriorityQueueNode<T> it = head;

        while(it != null) {
            yield return it.v;
            it = it.next;
        }
    }

    public T this[int i] {
        get {
            PriorityQueueNode<T> it = head;
            while(i-- > 0) {
                it = it.next;
            }
            return it.v;
        }
    }

}