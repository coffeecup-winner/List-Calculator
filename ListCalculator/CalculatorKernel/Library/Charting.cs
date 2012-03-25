using IronPython.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PythonList = IronPython.Runtime.List;

namespace CalculatorKernel.Library {
    public static class Charting {
        static T CreateSeries<T>(IList<object> independent, IList<object> dependent, string title) where T : ChartSeries, new() {
            Contract.Requires(independent.Count == dependent.Count);
            T series = new T { Title = title };
            for(int i = 0; i < dependent.Count; i++)
                series.Add(independent[i], dependent[i]);
            return series;
        }
        static T CreateSeries<T>(IList<object> independent, PythonFunction function, string title) where T : ChartSeries, new() {
            dynamic f = function;
            return CreateSeries<T>(independent, independent.Select(x => f(x)).ToList(), title);
        }
        #region AreaChart
        public static AreaChartSeries AreaChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<AreaChartSeries>(independent, dependent, title);
        }
        public static AreaChartSeries AreaChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<AreaChartSeries>(independent, function, title);
        }
        #endregion
        #region BarChart
        public static BarChartSeries BarChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<BarChartSeries>(independent, dependent, title);
        }
        public static BarChartSeries BarChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<BarChartSeries>(independent, function, title);
        }
        #endregion
        #region BubbleChart
        public static BubbleChartSeries BubbleChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<BubbleChartSeries>(independent, dependent, title);
        }
        public static BubbleChartSeries BubbleChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<BubbleChartSeries>(independent, function, title);
        }
        #endregion
        #region ColumnChart
        public static ColumnChartSeries ColumnChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<ColumnChartSeries>(independent, dependent, title);
        }
        public static ColumnChartSeries ColumnChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<ColumnChartSeries>(independent, function, title);
        }
        #endregion
        #region LineChart
        public static LineChartSeries LineChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<LineChartSeries>(independent, dependent, title);
        }
        public static LineChartSeries LineChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<LineChartSeries>(independent, function, title);
        }
        #endregion
        #region PieChart
        public static PieChartSeries PieChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<PieChartSeries>(independent, dependent, title);
        }
        public static PieChartSeries PieChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<PieChartSeries>(independent, function, title);
        }
        #endregion
        #region ScatterChart
        public static ScatterChartSeries ScatterChart(IList<object> independent, IList<object> dependent, string title = "") {
            return CreateSeries<ScatterChartSeries>(independent, dependent, title);
        }
        public static ScatterChartSeries ScatterChart(IList<object> independent, PythonFunction function, string title = "") {
            return CreateSeries<ScatterChartSeries>(independent, function, title);
        }
        #endregion
    }

    public abstract class ChartSeries : PythonList {
        public string Title { get; set; }
        public void Add(object x, object y) {
            append(new ChartSeriesValue { X = x, Y = y });
        }
    }
    public class AreaChartSeries : ChartSeries { }
    public class BarChartSeries : ChartSeries { }
    public class BubbleChartSeries : ChartSeries { }
    public class ColumnChartSeries : ChartSeries { }
    public class LineChartSeries : ChartSeries { }
    public class PieChartSeries : ChartSeries { }
    public class ScatterChartSeries : ChartSeries { }

    public struct ChartSeriesValue {
        public object X { get; set; }
        public object Y { get; set; }
    }
}
