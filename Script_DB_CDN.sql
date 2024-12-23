--Crear DB
CREATE DATABASE CDN;
-- Crear DB
CREATE DATABASE CDN;
GO

USE CDN;
GO

-- Crear la tabla Roles
CREATE TABLE Roles (
    ID INT PRIMARY KEY IDENTITY,
    nombre_rol VARCHAR(50) UNIQUE
);

-- Insertar roles predeterminados
INSERT INTO Roles (nombre_rol) VALUES 
    ('administrador'), 
    ('salud'), 
    ('psicologo'), 
    ('trabajador_social'),
    ('estudiante');

-- Crear la tabla Jovenes
CREATE TABLE Jovenes (
    ID INT PRIMARY KEY IDENTITY,
	cedula INT,
    nombre VARCHAR(100),
    edad INT,
    direccion VARCHAR(255),
    telefono_contacto VARCHAR(20)
);

-- Inserts ficticios para Jovenes
INSERT INTO Jovenes (cedula, nombre, edad, direccion, telefono_contacto) VALUES
    (12345678, 'María López', 18, 'Calle Principal 123', '555-1234'),
    (98765432, 'Pedro González', 17, 'Avenida Central 456', '555-5678'),
    (56781234, 'Ana Martínez', 16, 'Boulevard Norte 789', '555-9012'),
    (34567890, 'Carlos Rodríguez', 19, 'Avenida Sur 234', '555-3456'),
    (89012345, 'Laura Pérez', 20, 'Carrera Este 567', '555-6789');

-- Crear la tabla Expedientes
CREATE TABLE Expedientes (
    ID INT PRIMARY KEY IDENTITY,
    id_joven INT,
    nombre_joven VARCHAR(100),
    fecha_ingreso DATE,
    tutor_legal VARCHAR(255),
    antecedentes_medicos TEXT,
    historial_academico TEXT,
    notas_adicionales TEXT,
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);
-- Inserts ficticios para Expedientes
INSERT INTO Expedientes (id_joven, nombre_joven, edad, fecha_ingreso, direccion, telefono_contacto, tutor_legal, antecedentes_medicos, historial_academico, notas_adicionales) VALUES
    (1, 'María López', 18, '2024-06-01', 'Calle Principal 123', '555-1234', 'Ana Gómez', 'Antecedentes de alergias estacionales. Ninguna enfermedad crónica.', 'Promedio académico destacado en escuela secundaria. Participación en actividades extracurriculares.', 'Interés en continuar estudios universitarios.'),
    (2, 'Pedro González', 17, '2024-06-02', 'Avenida Central 456', '555-5678', 'Luis Pérez', 'Historial de asma controlado. Vacunaciones al día.', 'Regular en estudios, con mejoras significativas en el último año.', 'Participación en actividades deportivas.'),
    (3, 'Ana Martínez', 16, '2024-06-03', 'Boulevard Norte 789', '555-9012', 'Sandra Rodríguez', 'No presenta antecedentes médicos relevantes.', 'Excelente desempeño académico en la escuela primaria.', 'Intereses artísticos y culturales.'),
    (4, 'Carlos Rodríguez', 19, '2024-06-04', 'Avenida Sur 234', '555-3456', 'Juan Pérez', 'Control regular de hipertensión. Sin otras condiciones médicas relevantes.', 'Graduado de bachillerato técnico con distinciones.', 'Habilidades técnicas en informática y electrónica.'),
    (5, 'Laura Pérez', 20, '2024-06-05', 'Carrera Este 567', '555-6789', 'María Ramírez', 'Historial de alergias alimentarias. Chequeos médicos regulares.', 'Estudiante universitaria con enfoque en ciencias sociales.', 'Voluntaria activa en programas comunitarios.');


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

-- Inserts ficticios para Users
INSERT INTO Users (nombre_usuario, nombre, apellidos, correo, fecha_nacimiento, cedula, contraseña, id_rol) VALUES
    ('admin123', 'Admin', 'Sistema', 'admin@example.com', '1990-01-01', 123456789, 'adminpwd', 1),
    ('psico456', 'Psicólogo', 'Clínico', 'psico@example.com', '1985-05-15', 987654321, 'psicopwd', 3),
    ('worker789', 'Trabajador', 'Social', 'worker@example.com', '1988-11-20', 567890123, 'socialpwd', 4),
    ('estu567', 'Estudiante', 'Universitario', 'estu@example.com', '2000-03-10', 345678901, 'estupwd', 5),
    ('health234', 'Personal', 'Salud', 'salud@example.com', '1995-08-25', 890123456, 'saludpwd', 2);

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

