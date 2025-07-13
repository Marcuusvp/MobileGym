CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "EXERCICIOS" (
    "Id" uuid NOT NULL,
    "Nome" varchar(100) NOT NULL,
    "Series" integer NOT NULL,
    "Repeticoes" integer NOT NULL,
    "Carga" numeric NOT NULL,
    "Foto" varchar(255) NOT NULL,
    "Video" varchar(255) NOT NULL,
    CONSTRAINT "PK_EXERCICIOS" PRIMARY KEY ("Id")
);

CREATE TABLE "TREINOS" (
    "Id" uuid NOT NULL,
    "Nome" varchar(100) NOT NULL,
    "Usuario" varchar(50) NOT NULL,
    CONSTRAINT "PK_TREINOS" PRIMARY KEY ("Id")
);

CREATE TABLE "ExercicioTreino" (
    "ExerciciosId" uuid NOT NULL,
    "TreinoId" uuid NOT NULL,
    CONSTRAINT "PK_ExercicioTreino" PRIMARY KEY ("ExerciciosId", "TreinoId"),
    CONSTRAINT "FK_ExercicioTreino_EXERCICIOS_ExerciciosId" FOREIGN KEY ("ExerciciosId") REFERENCES "EXERCICIOS" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ExercicioTreino_TREINOS_TreinoId" FOREIGN KEY ("TreinoId") REFERENCES "TREINOS" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ExercicioTreino_TreinoId" ON "ExercicioTreino" ("TreinoId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250712174939_InitialCreate', '9.0.0');

COMMIT;

