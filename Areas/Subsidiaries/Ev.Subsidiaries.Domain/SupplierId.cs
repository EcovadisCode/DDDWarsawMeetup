using System;

namespace Ev.Subsidiaries.Domain
{
    public class SupplierId 
    {
        public SupplierId(int value)
        {
            if (value <= 0) throw new ArgumentException($"Incorrect value for {nameof(SupplierId)}");

            Value = value;
        }

        public static implicit operator int(SupplierId id)
        {
            return id.Value;
        }

        public int Value { get; }
    }
}