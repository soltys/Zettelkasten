using System.Linq;
using SoltysVm.Test.TestUtils;
using Xunit;

namespace SoltysVm.Test
{
    public partial class InstructionDecoderTests
    {
        [Fact]
        public void DecodeStream_StreamWithMultipleOpcode_DecodesInstructions()
        {
            var bytesStream = InstructionByteBuilder.Create()
                .Opcode(Opcode.Load, LoadType.Integer, 42)
                .Opcode(Opcode.Load, LoadType.Integer, 69)
                .Opcode(Opcode.Add)
                .Opcode(Opcode.Return)
                .AsStream();

            var instructions =  InstructionDecoder.DecodeStream(bytesStream).ToList();

            Assert.Equal(4, instructions.Count);
            Assert.Equal("add", instructions.ElementAt(2).ToString());
            Assert.Equal("ret", instructions.ElementAt(3).ToString());
        }
    }
}
