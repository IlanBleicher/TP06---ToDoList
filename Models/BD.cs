using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;


namespace TP06_ToDoList;

static public class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=listaTareas;Integrated Security=True;TrustServerCertificate=True;";



// login
     public static int Login(string username, string password)
    {
        int IDusuarioBuscado = -1;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ID FROM Usuarios WHERE username = @pUsername AND password = @pPassword";
            IDusuarioBuscado = connection.QueryFirstOrDefault<int>(query, new { pPassword = password, pUsername = username});
        }
            return IDusuarioBuscado;
    }

// Registrarse
    public static void SignIn(string username, string password, string nombre, string apellido, string foto, DateTime fechaUltimoLogin)
    {
        if(GetUsuario(username, password) == null)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Usuarios (username, password, nombre, apellido, foto, ultimoLogin) VALUES (@pUsername, @pPassword, @pNombre, @pApellido, @pFoto, @pFechaUltimoLogin)";
                connection.Execute(query, new {pUsername = username, pPassword = password, pNombre = nombre, pApellido = apellido, pFoto = foto, pFechaUltimoLogin = fechaUltimoLogin});
            }
        }
    }

    public static Usuario GetUsuario(string username, string password)
    {
       Usuario usuarioBuscado = null;
         using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE username = @pUsername AND password = @pPassword";
            usuarioBuscado = connection.QueryFirstOrDefault<Usuario>(query, new { pUsername = username, pPassword = password});
        }

        return usuarioBuscado;
    }
    
    public static List<Tarea> getListaTareas(int idUsuario)
    {
        List<Tarea> listaDeTareas = new List<Tarea>();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Tareas WHERE IDUsuario = @pIDUsuario";
            listaDeTareas = connection.Query<Tarea>(query, new { pIDUsuario = idUsuario}).ToList();
        }

        return listaDeTareas;
    }

    public static void crearTarea(string titulo, string descripcion, DateTime fecha, int IDUsuario)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Tareas (titulo, descripcion, fecha, finalizada, IDUsuario) VALUES (@pTitulo, @pDescripcion, @pFecha, @pFinalizada, @pIDUsuario)";
            connection.Execute(query, new {pTitulo = titulo, pDescripcion = descripcion, pFecha = fecha, pFinalizada = 0, pIDUsuario = IDUsuario});
        }
    }

    public static void editarTarea(string titulo, string descripcion, DateTime fecha, bool finalizada, int IDUsuario, int idTarea)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tareas SET titulo = @pTitulo, descripcion = @pDescripcion, fecha = @pFecha, finalizada = @pFinalizada, IDUsuario = @pIDUsuario WHERE ID = @pIDTarea";
            connection.Execute(query, new {pTitulo = titulo, pDescripcion = descripcion, pFecha = fecha, pFinalizada = finalizada, pIDUsuario = IDUsuario, pIDTarea = idTarea});
        }
    }

    public static void borrarTarea(int idTarea)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "DELETE FROM Tareas WHERE ID = @pidTarea";
            connection.Execute(query, new {pidTarea = idTarea});
        }
    }

    public static void marcarTareaFinalizada(int idTarea)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tareas SET finalizada = 1 WHERE ID = @pIdTarea";
            connection.Execute(query, new {pIdTarea = idTarea});
        }
    }
    public static Tarea verTarea(int idTarea)
    {
        Tarea tareaBuscada =  null;

        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Tareas WHERE ID = @pIDTarea";
            tareaBuscada = connection.QueryFirstOrDefault<Tarea>(query, new { pIDTarea = idTarea });
        }

        return tareaBuscada;
    }

    public static DateTime getFechaUltIngr(int idUsuario)
    {
        DateTime fechaUltimoIngreso;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT ultimoLogin FROM Usuarios WHERE ID = @pIDUsuario";
            fechaUltimoIngreso = connection.QueryFirstOrDefault<DateTime>(query, new { pIDUsuario = idUsuario });

        }

        return fechaUltimoIngreso;

    }
}

