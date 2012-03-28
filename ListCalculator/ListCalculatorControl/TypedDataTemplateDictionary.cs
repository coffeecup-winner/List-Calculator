using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace ListCalculatorControl {
    public class DataTemplateInfo {
        public DataTemplate DataTemplate { get; set; }
        public string Name { get; set; }
    }
    public class TypedDataTemplateDictionary : LazyCacheBase<Type, List<DataTemplateInfo>> {
        readonly Dictionary<Type, DataTemplateInfo> templates = new Dictionary<Type, DataTemplateInfo>();
        readonly TypeHierarchyCache typeHierarchyCache = new TypeHierarchyCache();

        protected Dictionary<Type, DataTemplateInfo> Templates { get { return templates; } }
        protected TypeHierarchyCache TypeHierarchyCache { get { return typeHierarchyCache; } }
        protected DataTemplateInfo GetTemplateOrNull(Type type) {
            DataTemplateInfo result;
            return Templates.TryGetValue(type, out result) ? result : null;
        }
        public void AddTemplateFor(Type type, string name, DataTemplate dataTemplate) {
            Templates.Add(type, new DataTemplateInfo { DataTemplate = dataTemplate, Name = name });
            InvalidateValues(t => IsOrDerivedFrom(type));
        }
        bool IsOrDerivedFrom(Type type) {
            return TypeHierarchyCache.GetTypeHierarchyFor(type).Contains(type);
        }
        public DataTemplate GetBestTemplateFor(Type type) {
            return GetTemplatesFor(type)[0].DataTemplate;
        }
        public List<DataTemplateInfo> GetTemplatesFor(Type type) {
            return GetCachedOrCacheNew(type);
        }
        #region Alternative syntax
        public void AddTemplateFor<T>(string name, DataTemplate dataTemplate) {
            AddTemplateFor(typeof(T), name, dataTemplate);
        }
        public DataTemplate GetBestTemplateFor<T>() {
            return GetBestTemplateFor(typeof(T));
        }
        public List<DataTemplateInfo> GetTemplatesFor<T>() {
            return GetTemplatesFor(typeof(T));
        }
        #endregion
        protected override List<DataTemplateInfo> GetValueFor(Type key) {
            return TypeHierarchyCache.GetTypeHierarchyFor(key).Select(type => GetTemplateOrNull(type)).Where(dt => dt != null).ToList();
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