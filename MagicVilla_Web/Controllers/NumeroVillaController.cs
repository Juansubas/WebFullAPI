﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
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

        public async Task<IActionResult> IndexNumeroVilla() 
        {
            List<NumeroVillaDto> numeroVillaList = new();

            var response = await _numeroVillaService.ObtenerTodos<APIResponse>();

            if(response != null && response.IsSuccess)
            {
                numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(numeroVillaList);
        }

        public async Task<IActionResult> CrearNumeroVilla()
        {
            NumeroVillaViewModel numeroVillaVM = new();

            var response = await _villaService.ObtenerTodos<APIResponse>();
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
                var response = await _numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla);
                if(response != null && response.IsSuccess)
                {
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

            var res = await _villaService.ObtenerTodos<APIResponse>();
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
    }
}
