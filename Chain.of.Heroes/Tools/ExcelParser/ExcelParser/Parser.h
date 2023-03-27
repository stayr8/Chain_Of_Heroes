#pragma once
#include <xlnt/xlnt.hpp>
#include <json/json.h>
#include <unordered_map>

enum class ParseMode
{
	Json,
	GenerateCode
};

enum RowType
{
	DataType = 0,
	Empty = 1,
	Comment = 2,
	Name = 3,
};

class Parser
{
public:
	Parser() = default;
	~Parser() = default;

public:
	void Initialize();
	void Parse(ParseMode mode);

private:
	void JsonParse();
	void CreateJsonValue(Json::Value& Data, xlnt::cell Cell, int CellCount);
	void CreateJsonFile();

private:
	void CodeGenerate();
	std::string GetDataTypeString(std::string type);
	std::string GetDataTypeAs(std::string type);

private:
	xlnt::worksheet _workSheet;
	Json::Value Root;

	std::string _workSheetTitle;

private:
	std::map<int, std::string> CellDataTypes;
	std::map<int, std::string> CellNames;
};