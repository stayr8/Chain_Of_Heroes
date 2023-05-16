#include "Parser.h"

#include <iostream>

int main()
{
	Parser parser;
	parser.Initialize();

	parser.Parse(ParseMode::Json);
	parser.Parse(ParseMode::GenerateCode);

	system("pause");

	return 0;
}