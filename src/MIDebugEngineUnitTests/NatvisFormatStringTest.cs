using Xunit;
using Microsoft.MIDebugEngine.Natvis;
using Microsoft.MIDebugEngine;

namespace MIDebugEngineUnitTests
{
    /// <summary>
    /// unit tests for natvis format string processing with format specifiers
    /// tests the handling of format specifiers like, 'na', sub (UTF-16), sb (ASCII).
    /// </summary>
    public class NatvisFormatStringTest
    {
        // -- format specifier with template parameters ($T1, $T2,) --
        [Fact]
        public void ExtractFormatSpecifier_HasTemplateParameter_TemplateSubstituted()
        {
            string spec = Natvis.ExtractFormatSpecifier("myVector,sub");
            Assert.Equal("sub", spec);
        }

        // -- Format specifier na --

        [Fact]
        public void ExtractFormatSpecifier_WithNaModifier_HasNaIsTrue()
        {
            string spec = Natvis.ExtractFormatSpecifier("someExpr,na", out bool hasNa);
            Assert.Equal("", spec);
            Assert.True(hasNa);
        }

        [Fact]
        public void ExtractFormatSpecifier_WithNaAndFormat_HasNaIsTrueAndSpecReturned()
        {
            string spec = Natvis.ExtractFormatSpecifier("myString,subna", out bool hasNa);
            Assert.Equal("sub", spec);
            Assert.True(hasNa);
        }

        [Fact]
        public void ExtractFormatSpecifier_WithoutNaModifier_HasNaIsFalse()
        {
            string spec = Natvis.ExtractFormatSpecifier("myString,sub", out bool hasNa);
            Assert.Equal("sub", spec);
            Assert.False(hasNa);
        }

        // -- UTF-16 and ASCII string cleanup --

        [Fact]
        public void CleanUtf16StringValue_WithAddressAndQuotes_AddressAndQuotesStripped()
        {
            string result = Natvis.CleanUtf16StringValue("0x00007fff5fbff6c0 u\"Hello\"");
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CleanUtf16StringValue_WithCapitalU_QuotesStripped()
        {
            string result = Natvis.CleanUtf16StringValue("U\"Hello\"");
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CleanUtf16StringValue_JustAddress_AddressStripped()
        {
            string result = Natvis.CleanUtf16StringValue("0x00007fff u");
            Assert.Equal("u", result);
        }

        [Fact]
        public void CleanAsciiStringValue_WithAddressAndQuotes_AddressAndQuotesStripped()
        {
            string result = Natvis.CleanAsciiStringValue("0x00007fff5fbff6c0 \"Hello\"");
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CleanAsciiStringValue_TruncatedNoClosingQuote_OpeningQuoteStripped()
        {
            string result = Natvis.CleanAsciiStringValue("0x00007fff \"Hello...");
            Assert.Equal("Hello...", result);
        }
    }
}

