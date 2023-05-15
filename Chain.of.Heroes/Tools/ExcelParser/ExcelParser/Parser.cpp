#include "Parser.h"
#include <fstream>

std::string path = 
"../../../Assets/00. Dev/Junha/gameScenes/test/parse/Resources/";

const std::string& pathAndfileName =
"../Data/TextData.xlsx";

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
	/* ���� ������ �ε� �Ѵ�. */
	/* TODO: �������� �ִ� ��� ������ �о������ ���� */
	xlnt::workbook wb;

	//wb.load("F:\ExcelParser\Data\Test.xlsx");
	wb.load(pathAndfileName);

	/* ���� ���Ͼȿ� �ִ� 0��° ��Ʈ�� ����Ѵ�. */
	/* TODO: ��� ��Ʈ�� ����ϰԲ� ���� */
	_workSheet = wb.sheet_by_index(0);
	_workSheetTitle = _workSheet.title();

	int RowCount = 0;
	/* ��� ���� ��ȸ�ϸ鼭 ����Ѵ�. */
	for (auto row : _workSheet.rows(true))
	{
		/* ������ ���̺� ��Ģ�� ���� �����ؾ��ϴ� �� */
		if (RowCount == RowType::Comment || RowCount == RowType::Empty) { RowCount++; continue; }

		Json::Value Data;

		int CellCount = 0;
		for (auto cell : row)
		{
			/* �տ� #�� ���� ���� ���״�� �ּ��̹Ƿ� ���� �ʿ䰡 ����. */
			if (cell.to_string()[0] == '#')
			{
				continue;
			}

			/* ���� �д� �࿡ ���� �ൿ�� �޶�����.*/
			switch (RowCount)
			{
				/* 0��° ���� ������ Ÿ���� �����Ѵ�. */
			case DataType:
				CellDataTypes[CellCount] = cell.value<std::string>();
				break;
				/* 3��° ���� ���� �̸��� �����Ѵ�. */
			case Name:
				CellNames[CellCount] = cell.value<std::string>();
				break;
				/* �������� �����͸� �ǹ��Ѵ�. */
			default:
				CreateJsonValue(Data, cell, CellCount);
				break;
			}

			CellCount++;
		}

		/* �����͸� Root�� ������. */
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
	/* TODO : �� ���� �ڷ��� �߰� */
	std::string Type = CellDataTypes[CellCount];
	if (Type == "Int")
	{
		Data[CellNames[CellCount]] = Cell.value<int>();
	}
	else if (Type == "String")
	{
		Data[CellNames[CellCount]] = Cell.value<std::string>();
	}
	else if (Type == "Float")
	{
		Data[CellNames[CellCount]] = Cell.value<double>();
	}
}

void Parser::CreateJsonFile()
{
	/* ������ Root(JsonData)�� ���Ϸ� �����Ѵ�. */
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
		std::cout << "������ ���̺� ����������" << std::endl;
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