using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleApi.Models;
using SampleApi.Validators;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly ILogger<FamilyController> _logger;
        private readonly IBaseValidator<ParentModel> _parentValidator;

        public FamilyController(ILogger<FamilyController> logger, IBaseValidator<ParentModel> parentValidator)
        {
            _logger = logger;
            _parentValidator = parentValidator;
        }

        [HttpPost]
        public async Task<IActionResult> AddParent(ParentModel parent)
        {
            // shallow validation (quick and without calls to data storage or external resources)
            await _parentValidator.ShallowValidateAndThrowAsync(parent);
            
            // add some additional checks, principals, user permissions etc

            // deep validation (calls to storage, external resources)
            // !!! THIS model does no pass deep validation on purpose !!!
            await _parentValidator.ValidateAndThrowAsync(parent);

            // save model
            
            return Ok();
        }
    }
}