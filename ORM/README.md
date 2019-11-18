All fields have to be public properties with get/set
[Ignore] can be used to ignore properties that should not be present in the DB
[PK] has to be set on at least one entity (long) or one property has to be named Id


if the Property is a primary key by name or by attribute and the class is a subclass of another entity then 
the primary key automatically is a foreign key too