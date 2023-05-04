#include "Parser.h"
#include <fstream>

std::string path = 
"../../../Assets/00. Dev/Junha/gameScenes/test/parse/Resources/";

const std::string& pathAndfileName =
"../Data/NormalMonster_GoblinWarrior.xlsx";

void Parser::Initialize() {}

void Parser::Parse(ParseMode mode)
{
	switch (mode)
	{
	case ParseMode::Json:
		JsonParse();
		break;
	case ParseMode::GenerateCode:
		CodeGenerate();
		break;
	default:
		break;
	}
}

void Parser::JsonParse()
{
	/* 엑셀 파일을 로드 한다. */
	/* TODO: 폴더내에 있는 모든 파일을 읽어오도록 변경 */
	xlnt::workbook wb;

	//wb.load("F:\ExcelParser\Data\Test.xlsx");
	wb.load(pathAndfileName);

	/* 엑셀 파일안에 있는 0번째 시트를 사용한다. */
	/* TODO: 모든 시트를 사용하게끔 변경 */
	_workSheet = wb.sheet_by_index(0);
	_workSheetTitle = _workSheet.title();

	int RowCount = 0;
	/* 모든 쎌을 순회하면서 출력한다. */
	for (auto row : _workSheet.rows(true))
	{
		/* 데이터 테이블 규칙에 따라 무시해야하는 행 */
		if (RowCount == RowType::Comment || RowCount == RowType::Empty) { RowCount++; continue; }

		Json::Value Data;

		int CellCount = 0;
		for (auto cell : row)
		{
			/* 앞에 #이 붙은 쎌은 말그대로 주석이므로 읽을 필요가 없다. */
			if (cell.to_string()[0] == '#')
			{
				continue;
			}

			/* 현재 읽는 행에 따라 행동이 달라진다.*/
			switch (RowCount)
			{
				/* 0번째 행은 데이터 타입을 저장한다. */
			case DataType:
				CellDataTypes[CellCount] = cell.value<std::string>();
				break;
				/* 3번째 행은 변수 이름을 저장한다. */
			case Name:
				CellNames[CellCount] = cell.value<std::string>();
				break;
				/* 나머지는 데이터를 의미한다. */
			default:
				CreateJsonValue(Data, cell, CellCount);
				break;
			}

			CellCount++;
		}

		/* 데이터를 Root에 붙힌다. */
		if (Data.isNull() == false)
		{
			Root.append(Data);
		}

		RowCount++;
	}

	CreateJsonFile();
}

void Parser::CreateJsonValue(Json::Value& Data, xlnt::cell Cell, int CellCount)
{
	/* TODO : 더 많은 자료형 추가 */
	std::string Type = CellDataTypes[CellCount];
	if (Type == "Int")
	{
		Data[CellNames[CellCount]] = Cell.value<int>();
	}
	else if (Type == "String")
	{
		Data[CellNames[CellCount]] = Cell.value<std::string>();
	}
	else if (Type == "Float" || Type == "Enum")
	{
		Data[CellNames[CellCount]] = Cell.value<double>();
	}
}

void Parser::CreateJsonFile()
{
	/* 가공한 Root(JsonData)을 파일로 저장한다. */
	Json::StreamWriterBuilder Writer;
	std::string JsonString = Json::writeString(Writer, Root);

	//std::string path = "Output/";
	//std::string path = "../../../Assets/00. Dev/Junha/gameScenes/test/parse/Output/Resources/";
	std::ofstream file;
	file.open(path + _workSheetTitle + ".json");
	if (file.is_open())
	{
		file.write(JsonString.c_str(), JsonString.size());
	}
	else
	{
		file.close();
		std::cout << "ERROR: Failed open file: " << path + _workSheetTitle + ".json" << std::endl;
	}
	file.close();
}

void Parser::CodeGenerate()
{
	std::string code = "";

	code +=
		"using System.Collections.Generic;\n"
		"using UnityEngine;\n"
		"using SimpleJSON;\n\n";

	if (CellDataTypes.size() != CellNames.size())
	{
		std::cout << "데이터 테이블에 문제가있음" << std::endl;
	}

	code += "public class " + _workSheetTitle + "\n";
	code += "{\n";

	for (int i = 0; i < CellDataTypes.size(); i++)
	{
		code += "\tpublic " + GetDataTypeString(CellDataTypes[i]) + " " + CellNames[i] + ";\n";
	}

	code +=
		"\tpublic bool Parse(SimpleJSON.JSONNode Data)\n"
		"\t{\n";

	for (int i = 0; i < CellDataTypes.size(); i++)
	{
		code += "\t\t" + CellNames[i] + " = Data[\"" + CellNames[i] + "\"]" + GetDataTypeAs(CellDataTypes[i]) + "\n";
	}

	code +=
		"\t\treturn true;\n"
		"\t}\n";

	code += "}";


	//std::string path = "Output/";
	//std::string path = "../../../Assets/00. Dev/Junha/gameScenes/test/parse/Output/Resources/";
	std::ofstream file;
	file.open(path + "Data.cs");
	if (file.is_open())
	{
		file.write(code.c_str(), code.size());
	}
	else
	{
		file.close();
		std::cout << "ERROR: Failed open file: " << path + _workSheet.title() + ".json" << std::endl;
	}
	file.close();
}

#pragma region GetDataType

std::string Parser::GetDataTypeString(std::string type)
{
	if (type == "Int")
	{
		return "int";
	}
	else if (type == "String")
	{
		return "string";
	}
	else if (type == "Float")
	{
		return "float";
	}
}

std::string Parser::GetDataTypeAs(std::string type)
{
	if (type == "Int")
	{
		return ".AsInt;";
	}
	else if (type == "String")
	{
		return ";";
	}
	else if (type == "Float")
	{
		return ".AsFloat;";
	}
}

#pragma endregion