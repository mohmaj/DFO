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
*/

#include <iostream>
#include <stdlib.h>
#include <cmath>
#include <ctime>
using namespace std;

// FITNESS FUNCTION (SPHERE FUNCTION) 
float f(float x[], int D) { // x IS ONE FLY AND D IS DIMENSION
	float sum=0;
	for(int i=0; i<D; i++) 
		sum=sum+x[i]*x[i];
	return sum;
}

// GENERATE RANODM NUMBER IN RANGE [0, 1)
float r() { 
	return (float) rand() / (RAND_MAX); 
}

int main() {
	srand(time(NULL)); // TO GENERATE DIFFERENRT RANDOM NUMBERS
	// INITIALISE PARAMETERS
	int N=100; int D = 30; float delta = 0.001; int maxIter=1000; 
	float lowerB[D]; float upperB[D]; // LOWER AND UPPER BOUNDS
	int s = 0;			// INITIAL INDEX OF BEST FLY
	float X[N][D]; 		// FLIES VECTORS
	float fitness[N]; 	// FITNESS VALUES 

	// SET LOWER AND UPPER BOUND CONSTRAINTS FOR EACH DIMENSION
	for (int d=0; d<D; d++) {
		lowerB[d]=-5.12; upperB[d]=5.12;
	}
	
	// INITIALISE FLIES WITHIN BOUNDS. MATRIX SIZE: (N,D)
	for(int i=0; i<N; i++) 
		for(int d=0; d<D; d++) 
			X[i][d] = lowerB[d] + r()*(upperB[d]-lowerB[d]);
	
	// MAIN DFO LOOP
	for (int itr=0; itr<maxIter; itr++) {
		for(int i=0; i<N; i++) { // EVALUATE EACH FLY AND FIND THE BEST
			fitness[i]=f(X[i],D);
			if (fitness[i] <= fitness[s]) s = i;
		}
		
		if ( itr%100 == 0) // PRINT RESULT EVERY 100 ITERATIONS
			cout << "Iteration: " << itr << "\t Best fly index: " << s 
			<< "\t Fitness value: " << fitness[s] << endl;
		
		// UPDATE EACH FLY INDIVIDUALLY
		for(int i=0; i<N; i++) {
			// ELITIST STRATEGY (i.e. DON'T UPDATE BEST FLY)
			if (i==s) continue;
			
			// FIND BEST NEIGHBOUR FOR EACH FLY
			int left; int right; int bNeighbour;
			left=(i-1)%N; right=(i+1)%N; // INDICES: LEFT & RIGHT FLIES
			if (fitness[right]<fitness[left]) bNeighbour = right;
			else bNeighbour = left;
				
			// UPDATE EACH DIMENSION SEPARATELY 
			for (int d=0; d<D; d++) {
				if (r() < delta) { // DISTURBANCE MECHANISM
					X[i][d] = lowerB[d] + r()*(upperB[d]-lowerB[d]); continue;
				}
				
				// UPDATE EQUATION
				X[i][d] = X[bNeighbour][d] + r()*( X[s][d]- X[i][d] );
				
				// OUT OF BOUND CONTROL
				if (X[i][d] < lowerB[d] or X[i][d] > upperB[d])
					X[i][d] = lowerB[d] + r()*(upperB[d]-lowerB[d]);
			}
		}
	}
	// EVALUATE EACH FLY'S FITNESS AND FIND BEST FLY
	for(int i=0; i<N; i++) {
		fitness[i]=f(X[i],D);
		if (fitness[i] < fitness[s]) s = i;
	}
	cout << "Final best fitness: " << fitness[s] << endl;
	return 0;
}
