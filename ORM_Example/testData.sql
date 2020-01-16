INSERT INTO person(name, firstname, gender, bday) VALUES
('test_name', 'test_firstname', 'MALE', '2000-01-01'),
('test_name2', 'test_firstname2', 'FEMALE', '2002-02-02'),
('test_name3', 'test_firstname3', 'MALE', '2003-03-03');


INSERT INTO custom_name(custom_name, id)
VALUES
(2500, (SELECT id FROM person WHERE name = 'test_name')),
(2600, (SELECT id FROM person WHERE name = 'test_name2'));

INSERT INTO class(name, teacher) VALUES ('1A', (SELECT id FROM custom_name WHERE custom_name = 2500));     

INSERT INTO student(class, id) VALUES ((SELECT id FROM class WHERE name = '1A'), (SELECT id FROM person WHERE name = 'test_name3'));

INSERT INTO course(active, name, teacher)
VALUES
(TRUE, 'best course', (SELECT id FROM custom_name WHERE custom_name = 2500));

INSERT INTO student_course(fk_person_id, fk_course_id)
VALUES
((SELECT id FROM person WHERE name = 'test_name3'), (SELECT othernamethanid FROM course WHERE name = 'best course'));

-- normal select with inheritance
SELECT custom_name, id FROM person p1 JOIN custom_name c2 ON p1.id = c2.id;

-- selects for onetomany
SELECT * FROM course c1 WHERE c1.teacher = (SELECT id FROM custom_name WHERE custom_name = 2500);
SELECT * FROM class c2 WHERE c2.teacher = (SELECT id FROM custom_name WHERE custom_name = 2500);

-- selects for manytoone
SELECT * FROM custom_name c1 WHERE c1.id = (SELECT teacher FROM class WHERE name = '1A');  

-- selects for manytomany 

SELECT s5.class, s5.id, p1.id, p1.name, p1.firstname, p1.gender, p1.bday FROM student s5 
JOIN person p1 ON s5.id = p1.id
JOIN student_course s2 ON s5.id = s2.fk_person_id
JOIN course c3 ON s2.fk_course_id = c3.othernamethanid

-- insert with inheritance
WITH AJSF AS (
INSERT INTO person (name, firstname, gender, bday) 
VALUES 
('test_name', 'test_firstname', 'MALE', '2000-01-01'),
('test_name2', 'test_firstname2', 'FEMALE', '2002-02-02') 
RETURNING id
)
INSERT INTO custom_name (custom_name, id) 
VALUES 
(2500, (SELECT * FROM AJSF LIMIT 1 OFFSET 0)),
(2600, (SELECT * FROM AJSF LIMIT 1 OFFSET 1))
RETURNING id
 