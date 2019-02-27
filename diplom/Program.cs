using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    public delegate Output<T> ActivationFunction<T>(Input<T>[] inputs);
    public class Input<T>
    {
        public T Value {get; set;}
        public Input()
        {
            Value = default(T);
        }
    }
    public class Output<T>
    {
        public T Value { get; set; }
        public Output()
        {
            Value = default(T);
        }
    }
    public class MultiInput<T, Q> where Q : IEnumerable<Input<T>>, IList, new ()
    {
        public Q Inputs { get; set; }
        public MultiInput() 
        {
            Inputs = new Q();
        }

    }
    public class Neuron<T> 
    {
        public Input<T>[] Inputs { get; set; }
        public Output<T> Output { get; set; }
        private ActivationFunction<T> AF;
        public Neuron() : this (10)
        {

        }
        public Neuron(int numberOfInputs)
        {
            Inputs = new Input<T>[numberOfInputs];
            Output = new Output<T>();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
        }
    }
}
