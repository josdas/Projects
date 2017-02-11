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

vector<function<double(const point &)> > fun = 
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

string Num[11] = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

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

function<point(const point&)> getFunction(pair<vector<double>, vector<double> > pa) {
	return [&, pa](const point& p) {
		double ansX = 0;
		for (size_t i = 0; i < pa.first.size(); ++i) {
			double w = pa.first[i];
			ansX += w * fun[i](p);
		}
		double ansY = 0;
		for (size_t i = 0; i < pa.second.size(); ++i) {
			double w = pa.second[i];
			ansY += w * fun[i](p);
		}
		return point(ansX, ansY);
	};
}

double xInput, yInput, xOut, yOut;

pair<vector<double>, vector<double> > readCoef(int number){
	string NameIn = "coefficients_" + Num[number] + ".dat";
	ifstream in(NameIn.c_str());

	in >> xInput >> yInput >> xOut >> yOut;

	pair<vector<double>, vector<double> > T;
	while (!in.eof()){
		double xi, yi;
		if(in >> xi >> yi){
			T.first.push_back(xi);
			T.second.push_back(yi);
		}
	}
	in.close();
	return T;
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

void writeResult(const vector<point> &T, int number){
	string Name = "model_" + Num[number] +"_" + "1" + "_test" + ".dat";
	ofstream out(Name.c_str(), std::ofstream::out);
	for (size_t i = 0; i < T.size(); i++){
		out.precision(10);
		out << fixed << T[i].x << ' ' << T[i].y << '\n';
	}
	out.close();
}


void solve(int test){
	auto K = readCoef(test);
	auto Test = readTest(test);
	auto F = getFunction(K);
	vector<point> result;
	for(size_t i = 0; i < Test.size(); i++){
		auto t = normalizeIn(Test[i]);
		auto x = unNormalizeOut(F(t));
		result.push_back(x);
	}
	writeResult(result, test);
}

int main() {
	solve(3);
}