-- Inserts ficticios para Reportes_Expedientes
INSERT INTO Reportes_Expedientes (id_expediente, id_usuario, tipo, contenido, fecha_creacion) VALUES
    (1, 2, 'Observación', 'Informe detallado del progreso', GETDATE()),
    (2, 3, 'Seguimiento', 'Notas adicionales sobre desarrollo', GETDATE()),
    (3, 1, 'Diagnóstico', 'Resumen médico completo', GETDATE()),
    (1, 4, 'Educación', 'Registro académico y notas', GETDATE()),
    (2, 5, 'Social', 'Informe de seguimiento social', GETDATE());

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

-- Inserts ficticios para Reportes_Medicos
INSERT INTO Reportes_Medicos (id_usuario, id_joven, fecha_creacion, contenido) VALUES
    (2, 1, GETDATE(), 'Exámenes y resultados médicos'),
    (3, 2, GETDATE(), 'Consulta psicológica y recomendaciones'),
    (4, 3, GETDATE(), 'Historial médico detallado'),
    (5, 4, GETDATE(), 'Exámenes de salud física'),
    (1, 5, GETDATE(), 'Registro de salud general');

-- Crear la tabla Pruebas_Dopaje
CREATE TABLE Pruebas_Dopaje (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_joven INT,
    fecha DATE,
    resultado VARCHAR(8) CHECK (resultado IN ('Positivo', 'Negativo')) NULL,
    observaciones VARCHAR(255),
    lugar VARCHAR(255),
    FOREIGN KEY (id_usuario) REFERENCES Users(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);

-- Inserts ficticios para Pruebas_Dopaje
INSERT INTO Pruebas_Dopaje (id_usuario, id_joven, fecha, resultado, observaciones, lugar) VALUES
    (1, 1, '2024-06-30', 'Negativo', 'Sin observaciones adicionales', 'Laboratorio Central'),
    (2, 2, '2024-07-01', 'Positivo', 'Requiere seguimiento adicional', 'Clínica Norte'),
    (3, 3, '2024-07-02', 'Positivo', 'Prueba en curso', 'Estadio Municipal'),
    (4, 4, '2024-07-03', 'Negativo', 'Resultados normales', 'Centro de Salud Este'),
    (5, 5, '2024-07-04', 'Positivo', 'Recomendaciones de seguimiento', 'Hospital General');

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

-- Inserts ficticios para Incidentes
INSERT INTO Incidentes (id_usuario, id_joven, fecha_hora, descripcion) VALUES
    (1, 1, GETDATE(), 'Incidente menor reportado en el centro'),
    (2, 2, GETDATE(), 'Altercado entre jóvenes durante la actividad deportiva'),
    (3, 3, GETDATE(), 'Emergencia médica atendida de forma inmediata'),
    (4, 4, GETDATE(), 'Evento social con resultados positivos'),
    (5, 5, GETDATE(), 'Conducta inapropiada corregida de manera efectiva');

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

-- Inserts ficticios para Citas
INSERT INTO Citas (id_usuario, id_joven, fecha, tipo_usuario, detalles) VALUES
    (1, 1, GETDATE(), 'Administrativo', 'Reunión mensual de evaluación'),
    (2, 2, GETDATE(), 'Psicológico', 'Consulta individual programada'),
    (3, 3, GETDATE(), 'Social', 'Revisión de condiciones de vida'),
    (4, 4, GETDATE(), 'Educativo', 'Sesión de tutoría académica'),
    (5, 5, GETDATE(), 'Salud', 'Examen de salud preventivo');

-- Crear la tabla Inventario_Comedor
CREATE TABLE Inventario_Comedor (
    ID INT PRIMARY KEY IDENTITY,
    nombre_alimento VARCHAR(255),
    cantidad_disponible INT,
    fecha_ultima_reposicion DATE,
    proveedor VARCHAR(255),
    imagen VARBINARY(MAX) NULL
);

-- Inserts ficticios para Inventario_Comedor
INSERT INTO Inventario_Comedor (nombre_alimento, cantidad_disponible, fecha_ultima_reposicion, proveedor) VALUES
    ('Arroz', 100, '2024-06-30', 'Distribuidora Alimentaria S.A.'),
    ('Leche', 50, '2024-06-29', 'Lácteos del Valle'),
    ('Frutas', 75, '2024-06-28', 'Fruitexpress Ltda.'),
    ('Carne', 30, '2024-06-27', 'Carnicería Don José'),
    ('Verduras', 80, '2024-06-26', 'Agricultura Orgánica S.A.');

-- Crear la tabla Inventario_Higiene_Personal
CREATE TABLE Inventario_Higiene_Personal (
    ID INT PRIMARY KEY IDENTITY,
    nombre_producto VARCHAR(255),
    cantidad_disponible INT,
    fecha_ultima_reposicion DATE,
    proveedor VARCHAR(255),
    imagen VARBINARY(MAX) NULL,
    precio_unitario DECIMAL(10, 2) NULL
);

-- Inserts ficticios para Inventario_Higiene_Personal
INSERT INTO Inventario_Higiene_Personal (nombre_producto, cantidad_disponible, fecha_ultima_reposicion, proveedor, precio_unitario) VALUES
    ('Jabón', 200, '2024-06-30', 'Productos de Limpieza S.A.', 5.99),
    ('Desodorante', 150, '2024-06-29', 'Desodorante del Sur Ltda.', 3.49),
    ('Cepillo Dental', 300, '2024-06-28', 'Dentaid S.A.', 2.79),
    ('Shampoo', 100, '2024-06-27', 'Cosméticos Belleza Total Ltda.', 7.99),
    ('Plastigel', 120, '2024-06-26', 'Plastigel S.A.', 4.99);

-- Crear la tabla Capacitaciones
CREATE TABLE Capacitaciones (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    nombre_capacitacion VARCHAR(255),
	fecha DATE,
    descripcion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);
-- Insert ficticio para Capacitaciones
INSERT INTO Capacitaciones (id_usuario, nombre_capacitacion, fecha, descripcion) VALUES
    (1, 'Capacitación A', '2024-07-05', 'Taller práctico sobre gestión emocional y trabajo en equipo'),
    (2, 'Capacitación A', '2024-07-06', 'Seminario intensivo sobre técnicas de intervención psicológica'),
    (3, 'Capacitación A', '2024-07-07', 'Curso avanzado en desarrollo comunitario y resolución de conflictos'),
    (4, 'Capacitación A', '2024-07-08', 'Charla informativa sobre políticas educativas y bienestar estudiantil'),
    (5, 'Capacitación A', '2024-07-09', 'Taller de actualización en normativas y procedimientos de salud');

-- Crear la tabla Asistencia
CREATE TABLE Asistencia (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    estado VARCHAR(255),
	fecha DATE,
	horaRegistro DATETIME,
	horaSalida DATETIME,
    justificacion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);

-- Inserts ficticios para Asistencia
INSERT INTO Asistencia (id_usuario, estado, fecha, horaRegistro, horaSalida, justificacion) VALUES
    (1, 'Presente', '2024-06-30', '2024-06-30 08:00:00', '2024-06-30 17:00:00', 'Sin justificación requerida'),
    (2, 'Ausente', '2024-06-30', '2024-06-30 09:30:00', '2024-06-30 16:00:00', 'Razones personales'),
    (3, 'Presente', '2024-06-30', '2024-06-30 08:15:00', '2024-06-30 18:00:00', 'Asistencia completa'),
    (4, 'Presente', '2024-06-30', '2024-06-30 07:45:00', '2024-06-30 15:30:00', 'Sin justificación requerida'),
    (5, 'Ausente', '2024-06-30', '2024-06-30 10:00:00', '2024-06-30 17:45:00', 'Falta no prevista');

-- Crear la tabla Vacaciones
CREATE TABLE Vacaciones (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    estado VARCHAR(255),
	fechaInicio DATE,
	fechaFinaliza DATE,
    justificacion TEXT,
    FOREIGN KEY (id_usuario) REFERENCES Users(ID)
);

-- Inserts ficticios para Vacaciones
INSERT INTO Vacaciones (id_usuario, estado, fechaInicio, fechaFinaliza, justificacion) VALUES
    (1, 'Aprobadas', '2024-07-15', '2024-07-30', 'Vacaciones planificadas con antelación'),
    (2, 'Pendientes', '2024-08-01', '2024-08-15', 'Solicitud en revisión'),
    (3, 'Rechazadas', '2024-07-10', '2024-07-14', 'Fechas no disponibles'),
    (4, 'Aprobadas', '2024-07-20', '2024-08-05', 'Vacaciones confirmadas'),
    (5, 'Pendientes', '2024-07-25', '2024-08-10', 'Esperando confirmación');

-- Crear la tabla Tickete
CREATE TABLE Tickete (
    ID INT PRIMARY KEY IDENTITY,
	id_joven INT,
	id_inventario_higiene_personal INT,
	Tickete INT,
    FOREIGN KEY (id_inventario_higiene_personal) REFERENCES Inventario_Higiene_Personal(ID),
    FOREIGN KEY (id_joven) REFERENCES Jovenes(ID)
);





--Nuevo 17/7
DROP TABLE Tickete;

CREATE TABLE Tickete (
    ID INT PRIMARY KEY IDENTITY,
    id_usuario INT,
    id_inventario_higiene_personal INT,
    Tickete INT,
    FOREIGN KEY (id_inventario_higiene_personal) REFERENCES Inventario_Higiene_Personal(ID),
    FOREIGN KEY (id_usuario) REFERENCES Users(Id)
);



CREATE TABLE RegistroCompra (
    Id INT PRIMARY KEY IDENTITY,
    TicketeId INT NOT NULL,
    UserId INT NOT NULL,
	Inventario_HigieneId INT NOT NULL,
	estado VARCHAR(255),
    FOREIGN KEY (UserId) REFERENCES Users(ID),
	FOREIGN KEY (Inventario_HigieneId) REFERENCES Inventario_Higiene_Personal(ID)
);




ALTER TABLE Jovenes
ADD Localizacion VARCHAR(255);



INSERT INTO Jovenes (cedula, nombre, edad, direccion, telefono_contacto, Localizacion)
VALUES
(123456789, 'Juan Pérez', 20, 'Calle 1, Ciudad', '555-1234', 'Residencia Cartago'),
(234567890, 'Ana Gómez', 22, 'Calle 2, Ciudad', '555-2345', 'Albergue San Agustín'),
(345678901, 'Luis Fernández', 19, 'Calle 3, Ciudad', '555-3456', 'Residencia Heredia'),
(456789012, 'María López', 21, 'Calle 4, Ciudad', '555-4567', 'Albergue Santa Magdalena'),
(567890123, 'Carlos Martínez', 18, 'Calle 5, Ciudad', '555-5678', 'Residencia Guanacaste'),
(678901234, 'Laura Fernández', 23, 'Calle 6, Ciudad', '555-6789', 'Albergue Santa Mónica'),
(789012345, 'Pedro González', 20, 'Calle 7, Ciudad', '555-7890', 'Residencia Limon'),
(890123456, 'Isabel Ramírez', 22, 'Calle 8, Ciudad', '555-8901', 'Albergue Santa Rita'),
(901234567, 'José Rodríguez', 19, 'Calle 9, Ciudad', '555-9012', 'Albergue Miami'),
(123456780, 'Sandra Silva', 21, 'Calle 10, Ciudad', '555-0123', 'Residencia Alajuela'),
(234567891, 'Roberto Morales', 18, 'Calle 11, Ciudad', '555-1234', 'Albergue Cipreses'),
(345678902, 'Mariana Castro', 22, 'Calle 12, Ciudad', '555-2345', 'Residencia Cartago'),
(456789013, 'Andrés Jiménez', 20, 'Calle 13, Ciudad', '555-3456', 'Albergue Los Ángeles'),
(567890134, 'Verónica Ruiz', 21, 'Calle 14, Ciudad', '555-4567', 'Residencia Heredia'),
(678901245, 'David Moreno', 23, 'Calle 15, Ciudad', '555-5678', 'Albergue San Agustín'),
(789012356, 'Julia Vargas', 18, 'Calle 16, Ciudad', '555-6789', 'Residencia Guanacaste'),
(890123467, 'Eduardo Sánchez', 22, 'Calle 17, Ciudad', '555-7890', 'Albergue Santa Magdalena'),
(901234578, 'Sofía Pérez', 20, 'Calle 18, Ciudad', '555-8901', 'Residencia Limon'),
(123456791, 'Ricardo Vega', 21, 'Calle 19, Ciudad', '555-9012', 'Albergue Santa Mónica'),
(234567902, 'Carmen Díaz', 19, 'Calle 20, Ciudad', '555-0123', 'Residencia Alajuela'),
(345678913, 'Miguel Ángel', 22, 'Calle 21, Ciudad', '555-1234', 'Albergue Santa Rita'),
(456789024, 'Gabriela Martínez', 18, 'Calle 22, Ciudad', '555-2345', 'Albergue Miami'),
(567890135, 'Fernando López', 23, 'Calle 23, Ciudad', '555-3456', 'Residencia Cartago'),
(678901246, 'Estela Gómez', 21, 'Calle 24, Ciudad', '555-4567', 'Albergue Cipreses'),
(789012357, 'Mario Pérez', 20, 'Calle 25, Ciudad', '555-5678', 'Residencia Heredia'),
(890123468, 'Beatriz Ruiz', 22, 'Calle 26, Ciudad', '555-6789', 'Albergue Los Ángeles'),
(901234579, 'Jorge Castro', 19, 'Calle 27, Ciudad', '555-7890', 'Residencia Guanacaste'),
(123456792, 'Nina Ramírez', 21, 'Calle 28, Ciudad', '555-8901', 'Albergue San Agustín'),
(234567903, 'Luis Hernández', 20, 'Calle 29, Ciudad', '555-9012', 'Residencia Limon'),
(345678914, 'Sara Gómez', 18, 'Calle 30, Ciudad', '555-0123', 'Albergue Santa Magdalena'),
(456789025, 'Alejandro Vargas', 23, 'Calle 31, Ciudad', '555-1234', 'Residencia Alajuela'),
(567890146, 'Mariela Silva', 22, 'Calle 32, Ciudad', '555-2345', 'Albergue Santa Mónica'),
(678901257, 'Héctor Moreno', 21, 'Calle 33, Ciudad', '555-3456', 'Residencia Cartago'),
(789012368, 'Fernanda Castro', 20, 'Calle 34, Ciudad', '555-4567', 'Albergue Santa Rita'),
(890123479, 'Ricardo Díaz', 19, 'Calle 35, Ciudad', '555-5678', 'Albergue Miami'),
(901234580, 'Silvia López', 23, 'Calle 36, Ciudad', '555-6789', 'Residencia Guanacaste'),
(123456803, 'Alejandra Pérez', 21, 'Calle 37, Ciudad', '555-7890', 'Albergue Cipreses'),
(234567914, 'Carlos Silva', 20, 'Calle 38, Ciudad', '555-8901', 'Residencia Heredia'),
(345678925, 'Tatiana Gómez', 18, 'Calle 39, Ciudad', '555-9012', 'Albergue Los Ángeles'),
(456789036, 'Emilio Fernández', 22, 'Calle 40, Ciudad', '555-0123', 'Residencia Limon');


UPDATE Jovenes
SET telefono_contacto = '12345678';



CREATE TABLE Rifas (
    RifaId INT PRIMARY KEY IDENTITY,
    FechaRifa DATETIME,
	 NumeroGanador INT,
    Premio NVARCHAR(255)
);


CREATE TABLE RifaEntries (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    Correo NVARCHAR(100),
    NumeroComprado INT,
    FechaCompra DATETIME DEFAULT GETDATE(),
    RifaId INT,
    FOREIGN KEY (RifaId) REFERENCES Rifas(RifaId)
);


INSERT INTO Rifas (FechaRifa, NumeroGanador, Premio)
VALUES ('2024-08-15', NULL, 'Premio de Ejemplo');
INSERT INTO Rifas (FechaRifa, Premio, NumeroGanador)
VALUES ('2024-08-04 17:39:00', 'Premio Especial', NULL);
INSERT INTO Rifas (FechaRifa, Premio, NumeroGanador)
VALUES ('2024-08-07 21:15:00', 'Premio Especial', NULL);


alter table Rifas
ADD valor int;

INSERT INTO Pruebas_Dopaje (id_usuario, id_joven, fecha, resultado, observaciones, lugar) VALUES
(1, 1, '2023-01-15', 'Positivo', 'Observación de prueba 1', 'Lugar de prueba 1'),
(2, 2, '2023-01-22', 'Negativo', 'Observación de prueba 2', 'Lugar de prueba 2'),
(3, 3, '2023-02-10', 'Positivo', 'Observación de prueba 3', 'Lugar de prueba 3'),
(4, 4, '2023-02-20', 'Negativo', 'Observación de prueba 4', 'Lugar de prueba 4'),
(5, 5, '2023-03-05', 'Positivo', 'Observación de prueba 5', 'Lugar de prueba 5'),
(6, 6, '2023-03-15', 'Negativo', 'Observación de prueba 6', 'Lugar de prueba 6'),
(7, 7, '2023-03-25', 'Positivo', 'Observación de prueba 7', 'Lugar de prueba 7'),
(6, 8, '2023-04-10', 'Negativo', 'Observación de prueba 8', 'Lugar de prueba 8'),
(2, 9, '2023-04-20', 'Positivo', 'Observación de prueba 9', 'Lugar de prueba 9'),
(1, 10, '2023-05-05', 'Negativo', 'Observación de prueba 10', 'Lugar de prueba 10'),
(1, 2, '2023-05-15', 'Positivo', 'Observación de prueba 11', 'Lugar de prueba 11'),
(2, 3, '2023-06-01', 'Negativo', 'Observación de prueba 12', 'Lugar de prueba 12'),
(3, 4, '2023-06-10', 'Positivo', 'Observación de prueba 13', 'Lugar de prueba 13'),
(4, 5, '2023-06-20', 'Negativo', 'Observación de prueba 14', 'Lugar de prueba 14'),
(5, 6, '2023-07-05', 'Positivo', 'Observación de prueba 15', 'Lugar de prueba 15'),
(6, 7, '2023-07-15', 'Negativo', 'Observación de prueba 16', 'Lugar de prueba 16'),
(7, 8, '2023-08-01', 'Positivo', 'Observación de prueba 17', 'Lugar de prueba 17'),
(3, 9, '2023-08-10', 'Negativo', 'Observación de prueba 18', 'Lugar de prueba 18'),
(3, 10, '2023-08-20', 'Positivo', 'Observación de prueba 19', 'Lugar de prueba 19'),
(1, 1, '2023-09-05', 'Negativo', 'Observación de prueba 20', 'Lugar de prueba 20'),
(1, 3, '2023-09-15', 'Positivo', 'Observación de prueba 21', 'Lugar de prueba 21'),
(2, 4, '2023-10-01', 'Negativo', 'Observación de prueba 22', 'Lugar de prueba 22'),
(3, 5, '2023-10-10', 'Positivo', 'Observación de prueba 23', 'Lugar de prueba 23'),
(4, 6, '2023-10-20', 'Negativo', 'Observación de prueba 24', 'Lugar de prueba 24'),
(5, 7, '2023-11-05', 'Positivo', 'Observación de prueba 25', 'Lugar de prueba 25'),
(6, 8, '2023-11-15', 'Negativo', 'Observación de prueba 26', 'Lugar de prueba 26'),
(7, 9, '2023-12-01', 'Positivo', 'Observación de prueba 27', 'Lugar de prueba 27'),
(2, 10, '2023-12-10', 'Negativo', 'Observación de prueba 28', 'Lugar de prueba 28'),
(3, 1, '2023-12-20', 'Positivo', 'Observación de prueba 29', 'Lugar de prueba 29'),
(1, 2, '2023-12-30', 'Negativo', 'Observación de prueba 30', 'Lugar de prueba 30'),
(1, 4, '2023-01-25', 'Positivo', 'Observación de prueba 31', 'Lugar de prueba 31'),
(2, 5, '2023-02-05', 'Negativo', 'Observación de prueba 32', 'Lugar de prueba 32'),
(3, 6, '2023-02-15', 'Positivo', 'Observación de prueba 33', 'Lugar de prueba 33'),
(4, 7, '2023-03-01', 'Negativo', 'Observación de prueba 34', 'Lugar de prueba 34'),
(5, 8, '2023-03-10', 'Positivo', 'Observación de prueba 35', 'Lugar de prueba 35'),
(6, 9, '2023-03-20', 'Negativo', 'Observación de prueba 36', 'Lugar de prueba 36'),
(7, 10, '2023-04-05', 'Positivo', 'Observación de prueba 37', 'Lugar de prueba 37'),
(1, 1, '2023-04-15', 'Negativo', 'Observación de prueba 38', 'Lugar de prueba 38'),
(2, 2, '2023-05-01', 'Positivo', 'Observación de prueba 39', 'Lugar de prueba 39'),
(1, 3, '2023-05-10', 'Negativo', 'Observación de prueba 40', 'Lugar de prueba 40'),
(1, 5, '2023-05-20', 'Positivo', 'Observación de prueba 41', 'Lugar de prueba 41'),
(2, 6, '2023-06-01', 'Negativo', 'Observación de prueba 42', 'Lugar de prueba 42'),
(3, 7, '2023-06-10', 'Positivo', 'Observación de prueba 43', 'Lugar de prueba 43'),
(4, 8, '2023-06-20', 'Negativo', 'Observación de prueba 44', 'Lugar de prueba 44'),
(5, 9, '2023-07-05', 'Positivo', 'Observación de prueba 45', 'Lugar de prueba 45'),
(6, 10, '2023-07-15', 'Negativo', 'Observación de prueba 46', 'Lugar de prueba 46'),
(7, 1, '2023-08-01', 'Positivo', 'Observación de prueba 47', 'Lugar de prueba 47'),
(4, 2, '2023-08-10', 'Negativo', 'Observación de prueba 48', 'Lugar de prueba 48'),
(4, 3, '2023-08-20', 'Positivo', 'Observación de prueba 49', 'Lugar de prueba 49'),
(1, 4, '2023-09-05', 'Negativo', 'Observación de prueba 50', 'Lugar de prueba 50'),
(1, 6, '2023-09-15', 'Positivo', 'Observación de prueba 51', 'Lugar de prueba 51'),
(2, 7, '2023-10-01', 'Negativo', 'Observación de prueba 52', 'Lugar de prueba 52'),
(3, 8, '2023-10-10', 'Positivo', 'Observación de prueba 53', 'Lugar de prueba 53'),
(4, 9, '2023-10-20', 'Negativo', 'Observación de prueba 54', 'Lugar de prueba 54'),
(5, 10, '2023-11-05', 'Positivo', 'Observación de prueba 55', 'Lugar de prueba 55'),
(6, 1, '2023-11-15', 'Negativo', 'Observación de prueba 56', 'Lugar de prueba 56'),
(7, 2, '2023-12-01', 'Positivo', 'Observación de prueba 57', 'Lugar de prueba 57'),
(2, 3, '2023-12-10', 'Negativo', 'Observación de prueba 58', 'Lugar de prueba 58'),
(1, 4, '2023-12-20', 'Positivo', 'Observación de prueba 59', 'Lugar de prueba 59'),
(1, 5, '2023-12-30', 'Negativo', 'Observación de prueba 60', 'Lugar de prueba 60'),
(1, 7, '2023-01-20', 'Positivo', 'Observación de prueba 61', 'Lugar de prueba 61'),
(2, 8, '2023-02-05', 'Negativo', 'Observación de prueba 62', 'Lugar de prueba 62'),
(3, 9, '2023-02-15', 'Positivo', 'Observación de prueba 63', 'Lugar de prueba 63'),
(4, 10, '2023-03-01', 'Negativo', 'Observación de prueba 64', 'Lugar de prueba 64'),
(5, 1, '2023-03-10', 'Positivo', 'Observación de prueba 65', 'Lugar de prueba 65'),
(6, 2, '2023-03-20', 'Negativo', 'Observación de prueba 66', 'Lugar de prueba 66'),
(7, 3, '2023-04-05', 'Positivo', 'Observación de prueba 67', 'Lugar de prueba 67'),
(2, 4, '2023-04-15', 'Negativo', 'Observación de prueba 68', 'Lugar de prueba 68'),
(2, 5, '2023-05-01', 'Positivo', 'Observación de prueba 69', 'Lugar de prueba 69'),
(1, 6, '2023-05-10', 'Negativo', 'Observación de prueba 70', 'Lugar de prueba 70'),
(1, 8, '2023-05-20', 'Positivo', 'Observación de prueba 71', 'Lugar de prueba 71'),
(2, 9, '2023-06-01', 'Negativo', 'Observación de prueba 72', 'Lugar de prueba 72'),
(3, 10, '2023-06-10', 'Positivo', 'Observación de prueba 73', 'Lugar de prueba 73'),
(4, 1, '2023-06-20', 'Negativo', 'Observación de prueba 74', 'Lugar de prueba 74'),
(5, 2, '2023-07-05', 'Positivo', 'Observación de prueba 75', 'Lugar de prueba 75'),
(6, 3, '2023-07-15', 'Negativo', 'Observación de prueba 76', 'Lugar de prueba 76'),
(7, 4, '2023-08-01', 'Positivo', 'Observación de prueba 77', 'Lugar de prueba 77'),
(1, 5, '2023-08-10', 'Negativo', 'Observación de prueba 78', 'Lugar de prueba 78'),
(1, 6, '2023-08-20', 'Positivo', 'Observación de prueba 79', 'Lugar de prueba 79'),
(1, 7, '2023-09-05', 'Negativo', 'Observación de prueba 80', 'Lugar de prueba 80'),
(1, 9, '2023-09-15', 'Positivo', 'Observación de prueba 81', 'Lugar de prueba 81'),
(2, 10, '2023-10-01', 'Negativo', 'Observación de prueba 82', 'Lugar de prueba 82'),
(3, 1, '2023-10-10', 'Positivo', 'Observación de prueba 83', 'Lugar de prueba 83'),
(4, 2, '2023-10-20', 'Negativo', 'Observación de prueba 84', 'Lugar de prueba 84'),
(5, 3, '2023-11-05', 'Positivo', 'Observación de prueba 85', 'Lugar de prueba 85'),
(6, 4, '2023-11-15', 'Negativo', 'Observación de prueba 86', 'Lugar de prueba 86'),
(7, 5, '2023-12-01', 'Positivo', 'Observación de prueba 87', 'Lugar de prueba 87'),
(3, 6, '2023-12-10', 'Negativo', 'Observación de prueba 88', 'Lugar de prueba 88'),
(2, 7, '2023-12-20', 'Positivo', 'Observación de prueba 89', 'Lugar de prueba 89'),
(1, 8, '2023-12-30', 'Negativo', 'Observación de prueba 90', 'Lugar de prueba 90'),
(1, 10, '2023-01-30', 'Positivo', 'Observación de prueba 91', 'Lugar de prueba 91'),
(2, 1, '2023-02-25', 'Negativo', 'Observación de prueba 92', 'Lugar de prueba 92'),
(3, 2, '2023-03-30', 'Positivo', 'Observación de prueba 93', 'Lugar de prueba 93'),
(4, 3, '2023-04-25', 'Negativo', 'Observación de prueba 94', 'Lugar de prueba 94'),
(5, 4, '2023-05-30', 'Positivo', 'Observación de prueba 95', 'Lugar de prueba 95'),
(6, 5, '2023-06-25', 'Negativo', 'Observación de prueba 96', 'Lugar de prueba 96'),
(7, 6, '2023-07-30', 'Positivo', 'Observación de prueba 97', 'Lugar de prueba 97'),
(1, 7, '2023-08-25', 'Negativo', 'Observación de prueba 98', 'Lugar de prueba 98'),
(1, 8, '2023-09-30', 'Positivo', 'Observación de prueba 99', 'Lugar de prueba 99'),
(1, 9, '2023-10-25', 'Negativo', 'Observación de prueba 100', 'Lugar de prueba 100'),
(1, 10, '2023-11-30', 'Positivo', 'Observación de prueba 101', 'Lugar de prueba 101'),
(2, 1, '2023-12-05', 'Negativo', 'Observación de prueba 102', 'Lugar de prueba 102'),
(3, 2, '2023-12-15', 'Positivo', 'Observación de prueba 103', 'Lugar de prueba 103'),
(4, 3, '2023-12-25', 'Negativo', 'Observación de prueba 104', 'Lugar de prueba 104'),
(5, 4, '2023-01-10', 'Positivo', 'Observación de prueba 105', 'Lugar de prueba 105'),
(6, 5, '2023-01-20', 'Negativo', 'Observación de prueba 106', 'Lugar de prueba 106'),
(7, 6, '2023-01-30', 'Positivo', 'Observación de prueba 107', 'Lugar de prueba 107'),
(1, 7, '2023-02-10', 'Negativo', 'Observación de prueba 108', 'Lugar de prueba 108'),
(1, 8, '2023-02-20', 'Positivo', 'Observación de prueba 109', 'Lugar de prueba 109'),
(1, 9, '2023-03-01', 'Negativo', 'Observación de prueba 110', 'Lugar de prueba 110'),
(1, 10, '2023-03-10', 'Positivo', 'Observación de prueba 111', 'Lugar de prueba 111'),
(2, 1, '2023-03-20', 'Negativo', 'Observación de prueba 112', 'Lugar de prueba 112'),
(3, 2, '2023-04-01', 'Positivo', 'Observación de prueba 113', 'Lugar de prueba 113'),
(4, 3, '2023-04-10', 'Negativo', 'Observación de prueba 114', 'Lugar de prueba 114'),
(5, 4, '2023-04-20', 'Positivo', 'Observación de prueba 115', 'Lugar de prueba 115'),
(6, 5, '2023-05-01', 'Negativo', 'Observación de prueba 116', 'Lugar de prueba 116'),
(1, 6, '2023-05-10', 'Positivo', 'Observación de prueba 117', 'Lugar de prueba 117'),
(1, 7, '2023-05-20', 'Negativo', 'Observación de prueba 118', 'Lugar de prueba 118'),
(1, 8, '2023-06-01', 'Positivo', 'Observación de prueba 119', 'Lugar de prueba 119'),
(1, 9, '2023-06-10', 'Negativo', 'Observación de prueba 120', 'Lugar de prueba 120'),
(1, 10, '2023-06-20', 'Positivo', 'Observación de prueba 121', 'Lugar de prueba 121'),
(2, 1, '2023-07-01', 'Negativo', 'Observación de prueba 122', 'Lugar de prueba 122'),
(3, 2, '2023-07-10', 'Positivo', 'Observación de prueba 123', 'Lugar de prueba 123'),
(4, 3, '2023-07-20', 'Negativo', 'Observación de prueba 124', 'Lugar de prueba 124'),
(5, 4, '2023-08-01', 'Positivo', 'Observación de prueba 125', 'Lugar de prueba 125');


SELECT * FROM Users WHERE ID IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
SELECT * FROM Jovenes WHERE ID IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10);



