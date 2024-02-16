/* 
    Выполнено для диалекта SQL PostgreSQL
*/

/* 
    Query 1
    Сотрудника с максимальной заработной платой.
*/

SELECT *
FROM EMPLOYEE
ORDER BY "SALARY" DESC
LIMIT 1;

/*
    Query 2
    Вывести одно число: максимальную длину цепочки руководителей по
    таблице сотрудников (вычислить глубину дерева).
*/

WITH RECURSIVE tree_result AS (
    SELECT 1 AS "LEVEL", "CHIEF_ID", "ID"
    FROM employee
    WHERE "CHIEF_ID" IS NULL
    UNION ALL
    SELECT "LEVEL" + 1, employee."CHIEF_ID", employee."ID"
    FROM employee
             JOIN tree_result ON employee."CHIEF_ID" = tree_result."ID"
)
SELECT MAX("LEVEL")
FROM tree_result;

/*
    Query 3
    Отдел, с максимальной суммарной зарплатой сотрудников. 

*/

SELECT department."ID", department."NAME", SUM("SALARY") as "SALARY_SUM"
FROM department
         INNER JOIN employee ON department."ID" = employee."DEPARTMENT_ID"
GROUP BY department."ID", employee."DEPARTMENT_ID"
ORDER BY SUM("SALARY") DESC
LIMIT 1;

/*
    Query 4
    Сотрудника, чье имя начинается на «Р» и заканчивается на «н».
*/

SELECT *
FROM employee
WHERE "NAME" ILIKE 'Р%н'
LIMIT 1;