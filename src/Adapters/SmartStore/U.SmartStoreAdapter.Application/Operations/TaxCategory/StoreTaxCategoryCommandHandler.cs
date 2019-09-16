using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Models.Products;
using U.SmartStoreAdapter.Application.Models.Taxes;
using U.SmartStoreAdapter.Application.Validators;
using U.SmartStoreAdapter.Domain.Entities.Seo;

namespace U.SmartStoreAdapter.Application.Operations.TaxCategory
{
    public class StoreTaxCategoryCommandHandler : IRequestHandler<StoreTaxCategoryCommand, TaxResponse>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;
        private readonly StoreTaxCategoryCommandValidator _validator;

        public StoreTaxCategoryCommandHandler(SmartStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _validator = new StoreTaxCategoryCommandValidator();
        }

        public async Task<TaxResponse> Handle(StoreTaxCategoryCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var taxCategory = new Domain.Entities.Tax.TaxCategory
                    {
                        Name = request.Name
                    };

                    await _context.AddAsync(taxCategory, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    var setting = new Setting
                    {
                        Name = $"tax.taxprovider.fixedrate.taxcategoryid{taxCategory.Id}", // todo hardcoded
                        Value = request.Value.ToString(),
                        StoreId = 0
                    };
                    await _context.AddAsync(setting, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    
                    transaction.Commit();
                    return new TaxResponse
                    {
                        Settings = new SettingsDto
                        {
                            Name = setting.Name,
                            Value = setting.Value
                        },
                        TaxCategory = new TaxCategoryDto
                        {
                            Name = taxCategory.Name
                        }
                        
                    };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}