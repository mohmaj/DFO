clear
% INITIALISE PARAMETERS (N: swarm size, D: dimensionality)
N=100; D = 30; delta = 0.001; maxIter=1000;
% LOWER AND UPPER BOUNDS
lowerB(1,1:D)=-5.12; upperB(1,1:D)=5.12;
s = 0;			% INITIAL INDEX OF BEST FLY
X = zeros(N,D);	% FLIES MATRIX
fitness(1,1:N) = realmax; % FITNESS VALUES 

for i=1:N
	for d=1:D
		X(i,d) = lowerB(d)+rand()*(upperB(d)-lowerB(d));
	end
end

for itr=1:maxIter
	% EVALUATE FITNESS OF THE FLIES AND FIND THE BEST
	fitness = f(X);
	[sFitness, s] = min(fitness);
	disp(['Iteration: ' , num2str(itr),'    Best fly index: ', num2str(s), '    Fitness value: ', num2str(sFitness), ] ) 

	% UPDATE EACH FLY INDIVIDUALLY
	for i=1:N
		% ELITIST STRATEGY (DON'T UPDATE BEST FLY)
		if i == s continue; end

		% FIND BEST NEIGHBOUR FOR EACH FLY
		left=mod(i-2,N)+1; right=mod(i,N)+1; % INDICES: LEFT & RIGHT FLIES
		if fitness(right)<fitness(left) bNeighbour = right; 
		else bNeighbour = left; end	

		for d=1:D
			% DISTURBANCE MECHANISM
			if rand()<delta X(i,d) = lowerB(d)+rand()*(upperB(d)-lowerB(d)); continue; end

			% UPDATE EQUATION
			X(i,d) = X(bNeighbour,d) + rand()*( X(s,d)- X(i,d) );

			% OUT OF BOUND CONTROL
			if or( X(i,d) < lowerB(d), X(i,d) > upperB(d) )
				X(i,d) = lowerB(d)+rand()*(upperB(d)-lowerB(d));
			end
		end
	end
end

% EVALUATE FITNESS OF THE FLIES AND FIND THE BEST
fitness = f(X);
[sFitness, s] = min(fitness);

disp( ['Final best fitness=', num2str(sFitness)] ) 
