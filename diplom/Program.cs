using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    public class ActivationFunctions<T> 
    {
        public delegate U ActivationFunction<U>(U[] inputs) where U: T;
        public static U Sigmoid<U>(U[] inputs) where U : T
        {
            return default(U);
        }
    }
    public abstract class Neuron<T> 
    {
        public T[] Inputs
        {
            get;
            /* не работает
            {
                return Inputs;
            }
            */
            set;
            /*
            {
                
                for (int i = 0; i < Inputs.Count(); i++)
                {
                    Inputs[i] = value[i];
                }
                
                Refresh();
            } 
            */
        }
        public T Output { get; set; }
        private ActivationFunctions<T>.ActivationFunction<T> activationFunction;
        public Neuron() : this (1)
        {}
        public Neuron(int numberOfInputs)
        {
            Inputs = new T[numberOfInputs];
            Output = default(T);
        }
        public void Refresh()
        {
            Output = activationFunction(Inputs);
        }
    }
    public class testNeuron : Neuron<double>
    {
        private ActivationFunctions<double>.ActivationFunction<double> activationFunction = ActivationFunctions<double>.Sigmoid;
        public testNeuron() : base()
        { }
        public testNeuron(int numberOfInputs) : base(numberOfInputs)
        { }
    }
    class Program
    {
        static void Main(string[] args)
        {
            testNeuron tn = new testNeuron(13);
            tn.Refresh();
            tn.Inputs[11] = 2.0;
        }
    }
}
