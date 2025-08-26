using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06_ToDoList.Models;

namespace TP06_ToDoList.Controllers;

public class AccountController : Controller
{


    private readonly IWebHostEnvironment _env;

    public AccountController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult guardarLogin(string username, string password)
    { 
        int idUsuario = BD.Login(username,password);
        if (idUsuario == -1)
        {
            return View("Login");
        }
        else{
        BD.getFechaUltIngr(idUsuario);
        HttpContext.Session.SetString("ID",idUsuario.ToString());
        return RedirectToAction("verTareas", "Home");
        }
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult guardarRegistro(string username, string password, string nombre, string apellido, IFormFile foto)
    {
        string rutaFoto = null;

        if(foto != null && foto.Length > 0)
        {
            string nombreFoto = foto.FileName;
            string rutaCarpeta = Path.Combine(_env.WebRootPath, "Imagenes");

            if(!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            string rutaCompleta = Path.Combine(rutaCarpeta, nombreFoto);
            using(var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                foto.CopyTo(stream);
            }

            rutaFoto = "/Imagenes/" + nombreFoto;
        }


        BD.SignIn(username,password,nombre,apellido,rutaFoto,(DateTime.Today));
        Usuario usuario = BD.GetUsuario(username,password);
        HttpContext.Session.SetString("ID", usuario.ID.ToString());
        return RedirectToAction("verTareas", "Home");
    }

    public IActionResult logOut()
    {
        HttpContext.Session.Remove("ID");
        return RedirectToAction("Index", "Home");
    }
}