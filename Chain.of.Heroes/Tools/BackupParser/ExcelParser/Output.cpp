///*
//https://tfussell.gitbooks.io/xlnt/content/ ��ť��Ʈ ����
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
//
//int main()
//{
//	/* ���� ������ �ε� �Ѵ�. */
//	xlnt::workbook wb;
//	wb.load("../Data/SwordWoman.xlsx");
//	
//	/* ���� ���Ͼȿ� �ִ� 0��° ��Ʈ�� ����Ѵ�. */
//	auto ws = wb.sheet_by_index(0);
//	std::cout << "Processing spread sheet" << std::endl;
//
//	/* �׽�Ʈ�� ���������� 3��° �� ������ �ʿ䰡 �����Ƿ� ���� �ʰԲ� ó���ϱ� ���� ���� */
//	int ignoreRow = 2;
//	int count = 0;
//
//	/* ��� ���� ��ȸ�ϸ鼭 ����Ѵ�. */
//	for (auto row : ws.rows())
//	{
//		/* 3��° ������ �ʿ䰡 �����Ƿ� Continue */
//		if (ignoreRow == count)
//		{
//			count++;
//			continue;
//		}
//
//		for (auto cell : row)
//		{
//			/* �տ� #�� ���� ���� ���״�� �ּ��̹Ƿ� ���� �ʿ䰡 ����. */
//			if (cell.to_string()[0] == '#')
//			{
//				continue;
//			}
//			std::cout << cell.to_string() << std::endl;
//		}
//		std::cout << "��������������������" << std::endl << std::endl;
//		count++;
//	}
//	std::cout << "Processing complete" << std::endl;
//	return 0;
//}