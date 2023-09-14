﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
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
        public async Task<IActionResult> IndexVilla()
        {

            List<VillaDto> villaList = new();

            var response = await _villaService.ObtenerTodos<APIResponse>();

            if(response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Resultado));
            }

            return View(villaList);
        }

        //El primero llamada a la vista y el segundo envia la informacion.
        //Get
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
                var response = await _villaService.Crear<APIResponse>(modelo);

                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(modelo );
        }
    }
}
