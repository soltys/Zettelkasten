﻿using System;
using Xunit;

namespace SoltysDb.Core.Test
{
    public class DataPageTests
    {
        [Fact]
        public void Length_DataPage_IsSameAsEveryPage()
        {
            var sut = new DataPage(new Page());
            Assert.Equal(Page.PageSize, sut.RawData.Length);
        }

        [Fact]
        public void RawData_ContainsDataFromConstructor()
        {
            var sut = new DataPage(new Page())
            {
                Data = new Span<byte>(new byte[] {1, 2, 3})
            };
            Assert.Equal(Page.PageSize, sut.RawData.Length);

            Assert.Equal(1, sut.Data[0]);
            Assert.Equal(2, sut.Data[1]);
            Assert.Equal(3, sut.Data[2]);
            Assert.Equal(0, sut.Data[3]);
        }
    }
}
