///*
//* 도큐먼트 참고
//https://tfussell.gitbooks.io/xlnt/content/
//http://open-source-parsers.github.io/jsoncpp-docs/doxygen/index.html
//*/
//
///*
//테스트용 엑셀에 대한 설명
//
//테스트용 엑셀의 규칙은 다음과 같다.
//
//1. #이 붙은 쎌은 주석이다.
//2. 3번째줄도 기획자가 읽기 위한 주석이다.
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
///* CellDataTypes에 들어가있는 자료형에 따라 현재 쎌에서 데이터를 추출한다. */
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
//	/* 엑셀 파일을 로드 한다. */
//	/* TODO: 폴더내에 있는 모든 파일을 읽어오도록 변경 */
//	xlnt::workbook wb;
//	wb.load("../Data/SwordWoman.xlsx");
//
//	/* 엑셀 파일안에 있는 0번째 시트를 사용한다. */
//	/* TODO: 모든 시트를 사용하게끔 변경 */
//	auto ws = wb.sheet_by_index(0);
//	std::cout << "Processing spread sheet" << std::endl;
//
//	Json::Value Root;
//
//	int RowCount = 0;
//	/* 모든 쎌을 순회하면서 출력한다. */
//	for (auto row : ws.rows(true))
//	{
//		/* 데이터 테이블 규칙에 따라 무시해야하는 행 */
//		if (RowCount == RowType::Comment || RowCount == RowType::Empty) { RowCount++; continue; }
//
//		Json::Value Data;
//
//		int CellCount = 0;
//		for (auto cell : row)
//		{
//			/* 앞에 #이 붙은 쎌은 말그대로 주석이므로 읽을 필요가 없다. */
//			if (cell.to_string()[0] == '#')
//			{
//				continue;
//			}
//
//			/* 현재 읽는 행에 따라 행동이 달라진다.*/
//			switch (RowCount)
//			{
//				/* 0번째 행은 데이터 타입을 저장한다. */
//			case DataType:
//				CellDataTypes[CellCount] = cell.value<std::string>();
//				break;
//				/* 3번째 행은 변수 이름을 저장한다. */
//			case Name:
//				CellNames[CellCount] = cell.value<std::string>();
//				break;
//				/* 나머지는 데이터를 의미한다. */
//			default:
//				CreateJsonValue(Data, cell, CellCount);
//				break;
//			}
//
//			CellCount++;
//		}
//
//		/* 데이터를 Root에 붙힌다. */
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
//	/* 가공한 Root(JsonData)을 파일로 저장한다. */
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