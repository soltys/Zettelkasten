using SoltysLib.Bson.BQuery;
using SoltysLib.TextAnalysis;
using Xunit;

namespace SoltysLib.Test.Bson
{
    public class BQueryParserTests
    {
        [Fact]
        public void ParseAccessQuery_OnePart_ExpectedAst()
        {
            var expectedAst = new AstValueAccess("foo");
            var actualAst = GetAccessQueryAst("foo");

            Assert.Equal(expectedAst.ElementName, actualAst.ElementName);
            Assert.Null(actualAst.SubAccess);
        }

        [Fact]
        public void ParseAccessQuery_WithSubAccess_ExpectedAst()
        {
            var expectedAst = new AstValueAccess("foo")
            {
                SubAccess = new AstValueAccess("bar")
            };

            var actualAst = GetAccessQueryAst("foo.bar");

            Assert.Equal(expectedAst.ElementName, actualAst.ElementName);
            Assert.Equal(expectedAst.SubAccess.ElementName, actualAst.SubAccess.ElementName);
        }

        [Fact]
        public void ParseAccessQuery_WithArrayAccess_ExpectedAst()
        {
            var expectedAst = new AstArrayAccess("foo", 42);
            var actualAst = GetAccessQueryAst("foo[42]");

            Assert.IsType<AstArrayAccess>(actualAst);
            var actualAccessAst = (AstArrayAccess)actualAst;
            Assert.Equal(expectedAst.ElementName, actualAccessAst.ElementName);
            Assert.Equal(expectedAst.ArrayIndex, actualAccessAst.ArrayIndex);
            Assert.Null(actualAccessAst.SubAccess);
        }

        [Fact]
        public void ParseAccessQuery_WithArrayAccessAndSubAccess_ExpectedAst()
        {
            var expectedAst = new AstArrayAccess("foo", 42)
            {
                SubAccess = new AstValueAccess("bar")
            };

            var actualAst = GetAccessQueryAst("foo[42].bar");

            Assert.IsType<AstArrayAccess>(actualAst);
            var actualAccessAst = (AstArrayAccess)actualAst;
            Assert.Equal(expectedAst.ElementName, actualAccessAst.ElementName);
            Assert.Equal(expectedAst.ArrayIndex, actualAccessAst.ArrayIndex);
            Assert.Equal(expectedAst.SubAccess.ElementName, actualAccessAst.SubAccess.ElementName);
        }

        private static AstValueAccess GetAccessQueryAst(string input)
        {
            var parser = new BQueryParser(new TokenSource<BQueryToken>(new BQueryLexer(input)));
            return parser.ParseValueQuery();
        }
    }
}
