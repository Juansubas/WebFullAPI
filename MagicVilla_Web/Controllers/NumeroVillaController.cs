﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using MagivVilla_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class NumeroVillaController : Controller
    {
        private readonly INumeroVillaService _numeroVillaService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public NumeroVillaController(INumeroVillaService numeroVillaService, IMapper mapper, IVillaService villaService) 
        { 
            _numeroVillaService = numeroVillaService;
            _villaService = villaService;
            _mapper = mapper;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexNumeroVilla() 
        {
            List<NumeroVillaDto> numeroVillaList = new();

            var response = await _numeroVillaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccess)
            {
                numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(numeroVillaList);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearNumeroVilla()
        {
            NumeroVillaViewModel numeroVillaVM = new();

            var response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if(response != null && response.IsSuccess )
            {
                numeroVillaVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Name,
                                              Value = v.Id.ToString(),
                                          });
            }

            return View(numeroVillaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearNumeroVilla(NumeroVillaViewModel modelo)
        {
            if( ModelState.IsValid )
            {
                var response = await _numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if(response != null && response.IsSuccess)
                {
                    TempData["exitoso"] = "Numero Villa Creado Exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if( response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());

                    }
                }
            }

            var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (res != null && res.IsSuccess)
            {
                modelo.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Name,
                                              Value = v.Id.ToString(),
                                          });
            }

            return View(modelo);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarNumeroVilla(int villaNo)
        {
            NumeroVillaUpdateViewModel numeroVillaVM = new();

            var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccess)
            {
                NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = _mapper.Map<NumeroVillaUpdateDto>(modelo);
            }

            response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.IsSuccess)
            {
                numeroVillaVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Name,
                                              Value = v.Id.ToString(),
                                          });
                return View(numeroVillaVM);
            }

            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarNumeroVilla(NumeroVillaUpdateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await _numeroVillaService.Actualizar<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["exitoso"] = "Numero Villa Actualizada Exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());

                    }
                }
            }

            var res = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (res != null && res.IsSuccess)
            {
                modelo.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(res.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Name,
                                              Value = v.Id.ToString(),
                                          });
            }

            return View(modelo);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarNumeroVilla(int villaNo)
        {
            NumeroVillaDeleteViewModel numeroVillaVM = new();

            var response = await _numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsSuccess)
            {
                NumeroVillaDto modelo = JsonConvert.DeserializeObject<NumeroVillaDto>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = modelo;
            }

            response = await _villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.IsSuccess)
            {
                numeroVillaVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Name,
                                              Value = v.Id.ToString(),
                                          });
                return View(numeroVillaVM);
            }

            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarNumeroVilla(NumeroVillaDeleteViewModel modelo)
        {
            var response = await _numeroVillaService.Remover<APIResponse>(modelo.NumeroVilla.VillaNo, HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsSuccess)
            {
                TempData["exitoso"] = "Numero Villa Eliminado Exitosamente";
                return RedirectToAction(nameof(IndexNumeroVilla));
            }

            TempData["error"] = "Un error Ocurrio al Remover";
            return View(modelo);
        }
    }
}
