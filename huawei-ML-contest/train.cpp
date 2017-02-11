#define _ijps 01
#define _CRT_SECURE_NO_DEPRECATE
#include <iostream>
#include <cmath>
#include <vector>
#include <cstdio>
#include <cstdlib>
#include <algorithm>
#include <string>
#include <fstream>
#include <assert.h> 
#include <functional>
using namespace std;

#define mk make_pair

struct __isoff{
	__isoff(){
		if (_ijps){
			freopen("input.txt", "r", stdin);
			freopen("output.txt", "w", stdout);
		}
		srand('C' + 'T' + 'A' + 'C' + 'Y' + 'M' + 'B' + 'A');
	}
	~__isoff(){
	}
} __osafwf;

const int infi = (int)1e9 + 7;

struct point{
	double x, y;
	point() : x(0), y(0){ }
	point(double x, double y) : x(x), y(y){	}
};

vector<double> gauss(vector<vector<double> > S, vector<double> A){
	int n = S.size();
	for (int i = 0; i < n; i++){
		double c = 0;
		int t = -1;
		for (int j = i; j < n; j++){
			if (c < abs(S[j][i])){
				c = abs(S[j][i]);
				t = j;
			}
		}
		assert(t >= 0);
		swap(S[t], S[i]);
		swap(A[t], A[i]);
		c = S[i][i];
		for (int j = 0; j < n; j++){
			S[i][j] /= c;
		}
		A[i] /= c;
		for (int j = 0; j < n; j++){
			if(j != i){
				double w = S[j][i];
				for (int k = 0; k < n; k++){
					S[j][k] -= w * S[i][k];
				}
				A[j] -= w * A[i];
			}
		}
	}
	return A;
}

vector<function<double(const point &)> > funX = 
{ 
	
	[](const point &x){ return sin(x.x + x.y); },
	[](const point &x){ return sin(x.x); },
	[](const point &x){ return sin(x.y); },
	[](const point &x){ return x.x * x.x; },
	[](const point &x){ return x.y * x.y; },
	[](const point &x){ return x.x * x.y; },
	[](const point &x){ return x.y * x.x * x.x; },
	[](const point &x){ return x.y * x.y * x.x; },
	[](const point &x){ return x.y * x.y * x.y; },
	[](const point &x){ return x.x * x.x * x.x; },
	[](const point &x){ return 1; },
	[](const point &x){ return x.x; },
	[](const point &x){ return x.y; } 
};

vector<function<double(const point &)> > funY = funX;

vector<double> train(const vector<pair<point, double>>& trainSet, vector<function<double(const point &)> > fun) {
	// a * x + b * y = z
	// sum {(a * x0 + b * y0 - z0) ^ 2} -> min
	// sum{2 * (a * xi + b * yi - zi) * xi} = 0 => a * sum{xi ^ 2} + b * sum{xi * yi} = sum {zi * xi}
	// a * sum{xi * yi} + b * sum{yi ^ 2} = sum {zi * yi}

	int N = fun.size();
	vector<vector<double> > matrix;
	vector<double> fr;
	for (int i = 0; i < N; ++i) {

		{
			double ans = 0;
			for (const auto& data : trainSet) {
				ans += data.second * fun[i](data.first);
			}
			fr.push_back(ans);
		}
		{
			vector<double> row;
			for (int j = 0; j < N; ++j) {
				double ans = 0;
				for (const auto& data : trainSet) {
					ans += fun[i](data.first) * fun[j](data.first);
				}
				row.push_back(ans);
			}
			matrix.push_back(row);
		}
	}
	auto T = gauss(matrix, fr);
	return T;
}

double evm(vector<point> trueAnswer, vector<point> out){
	double err = 0, sig = 0;
	for (size_t i = 0; i < trueAnswer.size(); i++){
		double dx = trueAnswer[i].x - out[i].x;
		double dy = trueAnswer[i].y - out[i].y;
		err += dx * dx + dy * dy;
		double x = trueAnswer[i].x;
		double y = trueAnswer[i].y;
		sig += x * x + y * y;
	}
	return 10 * log10(err / sig);
}

string Num[11] = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

vector<pair<point, point> > readAdap(int number){
	string NameIn = "signal_" + Num[number] + "_in_adap.dat";
	ifstream in(NameIn.c_str());
	string NameOut = "signal_" + Num[number] + "_out_adap.dat";
	ifstream out(NameOut.c_str());

	vector<pair<point, point> > T;
	while (!in.eof()){
		double xi, yi, xo, yo;
		if(in >> xi >> yi){
			if(out >> xo >> yo){
				T.push_back(mk(point(xi, yi), point(xo, yo)));
			}
		}
	}
	in.close();
	out.close();
	return T;
}

