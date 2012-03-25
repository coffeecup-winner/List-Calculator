using IronPython.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PythonList = IronPython.Runtime.List;

namespace CalculatorKernel.Library {
    public static class Charting {
        public static LineChartSeries LineChart(IList<object> independent, IList<object> dependent, string title = "") {
            Contract.Requires(independent.Count == dependent.Count);
            LineChartSeries series = new LineChartSeries() { Title = title };
            for(int i = 0; i < dependent.Count; i++)
                series.Add(independent[i], dependent[i]);
            return series;
        }
        public static LineChartSeries LineChart(IList<object> independent, PythonFunction function, string title = "") {
            dynamic f = function;
            return LineChart(independent, independent.Select(x => f(x)).ToList(), title);
        }
    }

    public abstract class ChartSeries : PythonList {
        public string Title { get; set; }
        public void Add(object x, object y) {
            append(new ChartSeriesValue { X = x, Y = y });
        }
    }
    public class LineChartSeries : ChartSeries { }

    public struct ChartSeriesValue {
        public object X { get; set; }
        public object Y { get; set; }
    }
}
