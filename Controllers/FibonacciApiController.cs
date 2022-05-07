using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Fibonacci.Data;
using Fibonacci.Entities;
using Fibonacci.Helpers;
using Fibonacci.Models;
using Fibonacci.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace Fibonacci.Controllers
{
    [ApiController]
    [Route("fibonacci")]
    public class FibonacciApiController : ControllerBase
    {
        private readonly IHistoricEntryRepository _historicEntryRepository;
        private readonly IMapper _mapper;

        public FibonacciApiController(
            IHistoricEntryRepository historicEntryRepository,
            IMapper mapper)
        {
            _historicEntryRepository = historicEntryRepository ?? throw new ArgumentNullException(nameof(historicEntryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{input}", Name = "GetFibonacci")]
        public IActionResult GetNthFibonacci(int input)
        {
            if (input < 1)
            {
                return BadRequest("Input must be greater than 0");
            }
            if (input > 300)
            {
                return BadRequest("Input must be less than 300");
            }

            var existingEntry = _historicEntryRepository.Get(input);

            var newEntry = new HistoricEntry
            {
                Input = input,
                DateCreated = DateTime.UtcNow
            };

            if (existingEntry == null)
            {
                newEntry.Result = Calculator.Fibonacci(input).ToString();
            }
            else
            {
                newEntry.Result = existingEntry.Result;
            }

            _historicEntryRepository.Add(newEntry);
            _historicEntryRepository.Save();

            return Ok(newEntry.Result);
        }

        [HttpGet(Name = "GetAllFibonacci")]
        public IActionResult Get([FromQuery] ResourceParameters resourceParameters)
        {
            var entries = _historicEntryRepository.GetAll(resourceParameters);

            var pagination = new
            {
                totalCount = entries.TotalCount,
                pageSize = entries.PageSize,
                currentPage = entries.CurrentPage,
                totalPages = entries.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

            var links = CreateLinks(resourceParameters, entries.HasNext, entries.HasPrevious);

            return Ok(new { result = _mapper.Map<IEnumerable<HistoricEntryDto>>(entries), links });
        }

        private IEnumerable<LinkDto> CreateLinks(
            ResourceParameters resourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            links.Add(
               new LinkDto(CreateResourceUri(
                   resourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateResourceUri(
                      resourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateResourceUri(
                        resourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private string CreateResourceUri(
           ResourceParameters resourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAllFibonacci",
                      new
                      {
                          searchNumber = resourceParameters.SearchNumber,
                          pageNumber = resourceParameters.PageNumber - 1,
                          pageSize = resourceParameters.PageSize,
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAllFibonacci",
                      new
                      {
                          searchNumber = resourceParameters.SearchNumber,
                          pageNumber = resourceParameters.PageNumber + 1,
                          pageSize = resourceParameters.PageSize,
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAllFibonacci",
                    new
                    {
                        searchNumber = resourceParameters.SearchNumber,
                        pageNumber = resourceParameters.PageNumber,
                        pageSize = resourceParameters.PageSize,
                    });
            }

        }
    }
}