--Crear DB
CREATE DATABASE CDN;
GO

USE CDN;
GO


--Crear DB

-- Crear la tabla Roles
CREATE TABLE Roles (
    ID INT PRIMARY KEY IDENTITY,
    nombre_rol VARCHAR(50) UNIQUE
);

-- Insertar roles predeterminados
INSERT INTO Roles (nombre_rol) VALUES ('administrador'), ('salud'), ('psicologo'), ('trabajador_social'),('estudiante');

-- Crear la tabla Jovenes
CREATE TABLE Jovenes (
    ID INT PRIMARY KEY IDENTITY,
	cedula INT,
    nombre VARCHAR(100),
    edad INT,
    direccion VARCHAR(255),
    telefono_contacto VARCHAR(20)
);

-- Crear la tabla Expedientes
CREATE TABLE Expedientes (
    ID INT PRIMARY KEY IDENTITY,
    id_joven INT,
    nombre_joven VARCHAR(100),
    edad INT,
    fecha_ingreso DATE,
    direccion VARCHAR(255),
    telefono_contacto VARCHAR(20),
    tutor_legal VARCHAR(255),
    antecedentes_medicos TEXT,
    historial_academico TEXT,
    notas_adicionales TEXT,
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Crear la tabla Users
CREATE TABLE Users (
    ID INT PRIMARY KEY IDENTITY,
    nombre_usuario VARCHAR(100) UNIQUE,
	nombre VARCHAR(100),
	apellidos VARCHAR(100),
	correo VARCHAR(100),
	fecha_nacimiento DATE,
	cedula INT,
    contraseña VARCHAR(255),
    id_rol INT,
    FOREIGN KEY (id_rol) REFERENCES Roles(ID)
);

-- Crear la tabla Reportes_Expedientes
CREATE TABLE Reportes_Expedientes (
    ID INT PRIMARY KEY IDENTITY,
    id_expediente INT,
    id_usuario INT,
    tipo VARCHAR(50),
    contenido TEXT,
    fecha_creacion DATETIME,
    FOREIGN KEY (id_expediente) REFERENCES Expedientes(ID),
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);

-- Crear la tabla Reportes_Medicos
CREATE TABLE Reportes_Medicos (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_joven INT,
    fecha_creacion DATETIME,
    contenido TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Crear la tabla Pruebas_Dopaje
CREATE TABLE Pruebas_Dopaje (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_joven INT,
    fecha_hora DATETIME,
    lugar VARCHAR(255),
    FOREIGN KEY (id_usuario) REFERENCES Users(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Crear la tabla Incidentes
CREATE TABLE Incidentes (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_joven INT,
    fecha_hora DATETIME,
    descripcion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Crear la tabla Citas
CREATE TABLE Citas (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_joven INT,
    fecha DATETIME,
    tipo_usuario VARCHAR(50),
    detalles TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Crear la tabla Inventario_Comedor
CREATE TABLE Inventario_Comedor (
    ID INT PRIMARY KEY IDENTITY,
    nombre_alimento VARCHAR(255),
    cantidad_disponible INT,
    fecha_ultima_reposicion DATE,
    proveedor VARCHAR(255)
);

-- Crear la tabla Inventario_Higiene_Personal
CREATE TABLE Inventario_Higiene_Personal (
    ID INT PRIMARY KEY IDENTITY,
    nombre_producto VARCHAR(255),
    cantidad_disponible INT,
    fecha_ultima_reposicion DATE,
    proveedor VARCHAR(255)
);

CREATE TABLE Tickete (
    ID INT PRIMARY KEY IDENTITY,
	id_joven int,
	id_inventario_higiene_personal int,
	Tickete int,
    FOREIGN KEY (id_inventario_higiene_personal) REFERENCES Inventario_Higiene_Personal(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);












--Alter de Inventario Comedor y Higiene Personal

ALTER TABLE Inventario_Comedor
ADD imagen VARBINARY(MAX) NULL;

ALTER TABLE Inventario_Higiene_Personal
ADD imagen VARBINARY(MAX) NULL,
    precio_unitario DECIMAL(10, 2) NULL;





	-- Crear la tabla Capacitaciones
CREATE TABLE Capacitaciones  (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    nombre_capacitacion VARCHAR(255),
	fecha DATE,
    descripcion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);

-- Crear la tabla Asistencia
CREATE TABLE Asistencia  (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    estado VARCHAR(255),
	fecha DATE,
	horaRegistro DATETIME,
	horaSalida DATETIME,
    justificacion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);
-- Crear la tabla vacaciones 
CREATE TABLE Vacaciones  (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    estado VARCHAR(255),
	fechaInicio DATE,
	fechaFinaliza DATE,
    justificacion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);


--ALTER TABLE Users
--ADD correo VARCHAR(100);