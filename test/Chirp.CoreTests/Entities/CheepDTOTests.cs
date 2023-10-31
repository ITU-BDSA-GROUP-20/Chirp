using System.ComponentModel.DataAnnotations;
using Chirp.Core.Entities;
using Microsoft.EntityFrameworkCore;

//Co-authored by ChatGPT-3.5

namespace Chirp.CoreTests.Entities;

public class CheepDTOTests
{
    [Fact]
        public void CheepDTO_CheepId_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(CheepDTO).GetProperty("CheepId");
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;

            Assert.NotNull(requiredAttribute);
        }

        [Fact]
        public void CheepDTO_AuthorId_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(CheepDTO).GetProperty("AuthorId");
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;

            Assert.NotNull(requiredAttribute);
        }

        [Fact]
        public void CheepDTO_AuthorDto_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(CheepDTO).GetProperty("AuthorDto");
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;

            Assert.NotNull(requiredAttribute);
        }

        [Fact]
        public void CheepDTO_Text_ShouldHaveStringLengthAttributeWithMinMax()
        {
            var propertyInfo = typeof(CheepDTO).GetProperty("Text");
            var stringLengthAttribute = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;

            Assert.NotNull(stringLengthAttribute);
            Assert.Equal(5, stringLengthAttribute.MinimumLength);
            Assert.Equal(128, stringLengthAttribute.MaximumLength);
        }

        [Fact]
        public void CheepDTO_TimeStamp_ShouldHaveRequiredAttribute()
        {
            var propertyInfo = typeof(CheepDTO).GetProperty("TimeStamp");
            var requiredAttribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;

            Assert.NotNull(requiredAttribute);
        }

        [Fact]
        public void CheepDTO_IndexAttribute_ShouldBeUnique()
        {
            var indexAttribute = typeof(CheepDTO).GetCustomAttributes(typeof(IndexAttribute), true).FirstOrDefault() as IndexAttribute;

            Assert.NotNull(indexAttribute);
            Assert.True(indexAttribute.IsUnique);
        }
}