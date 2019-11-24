All fields have to be public properties with get/set
[Ignore] can be used to ignore properties that should not be present in the DB
[PK] has to be set on at least one entity (long) or one property has to be named Id


if the Property is a primary key by name or by attribute and the class is a subclass of another entity then 
the primary key automatically is a foreign key too

OneToMany and ManyToMany have to be bidirectional at the moment



was ich sehr cool finde:
ich lasse mir vom user ein IDBConnection übergeben dadurch programmiere ich die ganze Zeit gegen
generische Interfaces und kann so den TypeMapper benutzen um alle Datenbanken nutzen zu können
ich habe einen TypeMapper definiert der mir vom user zur verfügung gestellt werden muss,
der somit ermöglicht neue Datenbanken zu unterstützen

Ich habe eine DBContext Klasse, die der einzige Eintrittspunkt für den User ist (Konfiguration)
Komplexe Reflection angewandt um im DBContext Kontrolle über Entities des Users zu gewinnen

Einfaches Querying komplett ohne SQL oder JPQL-Verschnitte