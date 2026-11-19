using Microsoft.Extensions.Logging;
using BussinessLayer.Interface;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.Label.ResponseModel;

namespace BussinessLayer.Label.Query
{
    public class GetLabelQueryHandler : IGetLabelQueryHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly ILogger<GetLabelQueryHandler> _logger;

        public GetLabelQueryHandler(ILabelBL labelBL, ILogger<GetLabelQueryHandler> logger)
        {
            _labelBL = labelBL;
            _logger = logger;
        }

        public async Task<List<LabelResponseModel>> HandleAsync(GetLabelQuery query)
        {
            _logger.LogInformation(
                "Retrieving labels for user {UserId}",
                query.UserId);
            try
            {
                var labels = await _labelBL.GetMyLabelsAsync(
                    query.UserId);

                _logger.LogInformation(
                    "Retrieved {LabelCount} labels for user {UserId}",
                    labels.Count,
                    query.UserId);

                return labels;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve labels for user {UserId}",
                    query.UserId);

                throw;
            }
        }
    }
}

