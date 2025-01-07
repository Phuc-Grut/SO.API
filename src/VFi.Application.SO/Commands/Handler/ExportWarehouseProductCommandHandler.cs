using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler
{
    internal class ExportWarehouseProductCommandHandler : CommandHandler,
    IRequestHandler<ExportWarehouseProductDeleteCommand, ValidationResult>
    {
        private readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
        private readonly IExportWarehouseRepository _exportWarehouseRepository;

        public ExportWarehouseProductCommandHandler(IExportWarehouseProductRepository exportWarehouseProductRepository, IExportWarehouseRepository exportWarehouseRepository)
        {
            _exportWarehouseProductRepository = exportWarehouseProductRepository;
            _exportWarehouseRepository = exportWarehouseRepository;
        }

        public void Dispose()
        {
            _exportWarehouseProductRepository.Dispose();
        }
        public async Task<ValidationResult> Handle(ExportWarehouseProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var obj = await _exportWarehouseProductRepository.GetById(request.Id);

            if (!request.IsValid(_exportWarehouseProductRepository))
                return request.ValidationResult;

            var ExportWarehouseProduct = new ExportWarehouseProduct
            {
                Id = request.Id
            };
            _exportWarehouseProductRepository.Remove(ExportWarehouseProduct);

            var ewList = await _exportWarehouseProductRepository.GetByExportWarehouseId((Guid)obj.ExportWarehouseId);
            if (ewList.Count() == 1)
            {
                var exportWarehouse = new ExportWarehouse
                {
                    Id = (Guid)obj.ExportWarehouseId
                };
                _exportWarehouseRepository.Remove(exportWarehouse);
                return await Commit(_exportWarehouseRepository.UnitOfWork);
            }
            return await Commit(_exportWarehouseProductRepository.UnitOfWork);
        }
    }
}
