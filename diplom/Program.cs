using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    interface IActivationFunction
    { }
    interface IInput { }
    interface IOutput { }
    public class Neuron
    {
        public IEnumerable<IInput> Input { get; set; }
        public IEnumerable<double> Output { get; set; }
        private readonly ActivationFunction AF;
        public Neuron()
        {
            Input = new double[1];
            Output = new double[1];
            AF = new ActivationFunction();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            
        }
    }
}
