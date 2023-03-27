///*
//https://tfussell.gitbooks.io/xlnt/content/ 도큐먼트 참고
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
//
//int main()
//{
//	/* 엑셀 파일을 로드 한다. */
//	xlnt::workbook wb;
//	wb.load("../Data/SwordWoman.xlsx");
//	
//	/* 엑셀 파일안에 있는 0번째 시트를 사용한다. */
//	auto ws = wb.sheet_by_index(0);
//	std::cout << "Processing spread sheet" << std::endl;
//
//	/* 테스트용 엑셀에서는 3번째 줄 내용이 필요가 없으므로 읽지 않게끔 처리하기 위한 변수 */
//	int ignoreRow = 2;
//	int count = 0;
//
//	/* 모든 쎌을 순회하면서 출력한다. */
//	for (auto row : ws.rows())
//	{
//		/* 3번째 내용은 필요가 없으므로 Continue */
//		if (ignoreRow == count)
//		{
//			count++;
//			continue;
//		}
//
//		for (auto cell : row)
//		{
//			/* 앞에 #이 붙은 쎌은 말그대로 주석이므로 읽을 필요가 없다. */
//			if (cell.to_string()[0] == '#')
//			{
//				continue;
//			}
//			std::cout << cell.to_string() << std::endl;
//		}
//		std::cout << "──────────" << std::endl << std::endl;
//		count++;
//	}
//	std::cout << "Processing complete" << std::endl;
//	return 0;
//}