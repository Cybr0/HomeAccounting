Ответ на вопрос который прозвучал на собеседовании.
Было дано 3 таблицы Ученики(Students), Преподаватели(Teachers) и Книги(Books), нужно было составить запрос.

select * from Students as s
left join Teachers as t
on s.teacherId = t.id
left join Books as b
on s.bookId = b.id
where b.name like '%название книги%' and t.name like '%Имя%';