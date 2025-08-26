using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06_ToDoList.Models;

namespace TP06_ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult verTareas()
    {
        int idUsuario = int.Parse(HttpContext.Session.GetString("ID"));
        ViewBag.listaTareas = BD.getListaTareas(idUsuario);
        return View();
    }

    public IActionResult verTarea(int idTarea)
    {
        ViewBag.Tarea = BD.verTarea(idTarea);
        return View();
    }

    public IActionResult agregarTarea()
    {
        return View();
    }

    [HttpPost]
    public IActionResult agregarTarea(string titulo, string descripcion, DateTime fecha)
    {
        int idUsuario = int.Parse(HttpContext.Session.GetString("ID"));
        BD.crearTarea(titulo, descripcion, fecha,idUsuario);
        return RedirectToAction("verTareas");
    }

    public IActionResult modificarTarea(int IDTarea)
    {
        ViewBag.tarea = BD.verTarea(IDTarea);
        return View();
    }
    
    [HttpPost]
    public IActionResult modificarTarea(string titulo, string descripcion, DateTime fecha, string opcionFinalizar, int idTarea)
    {
        int idUsuario = int.Parse(HttpContext.Session.GetString("ID"));


        bool finalizarBool = false;
        if(opcionFinalizar == "Finalizada")
        {
            finalizarBool = true;
        }else if(opcionFinalizar == "Por hacer")
        {
            finalizarBool = false;
        }

        BD.editarTarea(titulo, descripcion, fecha, finalizarBool,idUsuario, idTarea);
        return RedirectToAction("verTareas");
    }

    public IActionResult eliminarTarea(int idTarea)
    {
        BD.borrarTarea(idTarea);
        return RedirectToAction("verTareas");
    }

    public IActionResult finalizarTarea(int idTarea)
    {
        BD.marcarTareaFinalizada(idTarea);
        return RedirectToAction("verTareas");
    }

}
