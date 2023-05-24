using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ProjectSINGWI_TableParsing
{
    static class ExcelManager
    {
        //static List<string> EnumCheckList = new List<string>();
        //static Dictionary<string, string> EnumDictionary = new Dictionary<string, string>();
        static Excel.Application ExcelApp = null;

        public static void Generate(/*List<string> TypeList,*/ List<string> DataList, string JsonPath, string HeaderPath)
        {
            try
            {
                ExcelApp = new Excel.Application();

                //GenerateType(TypeList, HeaderPath);
                GenerateData(DataList, JsonPath, HeaderPath);

                ExcelApp.Quit();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                Release(ExcelApp);
            }
        }

        public static void GenerateType(List<string> TypeList, string HeaderPath)
        {
            foreach (var Type in TypeList)
            {
                Excel.Workbook TypeWorkbook = ExcelApp.Workbooks.Open(Type);
                Console.WriteLine(TypeWorkbook.FullName);

                Excel.Worksheet TypeWorkSheet = TypeWorkbook.Sheets.Item[1];
                if (null == TypeWorkSheet)
                {
                    throw new Exception("Sheet를 발견하지 못했습니다.");
                }

                Excel.Range Range = TypeWorkSheet.UsedRange;
                if (null == Range)
                {
                    throw new Exception("UsedRange is null of " + TypeWorkSheet.Name);
                }

                var Types = GetTypes(Range);
                if (0 >= Types.Count)
                {
                    throw new Exception("타입 추정에 실패하였습니다.");
                }

                foreach (var _Type in Types)
                {
                    string[] Name = _Type.Key.Split("_");

                    if (2 > Name.Length)
                    {
                        throw new Exception("Type의 이름 형식이 잘못됐습니다. '_'로 구분할 수 없습니다. : " + _Type.Key);
                    }

                    //if (Name[0].ToLower().Equals("enum"))
                    //{
                    //    EnumCheckList.Add(Name[1]);

                    //    CreateEnumCode(Name[1], _Type.Value, HeaderPath);
                    //}
                    //else
                    //{
                    //    throw new Exception("Type의 이름 형식이 잘못됐습니다.");
                    //}

                }
            }
        }

        public static void GenerateData(List<string> DataList, string JsonPath, string HeaderPath)
        {
            foreach (var Data in DataList)
            {
                Excel.Workbook Workbook = ExcelApp.Workbooks.Open(Data);

                Console.WriteLine(Workbook.FullName);

                foreach (Excel.Worksheet WorkSheet in Workbook.Worksheets)
                {
                    Excel.Range Range = WorkSheet.UsedRange;
                    if(null == Range)
                    {
                        throw new Exception("UsedRange is null of " + WorkSheet.Name);
                    }

                    Console.Write(WorkSheet.Name);
                    Console.Write("\tColumns: " + Range.Columns.Count.ToString());
                    Console.Write("\tRows: " + Range.Rows.Count.ToString());
                    Console.WriteLine();

                    Dictionary<string, string> Variables = GetVariables(Range);
                    DataTable Table = GetDatas(Variables, Range);

                    string Path = JsonPath + WorkSheet.Name + ".json";
                    string json = DataTableToJSONWithStringBuilder(Table);
                    //string json = Table.ToJson().ToString();
                    System.IO.File.WriteAllText(Path, json, Encoding.UTF8);

                    CreateCode(WorkSheet.Name, Variables, HeaderPath);
                }
            }
        }
        public static string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[\n");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{\n");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\t\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",\n");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\t\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"\n");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("\n}\n");
                    }
                    else
                    {
                        JSONString.Append("\n},\n");
                    }
                }
                JSONString.Append("\n]");
            }
            return JSONString.ToString();
        }

        private static Dictionary<string, string> GetVariables(Excel.Range UsedRange)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            Result.Add("ID", "int");

            for (int column = 2; column <= UsedRange.Columns.Count; column++)
            {
                string VariableName = (UsedRange.Cells[4, column] as Excel.Range).Value2.ToString();
                string TypeName = (UsedRange.Cells[1, column] as Excel.Range).Value2.ToString();

                //if(VariableName.Contains("#"))
                //{
                //    continue;
                //}

                //if(TypeName.Contains("#"))
                //{
                //    continue;
                //}

                //string[] TypeCheck = TypeName.Split(",");

                Result.Add(VariableName, TypeName);

                //switch (TypeCheck[0].ToLower())
                //{
                //    case "enum":
                //        if (EnumCheckList.Contains(TypeCheck[1]))
                //        {
                //            string EnumName = "E" + TypeCheck[1];
                //            Result.Add(VariableName, EnumName);
                //        }
                //        else
                //        {
                //            throw new Exception("3번째 행, " + column.ToString() + "번째 열에서 오류 발견\n" + TypeCheck[1] + "의 이름을 가진 Enum이 존재하지않습니다.");
                //        }
                //        break;
                //    default:
                //        break;
                //}
            }

            return Result;
        }

        private static Dictionary<string, List<string>> GetTypes(Excel.Range UsedRange)
        {
            Dictionary<string, List<string>> Result = new Dictionary<string, List<string>>();

            for (int column = 2; column <= UsedRange.Columns.Count; column += 2)
            {
                List<string> Values = new List<string>();

                if ((UsedRange.Cells[1, column] as Excel.Range).Value2 == null)
                {
                    break;
                }

                string TypeName = (UsedRange.Cells[1, column] as Excel.Range).Value2.ToString();

                int rowCount = 2;
                while (true)
                {
                    if ((UsedRange.Cells[rowCount, column] as Excel.Range).Value2 == null)
                    {
                        break;
                    }

                    string TypeValue = (UsedRange.Cells[rowCount, column] as Excel.Range).Value2.ToString();

                    Values.Add(TypeValue);
                    rowCount++;
                }

                Result.Add(TypeName, Values);
            }

            return Result;
        }

        private static DataTable GetDatas(Dictionary<string, string> Variables, Excel.Range UsedRange)
        {
            DataTable Table = new DataTable();
            foreach (var Var in Variables)
            {
                if (Var.Key.Contains("#")) { continue; }
                if (Var.Value.Contains("#")) { continue; }

                Table.Columns.Add(new DataColumn(Var.Key, StringToType(Var.Value)));
            }

            for (int row = 5; row <= UsedRange.Rows.Count; row++)
            {
                DataRow Row = Table.NewRow();
                for (int column = 1; column <= UsedRange.Columns.Count; column++)
                { 
                    string Cell = (UsedRange.Cells[row, column] as Excel.Range).Value2.ToString();
                    if(Cell.Contains("#")) { continue; }

                    switch (Variables.Values.ElementAt(column - 1))
                    {
                        case "float":
                            float FValue;
                            if (float.TryParse(Cell, out FValue))
                            {
                                Row[Variables.Keys.ElementAt(column - 1)] = FValue;
                            }
                            else
                            {
                                Console.WriteLine("자료형이 float으로 설정됐지만 float으로 형변환을 실패했습니다. : " + row + "행" + column + "열" + FValue);
                            }
                            break;
                        case "int":
                            int IValue;
                            if (int.TryParse(Cell, out IValue))
                            {
                                Row[Variables.Keys.ElementAt(column - 1)] = IValue;
                            }
                            else
                            {
                                Console.WriteLine("자료형이 int로 설정됐지만 int로 형변환을 실패했습니다. : " + row + "행" + column + "열" + IValue);
                            }
                            break;
                        default:
                            Row[Variables.Keys.ElementAt(column - 1)] = Cell.ToString();
                            break;
                    }
                }
                Table.Rows.Add(Row);
            }

            return Table;
        }

        private static void CreateCode(string WorksheetName, Dictionary<string, string> Variables, string Path)
        {
            StringBuilder Code = new StringBuilder();

            Code.AppendLine("using System.Collections.Generic;");
            Code.AppendLine("using UnityEngine;");
            Code.AppendLine("using SimpleJSON;");
            Code.AppendLine("\n");

            //foreach(var Variable in Variables)
            //{
            //    string TypeValue = ConvertStringTypeToUE4(Variable.Value);

            //    if (EnumDictionary.ContainsKey(TypeValue.Substring(1)))
            //    {
            //        Code.AppendLine(EnumDictionary[TypeValue.Substring(1)]);
            //    }
            //}

            Code.AppendFormat("public class {0}", WorksheetName);
            Code.Append("\n");
            Code.AppendLine("{");

            foreach (var Variable in Variables)
            {
                if (Variable.Key.Contains("#")) { continue; }
                if (Variable.Value.Contains("#")) { continue; }

                string TypeValue = ConvertStringTypeToUE4(Variable.Value);
                //Code.AppendLine("\tUPROPERTY(EditAnywhere, BlueprintReadWrite)");
                Code.AppendFormat("\tpublic {0} {1};", TypeValue.ToLower(), Variable.Key);
                Code.Append("\n");
            }
            Code.Append("\n");

            Code.AppendLine("\tpublic bool Parse(SimpleJSON.JSONNode Data)");
            Code.AppendLine("\t{");
            foreach (var Variable in Variables)
            {
                if (Variable.Key.Contains("#")) { continue; }
                if (Variable.Value.Contains("#")) { continue; }

                string TypeValue = ConvertStringToSimpleJson(Variable.Value);
                //Code.AppendLine("\tUPROPERTY(EditAnywhere, BlueprintReadWrite)");
                Code.AppendFormat("\t\t{0} = Data[\"{1}\"]", Variable.Key, Variable.Key);
                if(TypeValue == "String")
                {
                    Code.Append(";");
                }
                else
                {
                    Code.AppendFormat(".{0};", TypeValue);
                }
                Code.Append("\n");
            }
            Code.AppendLine("\t\treturn true;");
            Code.AppendLine("\t}");

            Code.AppendLine("}");

            string OutputPath = Path + WorksheetName + ".cs";
            System.IO.File.WriteAllText(OutputPath, Code.ToString());
        }

        //private static void CreateEnumCode(string EnumName, List<string> EnumProperties, string Path)
        //{
        //    StringBuilder Code = new StringBuilder();

        //    Code.AppendLine("UENUM(BlueprintType)");
        //    Code.AppendFormat("enum class E{0} : uint8", EnumName);
        //    Code.Append("\n");
        //    Code.AppendLine("{");

        //    for (int i = 0; i < EnumProperties.Count - 1; i++)
        //    {
        //        Code.AppendFormat("\t{0},", EnumProperties[i]);
        //        Code.Append("\n");
        //    }

        //    Code.AppendLine("\t" + EnumProperties[EnumProperties.Count - 1]);
        //    Code.AppendLine("};");

        //    EnumDictionary.Add(EnumName, Code.ToString());
        //    //string OutputPath = Path + "Type_" + EnumName + ".h";
        //    //System.IO.File.WriteAllText(OutputPath, Code.ToString());
        //}

        private static string ConvertStringTypeToUE4(string Type)
        {
            //switch (Type.ToString())
            //{
            //    case "string":
            //        return "FString";
            //}

            return Type;
        }

        private static string ConvertStringToSimpleJson(string Type)
        {
            switch (Type.ToString())
            {
                case "String":
                    return Type;
                case "Int":
                    return "AsInt";
                case "int":
                    return "AsInt";
                case "Float":
                    return "AsFloat";
            }

            return Type;
        }

        private static Type StringToType(string TypeName)
        {
            switch (TypeName)
            {
                case "Float":
                    return typeof(float);
                case "Int":
                    return typeof(int);
                case "String":
                    return typeof(string);
                default:
                    return typeof(string);
            }
        }

        private static void Release(object Object)
        {
            try
            {
                if (null != Object)
                {
                    Marshal.ReleaseComObject(Object);
                    Object = null;
                }
            }
            catch (Exception Ex)
            {
                Object = null;
                throw Ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
