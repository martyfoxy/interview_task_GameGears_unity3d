using System;

namespace Code.Players
{
    public sealed class Parameter
    {
        public int ID { get; }
        public float Value
        {
            get => _value;
            set
            {
                if (Math.Abs(_value - value) < float.Epsilon)
                    return;

                OldValue = _value;
                _value = value;
                
                if (_value <= float.Epsilon)
                    _value = 0f;
                
                OnChanged?.Invoke(this);
            }
        }
        private float _value;
        
        public float OldValue { get; private set; }
        public float InitValue { get; }

        public event Action<Parameter> OnChanged;

        public Parameter(int id, float initValue)
        {
            ID = id;
            InitValue = initValue;
            Value = initValue;
            OldValue = 0f;
        }

        public void Reset()
        {
            Value = InitValue;
        }
    }
}