--Agregar estado de activo y desactivo

--Citas
ALTER TABLE Citas
ADD estado VARCHAR(50) DEFAULT 'Activo';

UPDATE Citas
SET estado = 'Activo'
WHERE estado IS NULL;

-- Agregar columna estado a la tabla Inventario_Comedor
ALTER TABLE Inventario_Comedor
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Inventario_Comedor
SET estado = 'Activo'
WHERE estado IS NULL;


-- Agregar columna estado a la tabla Inventario_Higiene_Personal
ALTER TABLE Inventario_Higiene_Personal
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Inventario_Higiene_Personal
SET estado = 'Activo'
WHERE estado IS NULL;


ALTER TABLE Reportes_Expedientes
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Reportes_Expedientes
SET estado = 'Activo'
WHERE estado IS NULL;


ALTER TABLE Incidentes
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Incidentes
SET estado = 'Activo'
WHERE estado IS NULL;



ALTER TABLE Expedientes  
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Expedientes 
SET estado = 'Activo'
WHERE estado IS NULL;



ALTER TABLE Reportes_Medicos  
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Reportes_Medicos 
SET estado = 'Activo'
WHERE estado IS NULL;


ALTER TABLE Pruebas_Dopaje  
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Pruebas_Dopaje
SET estado = 'Activo'
WHERE estado IS NULL;


--quitar tipo_usuario no es necesario 
ALTER TABLE Citas
DROP COLUMN tipo_usuario;




ALTER TABLE Users  
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Users 
SET estado = 'Activo'
WHERE estado IS NULL;



ALTER TABLE Jovenes  
ADD estado VARCHAR(50) DEFAULT 'Activo';

-- Actualizar el estado a 'Activo' donde el estado es NULL
UPDATE Jovenes 
SET estado = 'Activo'
WHERE estado IS NULL;