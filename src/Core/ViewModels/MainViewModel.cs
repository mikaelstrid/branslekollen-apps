﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class MainViewModel
    {
        private readonly IVehicleService _vehicleService;

        public MainViewModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _vehicleService.GetAll();
        } 
    }
}
