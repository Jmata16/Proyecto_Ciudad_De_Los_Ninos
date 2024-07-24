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
