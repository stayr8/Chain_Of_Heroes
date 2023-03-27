///*
//* ��ť��Ʈ ����
//https://tfussell.gitbooks.io/xlnt/content/
//http://open-source-parsers.github.io/jsoncpp-docs/doxygen/index.html
//*/
//
///*
//�׽�Ʈ�� ������ ���� ����
//
//�׽�Ʈ�� ������ ��Ģ�� ������ ����.
//
//1. #�� ���� ���� �ּ��̴�.
//2. 3��°�ٵ� ��ȹ�ڰ� �б� ���� �ּ��̴�.
//*/
//
//#include <iostream>
//#include <xlnt/xlnt.hpp>
//#include <json/json.h>
//#include <unordered_map>
//#include <fstream>
//
//enum RowType
//{
//	DataType = 0,
//	Empty = 1,
//	Comment = 2,
//	Name = 3,
//};
//
//std::map<int, std::string> CellDataTypes;
//std::map<int, std::string> CellNames;
//
///* CellDataTypes�� ���ִ� �ڷ����� ���� ���� �쿡�� �����͸� �����Ѵ�. */
//void CreateJsonValue(Json::Value& Data, xlnt::cell Cell, int CellCount)
//{
//	std::string Type = CellDataTypes[CellCount];
//	if (Type == "Int")
//	{
//		Data[CellNames[CellCount]] = Cell.value<int>();
//	}
//	else if (Type == "Float")
//	{
//		Data[CellNames[CellCount]] = Cell.value<float>();
//	}
//	else if (Type == "String" || Type == "Enum")
//	{
//		Data[CellNames[CellCount]] = Cell.value<std::string>();
//	}
//}
//
//int main()
//{
//	/* ���� ������ �ε� �Ѵ�. */
//	/* TODO: �������� �ִ� ��� ������ �о������ ���� */
//	xlnt::workbook wb;
//	wb.load("../Data/SwordWoman.xlsx");
//
//	/* ���� ���Ͼȿ� �ִ� 0��° ��Ʈ�� ����Ѵ�. */
//	/* TODO: ��� ��Ʈ�� ����ϰԲ� ���� */
//	auto ws = wb.sheet_by_index(0);
//	std::cout << "Processing spread sheet" << std::endl;
//
//	Json::Value Root;
//
//	int RowCount = 0;
//	/* ��� ���� ��ȸ�ϸ鼭 ����Ѵ�. */
//	for (auto row : ws.rows(true))
//	{
//		/* ������ ���̺� ��Ģ�� ���� �����ؾ��ϴ� �� */
//		if (RowCount == RowType::Comment || RowCount == RowType::Empty) { RowCount++; continue; }
//
//		Json::Value Data;
//
//		int CellCount = 0;
//		for (auto cell : row)
//		{
//			/* �տ� #�� ���� ���� ���״�� �ּ��̹Ƿ� ���� �ʿ䰡 ����. */
//			if (cell.to_string()[0] == '#')
//			{
//				continue;
//			}
//
//			/* ���� �д� �࿡ ���� �ൿ�� �޶�����.*/
//			switch (RowCount)
//			{
//				/* 0��° ���� ������ Ÿ���� �����Ѵ�. */
//			case DataType:
//				CellDataTypes[CellCount] = cell.value<std::string>();
//				break;
//				/* 3��° ���� ���� �̸��� �����Ѵ�. */
//			case Name:
//				CellNames[CellCount] = cell.value<std::string>();
//				break;
//				/* �������� �����͸� �ǹ��Ѵ�. */
//			default:
//				CreateJsonValue(Data, cell, CellCount);
//				break;
//			}
//
//			CellCount++;
//		}
//
//		/* �����͸� Root�� ������. */
//		if (Data.isNull() == false)
//		{
//			Root.append(Data);
//		}
//
//		RowCount++;
//	}
//
//	std::cout << "Processing complete" << std::endl;
//
//	/* ������ Root(JsonData)�� ���Ϸ� �����Ѵ�. */
//	Json::StreamWriterBuilder Writer;
//	std::string JsonString = Json::writeString(Writer, Root);
//
//	std::string path = "Output/";
//	std::ofstream file;
//	file.open(path + ws.title() + ".json");
//	if (file.is_open())
//	{
//		file.write(JsonString.c_str(), JsonString.size());
//	}
//	else
//	{
//		file.close();
//		std::cout << "ERROR: Failed open file: " << path + ws.title() + ".json" << std::endl;
//	}
//	file.close();
//
//	system("pause");
//	return 0;
//}