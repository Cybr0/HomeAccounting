����� �� ������ ������� ��������� �� �������������.
���� ���� 3 ������� �������(Students), �������������(Teachers) � �����(Books), ����� ���� ��������� ������.

select * from Students as s
left join Teachers as t
on s.teacherId = t.id
left join Books as b
on s.bookId = b.id
where b.name like '%�������� �����%' and t.name like '%���%';