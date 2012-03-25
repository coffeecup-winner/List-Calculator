﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Windows;

namespace ListCalculatorControl.Tests {
    [TestFixture]
    public class TypedDataTemplateDictionaryTests {
        TypedDataTemplateDictionary templateDictionary;

        protected TypedDataTemplateDictionary TemplateDictionary { get { return templateDictionary; } }
        [SetUp]
        public void SetUp() {
            this.templateDictionary = new TypedDataTemplateDictionary();
        }
        [Test]
        public void GetExistingTemplateTest() {
            DataTemplate stringTemplate = new DataTemplate();
            TemplateDictionary.AddTemplateFor<string>(stringTemplate);
            Assert.That(TemplateDictionary.GetBestTemplateFor<string>(), Is.EqualTo(stringTemplate));
        }
        [Test]
        public void GetFallbackTemplateTest() {
            DataTemplate objectTemplate = new DataTemplate();
            TemplateDictionary.AddTemplateFor<object>(objectTemplate);
            Assert.That(TemplateDictionary.GetBestTemplateFor<object>(), Is.EqualTo(objectTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<string>(), Is.EqualTo(objectTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<int>(), Is.EqualTo(objectTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<FrameworkElement>(), Is.EqualTo(objectTemplate));
        }
        class CustomListInt : List<int> { }
        [Test]
        public void GetFallbackTemplateComplexTest() {
            DataTemplate objectTemplate = new DataTemplate();
            DataTemplate stringTemplate = new DataTemplate();
            DataTemplate listIntTemplate = new DataTemplate();
            TemplateDictionary.AddTemplateFor<object>(objectTemplate);
            TemplateDictionary.AddTemplateFor<string>(stringTemplate);
            TemplateDictionary.AddTemplateFor<List<int>>(listIntTemplate);
            Assert.That(TemplateDictionary.GetBestTemplateFor<object>(), Is.EqualTo(objectTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<string>(), Is.EqualTo(stringTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<List<int>>(), Is.EqualTo(listIntTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<CustomListInt>(), Is.EqualTo(listIntTemplate));
            Assert.That(TemplateDictionary.GetBestTemplateFor<FrameworkElement>(), Is.EqualTo(objectTemplate));
        }
        [Test]
        public void GetTemplatesTest() {
            DataTemplate objectTemplate = new DataTemplate();
            DataTemplate stringTemplate = new DataTemplate();
            TemplateDictionary.AddTemplateFor<object>(objectTemplate);
            TemplateDictionary.AddTemplateFor<string>(stringTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<string>(), stringTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor(typeof(string).BaseType), objectTemplate);
        }
        class A { }
        class B : A { }
        class C : B { }
        class D : C { }
        [Test]
        public void GetTemplatesWithRaggedTemplateHierarchyTest() {
            DataTemplate objectTemplate = new DataTemplate();
            DataTemplate bTemplate = new DataTemplate();
            DataTemplate dTemplate = new DataTemplate();
            TemplateDictionary.AddTemplateFor<object>(objectTemplate);
            TemplateDictionary.AddTemplateFor<B>(bTemplate);
            TemplateDictionary.AddTemplateFor<D>(dTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<D>(), dTemplate, bTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<C>(), bTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<B>(), bTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<A>(), objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<object>(), objectTemplate);
        }
        class B2 : A { }
        class C2 : B2 { }
        [Test]
        public void GetTemplatesWithTemplateHierarchyTest() {
            DataTemplate objectTemplate = new DataTemplate();
            DataTemplate aTemplate = new DataTemplate();
            DataTemplate cTemplate = new DataTemplate();
            DataTemplate b2Template = new DataTemplate();
            TemplateDictionary.AddTemplateFor<object>(objectTemplate);
            TemplateDictionary.AddTemplateFor<A>(aTemplate);
            TemplateDictionary.AddTemplateFor<C>(cTemplate);
            TemplateDictionary.AddTemplateFor<B2>(b2Template);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<object>(), objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<A>(), aTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<B>(), aTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<C>(), cTemplate, aTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<B2>(), b2Template, aTemplate, objectTemplate);
            AssertTemplates(TemplateDictionary.GetTemplatesFor<C2>(), b2Template, aTemplate, objectTemplate);
        }
        void AssertTemplates(List<DataTemplate> actualTemplates, params DataTemplate[] expectedTemplates) {
            Assert.That(actualTemplates.Count, Is.EqualTo(expectedTemplates.Length));
            for(int i = 0; i < actualTemplates.Count; i++)
                Assert.That(actualTemplates[i], Is.EqualTo(expectedTemplates[i]));
        }
    }

    [TestFixture]
    public class TypeHierarchyCacheTests {
        TypeHierarchyCache cache;

        protected TypeHierarchyCache Cache { get { return cache; } }
        [SetUp]
        public void SetUp() {
            this.cache = new TypeHierarchyCache();
        }
        [Test]
        public void TestsClassTest() {
            AssertTypeHierarchy<TypeHierarchyCacheTests, Object>(Cache.GetTypeHierarchyFor<TypeHierarchyCacheTests>());
        }
        [Test]
        public void SelfTest() {
            AssertTypeHierarchy<TypeHierarchyCache, LazyCacheBase<Type, List<Type>>, Object>(Cache.GetTypeHierarchyFor<TypeHierarchyCache>());
        }
        [Test]
        public void StructTest() {
            AssertTypeHierarchy<Int32, ValueType, Object>(Cache.GetTypeHierarchyFor<int>());
        }
        [Test]
        public void ObjectTest() {
            AssertTypeHierarchy<Object>(Cache.GetTypeHierarchyFor<object>());
        }
        class A { }
        class B : A { }
        [Test]
        public void CustomClassesTest() {
            AssertTypeHierarchy<B, A, Object>(Cache.GetTypeHierarchyFor<B>());
        }
        #region Assertion helpers
        void AssertTypeHierarchy<T1>(List<Type> hierarchy) {
            Assert.That(hierarchy.Count, Is.EqualTo(1));
            Assert.That(hierarchy[0], Is.EqualTo(typeof(T1)));
        }
        void AssertTypeHierarchy<T1, T2>(List<Type> hierarchy) {
            Assert.That(hierarchy.Count, Is.EqualTo(2));
            Assert.That(hierarchy[0], Is.EqualTo(typeof(T1)));
            Assert.That(hierarchy[1], Is.EqualTo(typeof(T2)));
        }
        void AssertTypeHierarchy<T1, T2, T3>(List<Type> hierarchy) {
            Assert.That(hierarchy.Count, Is.EqualTo(3));
            Assert.That(hierarchy[0], Is.EqualTo(typeof(T1)));
            Assert.That(hierarchy[1], Is.EqualTo(typeof(T2)));
            Assert.That(hierarchy[2], Is.EqualTo(typeof(T3)));
        }
        #endregion
    }
}