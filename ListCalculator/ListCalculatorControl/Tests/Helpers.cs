using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace ListCalculatorControl.Tests {
    public static class LayoutHelper {
        public static FrameworkElement FindElementByName(FrameworkElement root, string name) {
            return (FrameworkElement)root.FindName(name);
        }
        public static T FindChild<T>(DependencyObject depObj, string childName) where T : DependencyObject {
            // Confirm obj is valid. 
            if(depObj == null) return null;

            // success case
            if(depObj is T && ((FrameworkElement)depObj).Name == childName)
                return depObj as T;

            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                //DFS
                T obj = FindChild<T>(child, childName);

                if(obj != null)
                    return obj;
            }
            return null;
        }
        //public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject {
        //    if(parent == null) return null;
        //    T foundChild = null;
        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        //    for(int i = 0; i < childrenCount; i++) {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        T childType = child as T;
        //        if(childType == null) {
        //            foundChild = FindChild<T>(child, childName);
        //            if(foundChild != null) break;
        //        } else if(!string.IsNullOrEmpty(childName)) {
        //            FrameworkElement frameworkElement = child as FrameworkElement;
        //            if(frameworkElement != null && frameworkElement.Name == childName) {
        //                foundChild = (T)child;
        //                break;
        //            }
        //        } else {
        //            foundChild = (T)child;
        //            break;
        //        }
        //    }
        //    return foundChild;
        //}
    }
}
