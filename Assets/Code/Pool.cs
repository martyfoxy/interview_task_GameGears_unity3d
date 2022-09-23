using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public sealed class Pool<T> where T: MonoBehaviour
    {
	    private int _nextId = 0;
	    private readonly Stack<T> _inactiveStack;
	    private readonly T _prefab;
	    
	    private Transform _poolRoot;
		
		public Pool(T prefab, int warmCount) 
		{
			_inactiveStack = new Stack<T>(warmCount);
			_prefab = prefab;
			
			Warm(warmCount);
		}

		private void Warm(int warmCount)
		{
			_poolRoot = new GameObject($"Pool_{typeof(T)}").transform;
			
			for (var i = 0; i < warmCount; i++)
			{
				var instance = Object.Instantiate(_prefab, _poolRoot.transform, false);
				instance.name = _prefab.name + "_" + _nextId++;
				instance.transform.localPosition = Vector3.zero;
				instance.gameObject.SetActive(false);
				_inactiveStack.Push(instance);
			}
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
			obj.transform.SetParent(_poolRoot.transform, false);
			obj.gameObject.SetActive(false);
			_inactiveStack.Push(obj);
		}
    }
}