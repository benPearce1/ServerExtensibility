﻿using System;
using System.Collections.Generic;
using Assent;
using Newtonsoft.Json;
using NUnit.Framework;
using Octopus.Node.Extensibility.Metadata;

namespace Node.Extensibility.Tests.Metadata
{
    [TestFixture]
    public class MetadataGeneratorFixture
    {
        [Test]
        public void SettingsMetadataShouldBeCorrect()
        {
            IGenerateMetadata generator = new MetadataGenerator();

            var metadata = generator.GetMetadata<TopLevelResource>();

            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            var json = JsonConvert.SerializeObject(metadata, serializerSettings);

            this.Assent(json);

        }

        [Test]
        public void MetadataShouldHandleSelfReferences()
        {
            IGenerateMetadata generator = new MetadataGenerator();

            var metadata = generator.GetMetadata<SelfReferencingResource>();

            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            var json = JsonConvert.SerializeObject(metadata, serializerSettings);

            this.Assent(json);
        }

        [Test]
        public void MetadataShouldHandleNavigationalProperties()
        {
            IGenerateMetadata generator = new MetadataGenerator();

            var metadata = generator.GetMetadata<ParentResource>();

            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            var json = JsonConvert.SerializeObject(metadata, serializerSettings);

            this.Assent(json);
        }

        public void SettingsValuesShouldBeCorrect()
        {
            var resource = new TopLevelResource()
            {
                SecondLevelResource = new SecondLevelResource()
                {
                    StringProperty = "String value",
                    BoolProperty = false,
                    IntArrayProperty = new[] { 1, 2, 3 },
                    NullableDateTimeOffsetProperty = null,
                    StringArrayProperty = new[] { "first", "second" },
                },
                DateTimeOffsetProperty = DateTime.Now,
                IntProperty = 4,
                NullableDateTimeProperty = null,
                NullableIntProperty = 5,
                ListOfStringProperty = new List<string> { "1st", "2nd", "3rd" },
            };
        }

        public enum TestEnum
        {
            First = 5, Second = 7, Third = 11, Fourth
        }

        public abstract class SettingsResource 
        {
            protected SettingsResource()
            {
                Id = GetType().Name;
            }

            public string Id { get; set; }

            public string SomeValue { get; set; }
        }

        public class SelfReferencingResource : SettingsResource
        {
            public string Name { get; set; }

            public SelfReferencingResource SelfReference { get; set; }
        }

        public class ParentResource : SettingsResource
        {
            public List<ChildResource> Children { get; set; }
        }

        public class ChildResource
        {
            public ParentResource Parent { get; set; }

            public int ChildIntProperty { get; set; }
        }


        public class TopLevelResource : SettingsResource
        {
            public SecondLevelResource SecondLevelResource { get; set; }

            [DisplayLabel("Duplicated 2nd Level")]
            [Octopus.Node.Extensibility.Metadata.Description("This 2nd-level resource has been duplicated")]
            public SecondLevelResource DuplicateSecondLevelResource { get; set; }

            [Required]
            public DateTime? NullableDateTimeProperty { get; set; }

            public DateTimeOffset DateTimeOffsetProperty { get; set; }

            [HasOptions]
            [MultiSelect]
            public int IntProperty { get; set; }

            [HasOptions]
            public TestEnum EnumProp { get; set; }

            public int? NullableIntProperty { get; set; }

            public List<string> ListOfStringProperty { get; set; }

            public DateTime?[] NullableDateTimeArray { get; set; }
        }

        public class SecondLevelResource
        {
            [Sensitive]
            public string StringProperty { get; set; }

            public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }

            public bool BoolProperty { get; set; }

            [ListApi("")]
            [MultiSelect]
            public string[] StringArrayProperty { get; set; }

            [ListApi("")]
            public int[] IntArrayProperty { get; set; }
        }
    }
}
