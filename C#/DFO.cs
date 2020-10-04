using System;

namespace UnixTime
{

public class Program
{
	/*
	Dispersive Flies Optimisation
	 
	Copyright (C) 2014 Mohammad Majid al-Rifaie
	 
	This is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License.
	 
	For any query contact:
	m.alrifaie@gre.ac.uk
	 
	School of Computing & Mathematical Sciences
	University of Greenwich, Old Royal Naval College, 
	Park Row, London SE10 9LS, U.K.
	 
	Reference to origianl paper:
	Mohammad Majid al-Rifaie (2014), Dispersive Flies Optimisation, Proceedings of the 2014 Federated Conference on Computer Science and Information Systems, 535--544. IEEE.
	 
	    @inproceedings{FedCSIS_2014,
			author={Mohammad Majid al-Rifaie},
			pages={535--544},
			title={Dispersive Flies Optimisation},
			booktitle={Proceedings of the 2014 Federated Conference on Computer Science and Information Systems},
			year={2014},
			editor={M. Ganzha, L. Maciaszek, M. Paprzycki},
			publisher={IEEE}
		}

	Note: Code converted from C++ by Hooman Oroojeni, H.Oroojeni@gold.ac.uk
	*/


	// FITNESS FUNCTION (SPHERE FUNCTION) 
	public static float f(float[] x, int D)
	{ // x IS ONE FLY AND D IS DIMENSION
		float sum = 0F;
		for (int i = 0; i < D; i++)
			sum = sum + x[i] * x[i];
		return sum;
	}

	// GENERATE RANODM NUMBER IN RANGE [0, 1)
	public static float r()
	{
		return (float) RandomNumbers.NextNumber() / (Int32.MaxValue);
	}

	public static void Main()
	{
		long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();		
		RandomNumbers.Seed((int)unixTime); // TO GENERATE DIFFERENRT RANDOM NUMBERS
		// INITIALISE PARAMETERS
		int N = 100;
		int D = 30;
		float delta = 0.001F;
		int maxIter = 1000;
		float[] lowerB = new float[D];
		float[] upperB = new float[D]; // LOWER AND UPPER BOUNDS
		int s = 0; // INITIAL INDEX OF BEST FLY
		float[][] X = RectangularArrays.RectangularFloatArray(N, D); // FLIES VECTORS
		float[] fitness = new float[N]; // FITNESS VALUES

		// SET LOWER AND UPPER BOUND CONSTRAINTS FOR EACH DIMENSION
		for (int d = 0; d < D; d++)
		{
			lowerB[d] = -5.12F;
			upperB[d] = 5.12F;
		}

		// INITIALISE FLIES WITHIN BOUNDS. MATRIX SIZE: (N,D)
		for (int i = 0; i < N; i++)
			for (int d = 0; d < D; d++)
				X[i][d] = lowerB[d] + r() * (upperB[d] - lowerB[d]);

		// MAIN DFO LOOP
		for (int itr = 0; itr < maxIter; itr++)
		{
			for (int i = 0; i < N; i++)
			{ // EVALUATE EACH FLY AND FIND THE BEST
				fitness[i] = f(X[i], D);
				if (fitness[i] <= fitness[s])
					s = i;							
			}

			if (itr % 100 == 0) // PRINT RESULT EVERY 100 ITERATIONS
			{
				Console.Write("Iteration: ");
				Console.Write(itr);
				Console.Write("\t Best fly index: ");
				Console.Write(s);
				Console.Write("\t Fitness value: ");
				Console.Write(fitness[s]);
				Console.Write("\n");
			}

			// UPDATE EACH FLY INDIVIDUALLY
			for (int i = 0; i < N; i++)
			{
				// ELITIST STRATEGY (i.e. DON'T UPDATE BEST FLY)
				if (i == s)		
					continue;
				
				// FIND BEST NEIGHBOUR FOR EACH FLY
				int left;
				int right;
				int bNeighbour;
				left = (i - 1) % N;
				if (left < 0) 
					left= N - 1;
				right = (i + 1) % N; // INDICES: LEFT & RIGHT FLIES
				if (fitness[right] < fitness[left])
					bNeighbour = right;
				else
					bNeighbour = left;

				// UPDATE EACH DIMENSION SEPARATELY 
				for (int d = 0; d < D; d++)
				{
					if (r() < delta)
					{ // DISTURBANCE MECHANISM
						X[i][d] = lowerB[d] + r() * (upperB[d] - lowerB[d]);
						continue;
					}

					// UPDATE EQUATION
					X[i][d] = X[bNeighbour][d] + r() * (X[s][d] - X[i][d]);

					// OUT OF BOUND CONTROL
					if (X[i][d] < lowerB[d] || X[i][d] > upperB[d])
						X[i][d] = (upperB[d] - lowerB[d]) * r() + lowerB[d];
					
				}
			}
		}
		// EVALUATE EACH FLY'S FITNESS AND FIND BEST FLY
		for (int i = 0; i < N; i++)
		{
			fitness[i] = f(X[i], D);
			if (fitness[i] < fitness[s])		
				s = i;
		}
		
		Console.Write("\n");
		Console.Write("Final best fitness: ");
		Console.Write(fitness[s]);
		Console.Write("\n\n");
		Console.Write("Best fly position:");
		Console.Write("\n\n");
		Console.Write("[");		
		for(int i=0; i<D; i++)  
		{  
		    Console.Write("{0}, ", X[s][i]);
			//if ( i%2 != 0 ) Console.Write('\n');	
	    }
		Console.Write("]");
		Console.Write("\n");
	}

}


internal static class RandomNumbers
{
	private static System.Random r;

	public static int NextNumber()
	{
		if (r == null)
			Seed();

		return r.Next();
	}

	public static int NextNumber(int ceiling)
	{
		if (r == null)
			Seed();

		return r.Next(ceiling);
	}

	public static void Seed()
	{
		r = new System.Random();
	}

	public static void Seed(int seed)
	{
		r = new System.Random(seed);
	}
}

internal static class RectangularArrays
{
	public static float[][] RectangularFloatArray(int size1, int size2)
	{
		float[][] newArray = new float[size1][];
		for (int array1 = 0; array1 < size1; array1++)
			newArray[array1] = new float[size2];

		return newArray;
	}
}
	
}