vector<point> readTest(int number){
	string NameIn = "signal_" + Num[number] + "_in_test.dat";
	ifstream in(NameIn.c_str());

	vector<point> T;
	while (!in.eof()){
		double xi, yi;
		if(in >> xi >> yi){
			T.push_back(point(xi, yi));
		}
	}
	in.close();
	return T;
}

double xInput, yInput, xOut, yOut;

void writeCoeff(const pair<vector<double>, vector<double> > &T, int number){
	string Name = "coefficients_" + Num[number] + ".dat";
	ofstream out(Name.c_str(), std::ofstream::out);
	out.precision(10);
	out << fixed << xInput << ' ' << yInput << '\n' << xOut << ' ' << yOut << '\n';
	for (size_t i = 0; i < T.first.size(); i++){
		out << fixed << T.first[i] << ' ' << T.second[i] << '\n';
	}
	out.close();
}


void printVector(const vector<double> &T, string s){
	cout << s + " = [";
	for (size_t i = 0; i < T.size() && i < 5000; i++){
		cout << T[i] << "; ";
	}
	cout << "];\n";
}

pair<vector<double>, vector<double> > trainPoint(const vector<pair<point, point> > &T){
	vector<pair<point, double> > pX(T.size()), pY(T.size());
	for (size_t i = 0; i < T.size(); i++){
		pX[i] = { T[i].first, T[i].second.x };
		pY[i] = { T[i].first, T[i].second.y };
	}
	vector<double> kX = train(pX, funX);
	vector<double> kY = train(pY, funY);
	return{ kX, kY };
}

function<point(const point&)> getFunction(pair<vector<double>, vector<double> > pa) {
	return [&, pa](const point& p) {
		double ansX = 0;
		for (size_t i = 0; i < pa.first.size(); ++i) {
			double w = pa.first[i];
			ansX += w * funX[i](p);
		}
		double ansY = 0;
		for (size_t i = 0; i < pa.second.size(); ++i) {
			double w = pa.second[i];
			ansY += w * funY[i](p);
		}
		return point(ansX, ansY);
	};
}


vector<pair<point, point> > normalize(vector<pair<point, point> > P){
	xInput = yInput = xOut = yOut = 0;
	for(size_t i = 0; i < P.size(); i++){
		xInput = max(xInput, abs(P[i].first.x));
		xOut = max(xOut, abs(P[i].second.x));
		yInput = max(yInput, abs(P[i].first.y));
		yOut = max(yOut, abs(P[i].second.y));
	}
	
	for(size_t i = 0; i < P.size(); i++){
		P[i].first.x /= xInput;
		P[i].first.y /= yInput;
		P[i].second.x /= xOut;
		P[i].second.y /= yOut;
	}
	return P;
}

point unNormalizeOut(point t){
	t.x *= xOut;
	t.y *= yOut;
	return t;
}

point normalizeIn(point t){
	t.x /= xInput;
	t.y /= yInput;
	return t;
}

double trainTest(int test, int num){
	auto T = readAdap(test);
	random_shuffle(T.begin(), T.end());
	vector<pair<point, point> > F(num);
	for (int i = 0; i < num; i++){
		F[i] = T[i];
	}
	F = normalize(F);
	auto K = trainPoint(F);
	
	writeCoeff(K, test);

	//vector<double> X, Y, Zmy, Z;
	vector<point> realAnswer, answer;
	for (size_t i = num; i < T.size(); i++){
		auto x = unNormalizeOut(getFunction(K)(normalizeIn(T[i].first)));
		answer.push_back(x);
		realAnswer.push_back(T[i].second);

		/*X.push_back(T[i].first.x);
		Y.push_back(T[i].first.y);
		Z.push_back(T[i].second.x - x.x);
		Zmy.push_back(x.x);*/
	}

	/*
	printVector(X, "X");
	printVector(Y, "Y");
	printVector(Z, "Z");
	
	printVector(X, "Xmy");
	printVector(Y, "Ymy");
	printVector(Zmy, "Zmy");

	for(int i = 0; i < funX.size(); i++){
		printf("{%0.4f %0.4f} ", K.first[i], K.second[i]);
	}
	printf("\n");*/

	return evm(realAnswer, answer);
}

int main() {
	printf("%0.10f\n", trainTest(1, 35000));
}