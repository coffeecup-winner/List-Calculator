using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace CalculatorKernel.Kernel {
    public class IronPythonWrapper {
        ScriptEngine engine;
        ScriptScope scope;

        public IronPythonWrapper() {
            this.engine = Python.CreateEngine();
            this.scope = Engine.CreateScope();
        }
        protected ScriptEngine Engine { get { return engine; } }
        protected ScriptScope Scope { get { return scope; } }
        public dynamic Execute(string expression) {
            return Engine.Execute(expression, scope);
        }
        public T Execute<T>(string expression) {
            return Engine.Execute<T>(expression, scope);
        }
        public void Reset() {
            this.scope = Engine.CreateScope();
        }
    }
}
