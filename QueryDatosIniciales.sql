-- ========================================
-- 🧱 TABLA: Rol
-- ========================================
IF NOT EXISTS (SELECT 1 FROM Rol)
BEGIN
    INSERT INTO Rol (Nombre, Descripcion, Estado)
    VALUES 
        ('Administrador', 'Acceso completo al sistema', 1),
        ('Entrenador', 'Gestiona rutinas, entrenamientos y atletas', 1),
        ('Atleta', 'Usuario que recibe rutinas asignadas', 1);

    PRINT('✅ Roles iniciales insertados');
END
GO


-- ========================================
-- 🧱 TABLA: Permiso
-- ========================================
IF NOT EXISTS (SELECT 1 FROM Permiso)
BEGIN
    INSERT INTO Permiso (Codigo, Descripcion, Categoria, Ruta, Estado)
    VALUES
        ('GESTIONAR_USUARIOS', 'Permite administrar usuarios del sistema', 'Users', '/Users/ManageUsers', 1),
        ('GESTIONAR_ATLETAS', 'Permite gestionar atletas', 'Athletes', '/Trainer/ManageAthletes', 1),
        ('VER_PERFIL', 'Permite ver el perfil del usuario', 'Profile', '/Profile/MyProfile', 1),
        ('EDITAR_PERFIL', 'Permite editar el perfil del usuario', 'Profile', '/Profile/EditProfile', 1),
        ('CAMBIAR_CONTRASENNA_PERFIL', 'Permite cambiar la contraseña del usuario', 'Profile', '/Profile/ChangePassword', 1),
        ('AGREGAR_ATLETA_ENTRENADOR', 'Permite al entrenador agregar atletas a su lista asignada', 'Usuarios', '/Trainer/AddAthlete', 1),
        ('ELIMINAR_ATLETA_ENTRENADOR', 'Permite al entrenador eliminar atletas de su lista', 'Usuarios', '/Trainer/DeleteAthlete', 1),
        ('VER_PERMISOS', 'Permite ver la lista de permisos', 'Security', '/Permissions/Index', 1);

    PRINT('✅ Permisos iniciales insertados');
END
GO


-- ========================================
-- 🔗 TABLA: RolPermiso
-- ========================================
-- ADMIN: todos los permisos
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso
FROM Rol r
CROSS JOIN Permiso p
WHERE r.Nombre = 'Administrador'
AND NOT EXISTS (
    SELECT 1 FROM RolPermiso rp WHERE rp.IdRol = r.IdRol AND rp.IdPermiso = p.IdPermiso
);

-- TRAINER: permisos específicos
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso
FROM Rol r
JOIN Permiso p ON p.Codigo IN ('GESTIONAR_ATLETAS', 'VER_PERFIL', 'EDITAR_PERFIL', 'CAMBIAR_CONTRASENNA_PERFIL', 'AGREGAR_ATLETA_ENTRENADOR', 'ELIMINAR_ATLETA_ENTRENADOR')
WHERE r.Nombre = 'Entrenador'
AND NOT EXISTS (
    SELECT 1 FROM RolPermiso rp WHERE rp.IdRol = r.IdRol AND rp.IdPermiso = p.IdPermiso
);

-- ATHLETE: solo perfil
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT r.IdRol, p.IdPermiso
FROM Rol r
JOIN Permiso p ON p.Codigo IN ('VER_PERFIL', 'EDITAR_PERFIL', 'CAMBIAR_CONTRASENNA_PERFIL')
WHERE r.Nombre = 'Atleta'
AND NOT EXISTS (
    SELECT 1 FROM RolPermiso rp WHERE rp.IdRol = r.IdRol AND rp.IdPermiso = p.IdPermiso
);

PRINT('✅ RolPermiso relaciones insertadas');
GO


-- ========================================
-- 👤 TABLA: Usuario (Administrador inicial)
-- ========================================
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Correo = 'admin@dominio.com')
BEGIN
    DECLARE @RolAdminId INT = (SELECT TOP 1 IdRol FROM Rol WHERE Nombre = 'Administrador');

    INSERT INTO Usuarios (
        Nombre, Apellido, Correo, Telefono, FechaCumpleannos,
        ContrasennaHash, Estado, FechaCreacion, RolId
    )
    VALUES (
        'Admin',
        'Sistema',
        'admin@dominio.com',
        '60000000',
        '1990-01-01',
        'UDd9Jxr59YTGLmp8Dofxlw==.Bf/QH105NwCI9Dt8C+fkRpjRXwOlPSEOjVZKMgqK0pI=',
        1,
        GETDATE(),
        @RolAdminId
    );

    PRINT('✅ Usuario administrador insertado');
END
GO


--DELETE FROM Usuarios
--DELETE FROM Rol
--DELETE FROM RolPermiso
--DELETE FROM Permiso