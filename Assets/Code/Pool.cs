using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public sealed class Pool<T> where T: MonoBehaviour
    {
	    private int _nextId = 0;
	    private readonly Stack<T> _inactiveStack;
	    private readonly T _prefab;
		
		public Pool(T prefab, int initialQty) 
		{
			_inactiveStack = new Stack<T>(initialQty);
			_prefab = prefab;
		}

		public T Spawn(Transform pivot)
		{
			T instance;
			if (_inactiveStack.Count == 0)
			{
				instance = Object.Instantiate(_prefab, pivot);
				instance.name = _prefab.name + "_" + _nextId++;
			}
			else
			{
				instance = _inactiveStack.Pop();
				if (instance == null)
					return Spawn(pivot);
			}
			
			instance.transform.SetParent(pivot, false);
			instance.transform.localPosition = Vector3.zero;
			instance.gameObject.SetActive(true);
			
			return instance;
		}

		public void Despawn(T obj) 
		{
			obj.gameObject.SetActive(false);
			_inactiveStack.Push(obj);
		}
    }
}