﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using MagivVilla_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;   
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexVilla()
        {

            List<VillaDto> villaList = new();

            var response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(villaList);
        }

        //El primero llamada a la vista y el segundo envia la informacion.
        //Get
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearVilla()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearVilla(VillaCreateDto modelo)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.Crear<APIResponse>(modelo, HttpContext.Session.GetString(DS.SessionToken));

                if(response != null && response.IsSuccess)
                {
                    TempData["exitoso"] = "Villa Creada Exitosamente";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(modelo );
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarVilla(int villaId)
        {
            var response = await _villaService.Obtener<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccess)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Resultado));
                return View(_mapper.Map<VillaUpdateDto>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarVilla(VillaUpdateDto modelo)
        {
            if(ModelState.IsValid) 
            {
                var response = await _villaService.Actualizar<APIResponse>(modelo, HttpContext.Session.GetString(DS.SessionToken));

                if(response != null && response.IsSuccess)
                {
                    TempData["exitoso"] = "Villa Actualizada Exitosamente";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(modelo);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarVilla(int villaId)
        {
            var response = await _villaService.Obtener<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsSuccess)
            {
                VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Resultado));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarVilla(VillaDto modelo)
        {

            var response = await _villaService.Remover<APIResponse>(modelo.Id, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsSuccess)
            {
                TempData["exitoso"] = "Villa Eliminada exitosamente";
                return RedirectToAction(nameof(IndexVilla));
            }

            TempData["error"] = "Ocurrio un Error al Remover";
            return View(modelo);
        }
    }
}
