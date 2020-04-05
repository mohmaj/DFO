function [sum]=f(X)
	[N,D]=size(X);
	sum=zeros(N,1);
	for i=1:N
		for d=1:D
			sum(i,1)=sum(i,1)+X(i,d)^2;
		end
	end
end
