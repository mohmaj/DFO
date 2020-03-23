import numpy as np

# FITNESS FUNCTION (SPHERE FUNCTION)
def f(x): # x IS A VECTOR REPRESENTING ONE FLY
	sum = 0.0
	for i in range(len(x)):
		sum = sum + np.power(x[i],2)
	return sum

N = 100					# POPULATION SIZE
D = 30					# DIMENSIONALITY 
delta = 0.001			# DISTURBANCE THRESHOLD 
maxIterations = 1000	# ITERATIONS ALLOWED
lowerB = [-5.12]*D		# LOWER BOUND (IN ALL DIMENSIONS)
upperB = [ 5.12]*D		# UPPER BOUND (IN ALL DIMENSIONS)

# INITIALISATION PHASE
X = np.empty([N,D]) # EMPTY FLIES ARRAY OF SIZE: (N,D)
fitness = [None]*N  # EMPTY FITNESS ARRAY OF SIZE N

# INITIALISE FLIES WITHIN BOUNDS
for i in range(N):
	for d in range(D):
		X[i,d] = np.random.uniform(lowerB[d], upperB[d])
		
# MAIN DFO LOOP
for itr in range (maxIterations):
	for i in range(N): # EVALUATION
		fitness[i] = f(X[i,])
	s = np.argmin(fitness) # FIND BEST FLY

	if (itr%100 == 0): # PRINT BEST FLY EVERY 100 ITERATIONS
		print ("Iteration:", itr, "\tBest fly index:", s, 
			   "\tFitness value:", fitness[s])

	# TAKE EACH FLY INDIVIDUALLY 
	for i in range(N): 
		if i == s: continue # ELITIST STRATEGY

		# FIND BEST NEIGHBOUR
		left = (i-1)%N
		right = (i+1)%N
		bNeighbour = right if fitness[right]<fitness[left] else left

		for d in range(D): # UPDATE EACH DIMENSION SEPARATELY 
			if (np.random.rand() < delta):
				X[i,d] = np.random.uniform(lowerB[d], upperB[d])
				continue;

			u = np.random.rand()
			X[i,d] = X[bNeighbour,d] + u*(X[s,d] - X[i,d])

			# OUT OF BOUND CONTROL
			if X[i,d] < lowerB[d] or X[i,d] > upperB[d]:
				X[i,d] = np.random.uniform(lowerB[d], upperB[d])

for i in range(N): fitness[i] = f(X[i,]) # EVALUATION
s = np.argmin(fitness) # FIND BEST FLY

print("\nFinal best fitness:\t", fitness[s])
print("\nBest fly position:\n",  X[s,])	
