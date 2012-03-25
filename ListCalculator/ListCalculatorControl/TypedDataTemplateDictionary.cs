﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace ListCalculatorControl {
    public class TypedDataTemplateDictionary : LazyCacheBase<Type, List<DataTemplate>> {
        readonly Dictionary<Type, DataTemplate> templates = new Dictionary<Type, DataTemplate>();
        readonly TypeHierarchyCache typeHierarchyCache = new TypeHierarchyCache();

        protected DataTemplate GetTemplateOrNull(Type type) {
            DataTemplate result;
            return templates.TryGetValue(type, out result) ? result : null;
        }
        public void AddTemplateFor(Type type, DataTemplate dataTemplate) {
            templates.Add(type, dataTemplate);
        }
        public DataTemplate GetBestTemplateFor(Type type) {
            return GetTemplatesFor(type)[0];
        }
        public List<DataTemplate> GetTemplatesFor(Type type) {
            return GetCachedOrCacheNew(type);
        }
        #region Alternative syntax
        public void AddTemplateFor<T>(DataTemplate dataTemplate) {
            AddTemplateFor(typeof(T), dataTemplate);
        }
        public DataTemplate GetBestTemplateFor<T>() {
            return GetBestTemplateFor(typeof(T));
        }
        public List<DataTemplate> GetTemplatesFor<T>() {
            return GetTemplatesFor(typeof(T));
        }
        #endregion
        protected override List<DataTemplate> GetValueFor(Type key) {
            return typeHierarchyCache.GetTypeHierarchyFor(key).Select(type => GetTemplateOrNull(type)).Where(dt => dt != null).ToList();
        }
    }

    public class TypeHierarchyCache : LazyCacheBase<Type, List<Type>> {
        public List<Type> GetTypeHierarchyFor(Type type) {
            return GetCachedOrCacheNew(type);
        }
        public List<Type> GetTypeHierarchyFor<T>() {
            return GetTypeHierarchyFor(typeof(T));
        }
        protected override List<Type> GetValueFor(Type type) {
            Type current = type;
            List<Type> result = new List<Type>();
            while(current != null) {
                result.Add(current);
                current = current.BaseType;
            }
            return result;
        }
    }
}