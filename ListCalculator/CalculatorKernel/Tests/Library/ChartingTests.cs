using CalculatorKernel.Library;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculatorKernel.Kernel;

namespace CalculatorKernel.Tests {
    [TestFixture]
    public class ChartingTests {
        Kernel.Kernel kernel;

        protected Kernel.Kernel Kernel { get { return kernel; } }
        [TestFixtureSetUp]
        public void FixtureSetUp() {
            kernel = new Kernel.Kernel();
        }
        [Test]
        public void AreaChartTest() {
            TestChart<AreaChartSeries>("AreaChart");
        }
        [Test]
        public void BarChartTest() {
            TestChart<BarChartSeries>("BarChart");
        }
        [Test]
        public void BubbleChartTest() {
            TestChart<BubbleChartSeries>("BubbleChart");
        }
        [Test]
        public void ColumnChartTest() {
            TestChart<ColumnChartSeries>("ColumnChart");
        }
        [Test]
        public void LineChartTest() {
            TestChart<LineChartSeries>("LineChart");
        }
        [Test]
        public void PieChartTest() {
            TestChart<PieChartSeries>("PieChart");
        }
        [Test]
        public void ScatterChartTest() {
            TestChart<ScatterChartSeries>("ScatterChart");
        }
        void TestChart<TSeries>(string functionName) where TSeries : ChartSeries {
            TestChartLists<TSeries>(functionName);
            TestChartFunction<TSeries>(functionName);
        }
        void TestChartLists<TSeries>(string functionName) where TSeries : ChartSeries {
            TSeries x = Kernel.Calculate<TSeries>(functionName + "([1, 2, 3], [4, 5, 6])").Value;
            Assert.That(x.Title, Is.Empty);
            Assert.That(x[0], Is.EqualTo(new ChartSeriesValue { X = 1, Y = 4 }));
            Assert.That(x[1], Is.EqualTo(new ChartSeriesValue { X = 2, Y = 5 }));
            Assert.That(x[2], Is.EqualTo(new ChartSeriesValue { X = 3, Y = 6 }));
            x = Kernel.Calculate<TSeries>(functionName + "([1, 2, 3], [4, 5, 6], 'title')").Value;
            Assert.That(x.Title, Is.EqualTo("title"));
            Assert.That(x[0], Is.EqualTo(new ChartSeriesValue { X = 1, Y = 4 }));
            Assert.That(x[1], Is.EqualTo(new ChartSeriesValue { X = 2, Y = 5 }));
            Assert.That(x[2], Is.EqualTo(new ChartSeriesValue { X = 3, Y = 6 }));
        }
        void TestChartFunction<TSeries>(string functionName) where TSeries : ChartSeries {
            TSeries x = Kernel.Calculate<TSeries>(functionName + "([1, 2, 3], lambda x: x * 2)").Value;
            Assert.That(x.Title, Is.Empty);
            Assert.That(x[0], Is.EqualTo(new ChartSeriesValue { X = 1, Y = 2 }));
            Assert.That(x[1], Is.EqualTo(new ChartSeriesValue { X = 2, Y = 4 }));
            Assert.That(x[2], Is.EqualTo(new ChartSeriesValue { X = 3, Y = 6 }));
            x = Kernel.Calculate<TSeries>(functionName + "([1, 2, 3], lambda x: x * 2, 'title')").Value;
            Assert.That(x.Title, Is.EqualTo("title"));
            Assert.That(x[0], Is.EqualTo(new ChartSeriesValue { X = 1, Y = 2 }));
            Assert.That(x[1], Is.EqualTo(new ChartSeriesValue { X = 2, Y = 4 }));
            Assert.That(x[2], Is.EqualTo(new ChartSeriesValue { X = 3, Y = 6 }));
        }
    }
}
