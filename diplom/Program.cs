using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    public delegate T ActivationFunction<T>(T[] inputs);
    /*
    public class MultiInput<T, Q> where Q : IEnumerable<Input<T>>, IList, new ()
    {
        public Q Inputs { get; set; }
        public MultiInput() 
        {
            Inputs = new Q();
        }

    }
    */
    public class ActivationFunctions<T> where T : Neuron<T>
    {
        public static T Sigmoid<T>()
        {
            return null;
        }
    }
    public class Neuron<T> 
    {
        public T[] Inputs { get; set; }
        public T Output { get; set; }
        private F _activationFunction;
        public Neuron() : this (10)
        {

        }
        public Neuron(int numberOfInputs)
        {
            Inputs = new T[numberOfInputs];
            Output = _activationFunction;
            _activationFunction = ActivationFunction<T>;
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
        }
    }
}
