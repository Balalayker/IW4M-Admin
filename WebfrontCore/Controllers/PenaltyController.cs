﻿using Microsoft.AspNetCore.Mvc;
using SharedLibraryCore;
using SharedLibraryCore.Database.Models;
using SharedLibraryCore.Dtos;
using SharedLibraryCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebfrontCore.ViewComponents;

namespace WebfrontCore.Controllers
{
    public class PenaltyController : BaseController
    {
        public IActionResult List()
        {
            ViewBag.Description = "List of all the recent penalties (bans, kicks, warnings) on IW4MAdmin";
            ViewBag.Title = "Client Penalties";
            ViewBag.Keywords = "IW4MAdmin, penalties, ban, kick, warns";

            return View();
        }

        public async Task<IActionResult> ListAsync(int offset = 0)
        {
            return await Task.FromResult(View("_List", offset));
        }

        public async Task<IActionResult> PublicAsync()
        {
            var penalties = await (new GenericRepository<EFPenalty>())
                .FindAsync(p => p.Type == SharedLibraryCore.Objects.Penalty.PenaltyType.Ban && p.Active);

            var penaltiesDto = penalties.Select(p => new PenaltyInfo()
            {
                OffenderId = p.OffenderId,
                Offense = p.Offense,
                PunisherId = p.PunisherId,
                Type = p.Type.ToString(),
                TimePunished = p.When.ToString(),
                TimeRemaining = p.Expires.ToString()
            }).ToList();

            return Json(penaltiesDto);
        }
    }
}
