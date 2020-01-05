CREATE TABLE IF NOT EXISTS Person(
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);
CREATE TABLE IF NOT EXISTS Custom_name(
Custom_name integer,
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);
CREATE TABLE IF NOT EXISTS Class(
Id serial,
Name text,
Teacher integer
);
CREATE TABLE IF NOT EXISTS Course(
OtherNameThanId serial,
Active boolean,
Name text,
Teacher integer
);
CREATE TABLE IF NOT EXISTS Student(
Class integer,
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);

CREATE TABLE IF NOT EXISTS student_course(
fk_person_id integer,
fk_course_id integer
);

ALTER TABLE student_course ADD PRIMARY KEY (fk_course_id, fk_person_id);
ALTER TABLE Person ADD PRIMARY KEY (Id);
ALTER TABLE Custom_name ADD PRIMARY KEY (Id);
ALTER TABLE Custom_name ADD CONSTRAINT fk_Person_Id FOREIGN KEY (Id) REFERENCES Person (Id);
ALTER TABLE Class ADD PRIMARY KEY (Id);
ALTER TABLE Class ADD CONSTRAINT fk_Custom_name_Id FOREIGN KEY (Teacher) REFERENCES Custom_name (Id);
ALTER TABLE Course ADD PRIMARY KEY (OtherNameThanId);
ALTER TABLE Course ADD CONSTRAINT fk_Custom_name_Id FOREIGN KEY (Teacher) REFERENCES Custom_name (Id);
ALTER TABLE Student ADD CONSTRAINT fk_Class_Id FOREIGN KEY (Class) REFERENCES Class (Id);
ALTER TABLE Student ADD PRIMARY KEY (Id);
ALTER TABLE Student ADD CONSTRAINT fk_Person_Id FOREIGN KEY (Id) REFERENCES Person (Id);


CREATE TABLE IF NOT EXISTS Person(
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);
CREATE TABLE IF NOT EXISTS Custom_name(
Custom_name integer,
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);
CREATE TABLE IF NOT EXISTS Class(
Id serial,
Name text,
Teacher integer
);
CREATE TABLE IF NOT EXISTS Course(
OtherNameThanId serial,
Active boolean,
Name text,
Teacher integer
);
CREATE TABLE IF NOT EXISTS Student(
Class integer,
Id serial,
Name text,
FirstName text,
Gender text,
BDay timestamp
);

CREATE TABLE IF NOT EXISTS student_course(
fk_person_id integer,
fk_course_id integer
);

ALTER TABLE student_course ADD PRIMARY KEY (fk_course_id, fk_person_id);
ALTER TABLE Person ADD PRIMARY KEY (Id);
ALTER TABLE Custom_name ADD PRIMARY KEY (Id);
ALTER TABLE Custom_name ADD CONSTRAINT fk_Person_Id FOREIGN KEY (Id) REFERENCES Person (Id);
ALTER TABLE Class ADD PRIMARY KEY (Id);
ALTER TABLE Class ADD CONSTRAINT fk_Custom_name_Id FOREIGN KEY (Teacher) REFERENCES Custom_name (Id);
ALTER TABLE Course ADD PRIMARY KEY (OtherNameThanId);
ALTER TABLE Course ADD CONSTRAINT fk_Custom_name_Id FOREIGN KEY (Teacher) REFERENCES Custom_name (Id);
ALTER TABLE Student ADD CONSTRAINT fk_Class_Id FOREIGN KEY (Class) REFERENCES Class (Id);
ALTER TABLE Student ADD PRIMARY KEY (Id);
ALTER TABLE Student ADD CONSTRAINT fk_Person_Id FOREIGN KEY (Id) REFERENCES Person (Id);