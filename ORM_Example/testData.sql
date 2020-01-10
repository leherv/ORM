INSERT INTO person(name, firstname, gender, bday)
VALUES
('test_name', 'test_firstname', 'MALE', '2000-01-01');
('test_name2', 'test_firstname2', 'FEMALE', '2002-02-02');


INSERT INTO custom_name(custom_name, id)
VALUES
(2500, 1);

INSERT INTO class(name, teacher) VALUES ('1A', 1);     

INSERT INTO course(active, name, teacher)
VALUES
(TRUE, 'best course', 1);


SELECT custom_name, id FROM person p1 JOIN custom_name c2 ON p1.id = c2.id;

SELECT * FROM course c1 WHERE c1.teacher = 1;