﻿using System;
using System.Text;

namespace SoltysDb.Core
{
    class BinaryStringNVarField : BinaryField<string>
    {
        private readonly int maxStringLength;
        private readonly BinaryInt32Field currentStringLength;

        public BinaryStringNVarField(byte[] memory, int offset, int maxStringLength) : base(memory, offset, (maxStringLength * sizeof(char)) + sizeof(int))
        {
            this.maxStringLength = maxStringLength;
            this.currentStringLength = new BinaryInt32Field(memory, offset);
            this.fieldSpan = new Memory<byte>(memory, this.currentStringLength.FieldEnd, maxStringLength * sizeof(char));
            
        }

        public override string GetValue()
        {
            var encodedString = Encoding.Default.GetString(this.fieldSpan.Span.ToArray());
            return encodedString.Substring(0, this.currentStringLength.GetValue());
        }

        public override void SetValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            if (value.Length > this.maxStringLength)
            {
                throw new ArgumentException($"{nameof(value)} is too long to be set in binary field, max length is {this.maxStringLength}", nameof(value));
            }

            this.currentStringLength.SetValue(value.Length);
            var bytes = Encoding.Default.GetBytes(value);
            bytes.CopyTo(this.fieldSpan);
        }
    }
}